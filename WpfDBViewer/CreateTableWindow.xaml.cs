using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfDBViewer
{
    /// <summary>
    /// Логика взаимодействия для CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        public CreateWindow()
        {
            InitializeComponent();
            ObservableCollection<Column> columns = new ObservableCollection<Column>();
        }

        private void CreateTable(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            if (DbInfoClass.dbName != null)
            {
                var doc = new TextRange(Columns.Document.ContentStart, Columns.Document.ContentEnd);
                Regex regex = new Regex(@"\n", RegexOptions.Compiled);
                var text = regex.Replace((doc.Text), "").ToString().Split('\r');
                string[] columns = text.Where(a => !String.IsNullOrEmpty(a)).ToArray();

                using (var conn = new SQLiteConnection($"DataSource={DbInfoClass.dbName};Version=3;"))
                {
                    conn.Open();
                    string columnsCom = "";
                    foreach (var item in columns)
                    {
                        columnsCom += $", {item} TEXT";
                    }
                    var command = new SQLiteCommand($"CREATE TABLE {tableName.Text} (id INTEGER PRIMARY KEY AUTOINCREMENT{columnsCom})", conn);
                    command.ExecuteNonQuery();
                    flag = true;
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Не выбрана бд");
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "База данных (*.sqlite)|*.sqlite";
                dialog.FilterIndex = 2;

                Nullable<bool> result = dialog.ShowDialog();

                if (result == true)
                {
                    DbInfoClass.dbName = dialog.SafeFileName;
                    DbInfoClass.dbPath = dialog.FileName;
                }
            }
            MainWindow mw = new MainWindow();
            mw.UpdateUI();
            if (flag)
            {
                this.Close();
            }
        }

        public class Column : INotifyPropertyChanged
        {
            private string name;
            private string type;

            public string Name
            {
                get { return name; }
                set
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
            public string Type
            {
                get { return type; }
                set
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName]string prop = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }
    }
}
