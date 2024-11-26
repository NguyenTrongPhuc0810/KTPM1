using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VST111.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            System.Mvc.Engine.Register(this, result => { 
            });

            SearchBox.PreviewKeyUp += (s, e) => { 
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    var txt = SearchBox.Text.Trim();
                    if (string.IsNullOrEmpty(txt)) return;

                    var cname = System.Mvc.Engine.RequestContext.ControllerName;
                    App.Request(cname + "/search", txt);
                }
            };

            App.Request("home");
        }

        // Phương thức xử lý sự kiện GotFocus
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Search...")
            {
                textBox.Text = "";
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        // Phương thức xử lý sự kiện LostFocus
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search...";
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void Function1_Click(object sender, RoutedEventArgs e)
        {
            var data = new Provider().Select("Facility");
            DataGridTable.ItemsSource = data.DefaultView;
            DataGridTable.Visibility = Visibility.Visible; // Show the DataGrid
        }
    }

}
