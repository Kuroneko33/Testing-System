using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace TestingSystem
{
    /// <summary>
    /// Interaction logic for TestResults.xaml
    /// </summary>
    public partial class TestResults : Window
    {
        SqlConnection sqlConnection;
        public TestResults()
        {
            InitializeComponent();
            string connectionString = Connection.connectionString;
            sqlConnection = new SqlConnection(connectionString);
        }

        public async Task LoadTest(int Tests_id)
        {
            //Открытие подключения
            await sqlConnection.OpenAsync();

            //Чтение id добавленного ТЕСТА в базе данных для удаления ВОПРОСОВ
            SqlDataReader dataReader = null;
            SqlCommand sqlCommandSELECT = new SqlCommand($"SELECT TOP 150 * From [StudentsTestResults] WHERE Tests_id=N'{Tests_id}'", sqlConnection);

            List<int> Students_id = new List<int>();
            List<int> StudentScore = new List<int>();
            List<int> AllTestScore = new List<int>();
            List<int> Mark = new List<int>();

            try
            {
                dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    Students_id.Add(Convert.ToInt32(dataReader["Students_id"]));
                    StudentScore.Add(Convert.ToInt32(dataReader["StudentScore"]));
                    AllTestScore.Add(Convert.ToInt32(dataReader["AllTestScore"]));
                    Mark.Add(Convert.ToInt32(dataReader["Mark"]));
                }
                dataReader.Close();

                for (int i = 0; i < Students_id.Count; i++)
                {
                    sqlCommandSELECT = new SqlCommand($"SELECT * FROM [Students] WHERE Id=N'{Students_id[i]}'", sqlConnection);
                    dataReader = null;

                    try
                    {
                        dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                        await dataReader.ReadAsync();
                        string Name = (String)(Convert.ToString(dataReader["Name"])).Trim(' ');
                        string Surname = (String)(Convert.ToString(dataReader["Surname"])).Trim(' ');
                        string StGroup = (String)(Convert.ToString(dataReader["StGroup"])).Trim(' ');
                        string Number = (String)(Convert.ToString(dataReader["Number"])).Trim(' ');

                        AddTestResult(Name, Surname, StGroup, Number, StudentScore[i], AllTestScore[i], Mark[i]);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        dataReader.Close();
                    }
                }
                Owner.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
            }

            sqlConnection.Close();
        }

        private void AddTestResult(string Name, string Surname, string StGroup, string Number, int StudentScore, int AllTestScore, int Mark)
        {
            if (Name == "")
            {
                Name = "Неизвестно";
            }
            if (Surname == "")
            {
                Surname = "Неизвестно";
            }
            if (StGroup == "")
            {
                StGroup = "Неизвестно";
            }
            if (Number == "")
            {
                Number = "Неизвестно";
            }

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            TextBlock student = new TextBlock();
            student.FontSize = 15;
            student.TextWrapping = TextWrapping.Wrap;
            student.Text = Surname + "    " + Name + "    " + StGroup + "    " + Number;
            student.Margin = new Thickness(10);
            student.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(student, 0);

            TextBlock results = new TextBlock();
            results.FontSize = 15;
            results.TextWrapping = TextWrapping.Wrap;
            results.VerticalAlignment = VerticalAlignment.Center;
            results.Text = "Баллов: " + StudentScore + "/" + AllTestScore + "   Оценка: " + Mark; ;
            results.Margin = new Thickness(10);
            Grid.SetColumn(results, 1);

            grid.Children.Add(student);
            grid.Children.Add(results);

            TestResultsListBox.Items.Insert(0,grid);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Show();
        }
    }
}
