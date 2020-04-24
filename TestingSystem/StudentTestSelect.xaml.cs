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
    /// Interaction logic for StudentTestSelect.xaml
    /// </summary>
    public partial class StudentTestSelect : Window
    {
        SqlConnection sqlConnection;
        private int student_id = 0;
        public StudentTestSelect(int student_id)
        {
            InitializeComponent();
            string connectionString = Connection.connectionString;
            sqlConnection = new SqlConnection(connectionString);
            this.student_id = student_id;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                TestsListBox.Items.Clear();
                await LoadTestsListBox();
                await Task.Delay(2000);
            }
        }

        private async Task LoadTestsListBox()
        {
            //Открытие подключения
            await sqlConnection.OpenAsync();

            //Чтение Тестов
            SqlCommand sqlCommandSELECT = new SqlCommand($"SELECT * FROM [Tests]", sqlConnection);
            SqlDataReader dataReader = null;
            try
            {
                dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    AddTestToTestsListBox(Convert.ToString(dataReader["Name"]), Convert.ToString(dataReader["Status"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                dataReader.Close();
            }
            sqlConnection.Close();
        }

        private void AddTestToTestsListBox(string testName, string status)
        {
            status = status.Trim(' ');
            //Добавление элемента списка тестов 
            if (status == "Открыт")
            {
                Grid grid = new Grid();
                grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                TextBlock nameTextBlock = new TextBlock();
                nameTextBlock.FontSize = 20;
                nameTextBlock.TextWrapping = TextWrapping.Wrap;
                nameTextBlock.Text = testName;
                nameTextBlock.Margin = new Thickness(10);
                grid.Children.Add(nameTextBlock);
                TestsListBox.Items.Add(grid);
            }
        }

        private async void ReloadTest_Click(object sender, RoutedEventArgs e)
        {
            TestsListBox.Items.Clear();
            await LoadTestsListBox();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
                new Authorization().Show();
        }

        private async void TestsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestsListBox.SelectedIndex >= 0)
            {
                if (TestsListBox.Items[TestsListBox.SelectedIndex] is Grid grid)
                {
                    if (grid.Children[0] is TextBlock testNameTextBox)
                    {
                        string testName = testNameTextBox.Text;
                        StudentTestPassing StudentTestPassingWindow = new StudentTestPassing(TestsListBox, student_id) { Owner = this };
                        StudentTestPassingWindow.Show();
                        await StudentTestPassingWindow.LoadTest(testName);
                        this.Hide();
                    }
                }
            }
        }
    }
}
