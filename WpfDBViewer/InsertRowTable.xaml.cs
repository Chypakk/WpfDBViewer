using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

namespace WpfDBViewer
{
    /// <summary>
    /// Логика взаимодействия для InsertRowTable.xaml
    /// </summary>
    public partial class InsertRowTable : Window
    {
        public InsertRowTable()
        {
            InitializeComponent();
            using (var conn = new SQLiteConnection($"DataSource={DbInfoClass.dbName};Version=3;"))
            {
                DataTable dataTable = new DataTable();
                conn.Open();
                var command = new SQLiteCommand($"SELECT * FROM {DbInfoClass.selectedTable}", conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (i != 0)
                    {
                        var row = new RowDefinition();
                        Rows.RowDefinitions.Add(row);

                        var label = new TextBlock();
                        label.Text = dataTable.Columns[i].ColumnName;
                        label.Tag = dataTable.Columns[i].ColumnName;
                        label.HorizontalAlignment = HorizontalAlignment.Center;
                        label.VerticalAlignment = VerticalAlignment.Center;
                        Rows.Children.Add(label);

                        var textBox = new TextBox();
                        textBox.Name = dataTable.Columns[i].ColumnName;
                        textBox.Tag = dataTable.Columns[i].ColumnName + "TextBox";
                        textBox.HorizontalAlignment = HorizontalAlignment.Center;
                        textBox.VerticalAlignment = VerticalAlignment.Center;
                        textBox.Height = 25;
                        textBox.Width = 300;
                        Rows.Children.Add(textBox);

                        Grid.SetRow(label, Rows.RowDefinitions.Count - 1);
                        Grid.SetRow(textBox, Rows.RowDefinitions.Count - 1);
                    }
                }

                bool flag = true;
                foreach (var item in Rows.Children)
                {
                    if (flag)
                    {
                        Grid.SetColumn((TextBlock)item, 0);
                        flag = false;
                    }
                    else
                    {
                        Grid.SetColumn((TextBox)item, 1);
                        flag = true;
                    }
                }

                var rowButton = new RowDefinition();
                Rows.RowDefinitions.Add(rowButton);

                var button = new Button();
                button.Content = "Сохранить";
                button.Width = 100;
                button.Height = 25;
                button.Click += InsertRow;
                Rows.Children.Add(button);

                Grid.SetColumnSpan(button, 2);
                Grid.SetRow(button, Rows.RowDefinitions.Count - 1);
            }
        }

        private void InsertRow(object sender, RoutedEventArgs e)
        {
            List<TextBox> insertColumns = new List<TextBox>();
            foreach (var item in Rows.Children)
            {
                if (item is TextBox)
                {
                    insertColumns.Add((TextBox)item);
                }
            }

            var columns = "";
            var values = "";
            for (int i = 0; i < insertColumns.Count; i++)
            {
                if (i == insertColumns.Count - 1)
                {
                    columns += $"'{insertColumns[i].Name}'";
                    values += $"'{insertColumns[i].Text}'";
                }
                else
                {
                    columns += $"'{insertColumns[i].Name}', ";
                    values += $"'{insertColumns[i].Text}', ";
                }
            }

            using (var conn = new SQLiteConnection($"DataSource={DbInfoClass.dbName};Version=3;"))
            {
                DataTable dataTable = new DataTable();
                conn.Open();
                try
                {
                    var command = new SQLiteCommand($"INSERT INTO {DbInfoClass.selectedTable} ({columns}) values ({values})", conn);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MainWindow mw = (MainWindow)this.Owner;
                mw.UpdateTable();
                this.Close();

            }
        }
    }
}
