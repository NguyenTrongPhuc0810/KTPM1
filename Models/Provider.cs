using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace System
{
    public static class Migrate
    {
        public class Column
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public override string ToString()
            {
                return $"public {Type} {Name} {{ get; set; }}";
            }
        }
        public class Table
        {
            public string Name { get; set; }
            public List<Column> Columns { get; set; } = new List<Column>();

            public override string ToString()
            {
                var s = new List<string> { 
                    $"public class {Name}\r\n{{",
                };
                foreach (var c in Columns)
                    s.Add("\t" + c);
                s.Add("}");

                return string.Join("\r\n", s);
            }
        }
        static Dictionary<string, string> _dataTypes = new Dictionary<string, string> {
            { "varbinary", "byte[]" },
            { "binary", "byte[]" },
            { "image", "string" },
            { "varchar", "string" },
            { "char", "string" },
            { "nvarchar", "string" },
            { "nchar", "string" },
            { "text", "string" },
            { "ntext", "string" },
            { "uniqueidentifier", "Guid" },
            { "rowversion", "byte[]" },
            { "bit", "bool" },
            { "tinyint", "byte" },
            { "smallint", "short" },
            { "int", "int" },
            { "bigint", "long" },
            { "smallmoney", "decimal" },
            { "money", "decimal" },
            { "numeric", "decimal" },
            { "decimal", "decimal" },
            { "real", "single" },
            { "float", "double" },
            { "smalldatetime", "DateTime" },
            { "datetime", "DateTime" },
            { "sql_variant", "object" },
            { "table", "string" },
            { "cursor", "string" },
            { "timestamp", "string" },
            { "xml", "string" },
        };
        static Dictionary<string, Table> _tables { get; set; }
        static public IEnumerable<Table> Load()
        {
            var pro = new Provider();
            var rows = pro.LoadTable("SELECT * FROM INFORMATION_SCHEMA.TABLES").Rows;

            _tables = new Dictionary<string, Table>();
            foreach (DataRow r in rows)
            {
                var name = r["TABLE_NAME"].ToString();
                _tables.Add(name, new Table { 
                    Name = name
                });
            }

            rows = pro.LoadTable("SELECT * FROM INFORMATION_SCHEMA.COLUMNS").Rows;
            foreach (DataRow r in rows)
            {
                var name = r["TABLE_NAME"].ToString();
                var tabl = _tables[name];
                tabl.Columns.Add(new Column { 
                    Name = r["COLUMN_NAME"].ToString(),
                    Type = _dataTypes[r["DATA_TYPE"].ToString()],
                });
            }
            return _tables.Values;
        }
        static public List<T> ToList<T>(this DataTable table) where T: new()
        {
            var lst = new List<T>();
            var typ = typeof(T);

            var cols = new Dictionary<PropertyInfo, DataColumn>();
            foreach (DataColumn c in table.Columns)
            {
                var prop = typ.GetProperty(c.ColumnName);
                if (prop != null && prop.CanWrite)
                {
                    cols.Add(prop, c);
                }
            }

            foreach (DataRow r in table.Rows)
            {
                var e = new T();
                foreach (var p in cols)
                {
                    object v = r[p.Value];
                    if (v != DBNull.Value)
                    {
                        p.Key.SetValue(e, r[p.Value]);
                    }
                }
                lst.Add(e);
            }
            return lst;
        }
    }

    public class Provider
    {
        string _hostName = @"LOCALHOST";
        string _dataName = @"livestock";
        SqlConnection _conn;

        public string HostName
        {
            get => _hostName;
            set
            {
                _hostName = value;
                _conn = null;
            }
        }
        public string DataName
        {
            get => _dataName;
            set
            {
                _dataName = value;
                _conn = null;
            }
        }
        public string ConnectionString => $"Data Source={_hostName};Initial Catalog={_dataName};Integrated Security=True";
        public SqlConnection Open()
        {
            if (_conn == null)
                _conn = new SqlConnection(ConnectionString);
            if (_conn.State != ConnectionState.Open)
                _conn.Open();

            return _conn;
        }
        public void Close()
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
                _conn.Close();
        }
        public void CreateCommand(Action<SqlCommand> callback)
        {
            var cmd = new SqlCommand {
                Connection = Open()
            };
            callback(cmd);
            Close();
        }
        /// <summary>
        /// Đọc dữ liệu từ table
        /// </summary>
        /// <param name="sql">Câu lệnh SQL (select)</param>
        /// <returns>Đối tượng kiểu DataTable</returns>
        public DataTable LoadTable(string sql)
        {
            DataTable table = new DataTable();
            table.BeginLoadData();

            CreateCommand(cmd => {
                cmd.CommandText = sql;

                var reader = cmd.ExecuteReader();
                table.Load(reader);
            });

            table.EndLoadData();
            return table;
        }
        /// <summary>
        /// Chạy câu lệnh SQL
        /// </summary>
        /// <param name="sql">Câu lệnh SQL</param>
        /// <returns>Ô đầu tiên của bảng dữ liệu, trả về null nếu không có hàng nào trong bảng</returns>
        public object Exec(string sql)
        {
            object result = null;
            CreateCommand(cmd => {
                cmd.CommandText = sql;
                result = cmd.ExecuteScalar();
            });
            return result;
        }
        /// <summary>
        /// Chạy chương trình con hoặc câu SQL (insert, update, delete)
        /// </summary>
        /// <param name="sql">Câu lệnh SQL</param>
        /// <returns>Số bản ghi được xử lý</returns>
        public int RunProc(string sql)
        {
            int result = 0;
            CreateCommand(cmd => {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                result = cmd.ExecuteNonQuery();
            });
            return result;
        }

        public DataTable Select(string tableName)
        {
            return Select(tableName, null, null);
        }
        public DataTable Select(string tableName, string filter)
        {
            return Select(tableName, filter, null);
        }
        public DataTable Select(string tableName, string filter, string sort)
        {
            var sql = "SELECT * FROM " + tableName;
            if (!string.IsNullOrWhiteSpace(filter))
                sql += " WHERE " + filter;
            if (!string.IsNullOrWhiteSpace(sort))
                sql += " ORDER BY " + sort;

            return LoadTable(sql);
        }
    }
}
