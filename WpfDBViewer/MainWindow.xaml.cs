using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDBViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ApplicationVM();
        }

        private void CreateTableWindow(object sender, RoutedEventArgs e)
        {
            CreateWindow createWindow = new CreateWindow();
            createWindow.Show();
        }

        private void CreateDBWindow(object sender, RoutedEventArgs e)
        {
            DbCreateWindow dbCreate = new DbCreateWindow();
            dbCreate.Show();

        }

        private void ShowDbName(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{DbInfoClass.dbName}");
        }

        private void OpenDb(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "База данных (*.sqlite)|*.sqlite";
            dialog.FilterIndex = 2;

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                DbInfoClass.dbName = dialog.SafeFileName;
                DbInfoClass.dbPath = dialog.FileName;
            }
            UpdateUI();
        }

        public void UpdateUI()
        {

            if (DbInfoClass.dbName != null)
            {
                using (var conn = new SQLiteConnection($"DataSource={DbInfoClass.dbName};Version=3;"))
                {
                    DataTable dataTable = new DataTable();
                    conn.Open();
                    var command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table'", conn);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(dataTable);
                    List<string> tableName = new List<string>();
                    if (dataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            var asd = dataTable.Rows[i].ItemArray.Last().ToString();
                            tableName.Add(dataTable.Rows[i].ItemArray.Last().ToString());
                        }
                    }
                    DbTree.ItemsSource = tableName;

                    //var items = command.ExecuteReader();
                    //
                    //while (items.Read())
                    //{
                    //    object table = items.GetValue(0);
                    //    tableName.Add(table.ToString());
                    //}
                    //DbTree.ItemsSource = tableName;
                }
            }

        }

        class Item
        {
            public string item { get; set; }
        }
    }
}
