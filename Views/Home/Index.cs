using DeviceConfig.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VST111.Views.Home
{
    internal class Index : BaseView<DataGrid, DataTable>
    {
    }
    internal class Search : BaseView<DataGrid, DataTable>
    {
        protected override void RenderCore()
        {
            base.RenderCore();
        }
    }
}
