using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SQLite;

namespace WpfDBViewer
{
    /// <summary>
    /// Логика взаимодействия для DbCreateWindow.xaml
    /// </summary>
    public partial class DbCreateWindow : Window
    {
        public DbCreateWindow()
        {
            InitializeComponent();
        }

        private void DbCreate(object sender, RoutedEventArgs e)
        {
            if (!File.Exists($"{DBName.Text}.sqlite"))
            {
                SQLiteConnection.CreateFile($"{DBName.Text}.sqlite");
                DbInfoClass.dbName = DBName.Text;
                MessageBox.Show("Бд успешна создана");
            }
            else
            {
                MessageBox.Show("Такая бд уже есть");
            }         
            this.Close();
        }
    }
}
