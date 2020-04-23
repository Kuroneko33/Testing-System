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
    /// Interaction logic for TeacherTestsEditor.xaml
    /// </summary>
    public partial class TeacherTestsEditor : Window
    {
        SqlConnection sqlConnection;
        public TeacherTestsEditor()
        {
            InitializeComponent();
            string connectionString = Connection.connectionString;
            sqlConnection = new SqlConnection(connectionString);
            LoadTestsListBox();
        }

        private async void LoadTestsListBox()
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
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            TextBlock nameTextBlock = new TextBlock();
            nameTextBlock.FontSize = 20;
            nameTextBlock.TextWrapping = TextWrapping.Wrap;
            nameTextBlock.Text = testName;
            nameTextBlock.Margin = new Thickness(10);
            Grid.SetColumn(nameTextBlock, 0);
            Grid.SetColumnSpan(nameTextBlock, 4);
            grid.Children.Add(nameTextBlock);
            TextBlock statusTextBlock = new TextBlock();
            statusTextBlock.FontSize = 20;
            statusTextBlock.Text = status;
            if (status == "Открыт")
            {
                statusTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            }
            statusTextBlock.Opacity = 0.8;
            statusTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            statusTextBlock.VerticalAlignment = VerticalAlignment.Center;
            statusTextBlock.Margin = new Thickness(10);
            Grid.SetColumn(statusTextBlock, 4);
            grid.Children.Add(statusTextBlock);
            TestsListBox.Items.Add(grid);
        }

        private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            new EditTest(TestsListBox) { Owner = this }.Show();
        }

        private async void OpenTest_Click(object sender, RoutedEventArgs e)
        {
            if (TestsListBox.SelectedIndex >= 0)
            {
                if (TestsListBox.Items[TestsListBox.SelectedIndex] is Grid grid)
                {
                    if (grid.Children[1] is TextBlock statusTextBlock)
                    {
                        string status = "Открыт";
                        statusTextBlock.Text = status;
                        statusTextBlock.Margin = new Thickness(10);
                        statusTextBlock.Foreground = new SolidColorBrush(Colors.Green);

                        if (grid.Children[0] is TextBlock testNameTextBox)
                        {
                            string testName = testNameTextBox.Text;
                            //Открытие подключения
                            await sqlConnection.OpenAsync();

                            //Обновление статуса
                            SqlCommand sqlCommandUPDATE = new SqlCommand($"UPDATE [Tests] SET [Status]=@Status WHERE [Name]=@Name", sqlConnection);
                            sqlCommandUPDATE.Parameters.AddWithValue("Name", testName);
                            sqlCommandUPDATE.Parameters.AddWithValue("Status", status);
                            await sqlCommandUPDATE.ExecuteNonQueryAsync();
                            sqlConnection.Close();

                        }
                    }
                }
            }
        }

        private async void CloseTest_Click(object sender, RoutedEventArgs e)
        {
            if (TestsListBox.SelectedIndex >= 0)
            {
                if (TestsListBox.Items[TestsListBox.SelectedIndex] is Grid grid)
                {
                    if (grid.Children[1] is TextBlock statusTextBlock)
                    {
                        string status = "Закрыт";
                        statusTextBlock.Text = status;
                        statusTextBlock.Margin = new Thickness(10);
                        statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);

                        if (grid.Children[0] is TextBlock testNameTextBox)
                        {
                            string testName = testNameTextBox.Text;
                            //Открытие подключения
                            await sqlConnection.OpenAsync();

                            //Обновление статуса
                            SqlCommand sqlCommandUPDATE = new SqlCommand($"UPDATE [Tests] SET [Status]=@Status WHERE [Name]=@Name", sqlConnection);
                            sqlCommandUPDATE.Parameters.AddWithValue("Name", testName);
                            sqlCommandUPDATE.Parameters.AddWithValue("Status", status);
                            await sqlCommandUPDATE.ExecuteNonQueryAsync();
                            sqlConnection.Close();

                        }
                    }
                }
            }
        }

        private async void DeleteTest_Click(object sender, RoutedEventArgs e)
        {
            if (TestsListBox.SelectedIndex >= 0)
            {
                if (TestsListBox.Items[TestsListBox.SelectedIndex] is Grid grid)
                {
                    if (grid.Children[1] is TextBlock)
                    {
                        if (grid.Children[0] is TextBlock testNameTextBox)
                        {
                            string testName = testNameTextBox.Text;
                            if(MessageBox.Show($"Удалить тест '{testName}'?", "Удаление теста", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                EditTest EditTestWindow = new EditTest(TestsListBox) { Owner = this };
                                await EditTestWindow.ClearOldTestData(testName);
                            }
                        }
                    }
                }
            }
        }

        private async void EditTest_Click(object sender, RoutedEventArgs e)
        {
            if (TestsListBox.SelectedIndex >= 0)
            {
                if (TestsListBox.Items[TestsListBox.SelectedIndex] is Grid grid)
                {
                    if (grid.Children[1] is TextBlock)
                    {
                        if (grid.Children[0] is TextBlock testNameTextBox)
                        {
                            string testName = testNameTextBox.Text;
                            EditTest EditTestWindow = new EditTest(TestsListBox) { Owner = this };
                            EditTestWindow.Show();
                            await EditTestWindow.LoadTest(testName);
                        }
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new Authorization().Show();
        }

        private void CheckResults_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
