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
    /// Interaction logic for StudentTestPassing.xaml
    /// </summary>
    public partial class StudentTestPassing : Window
    {
        SolidColorBrush brushWatermarkBackground = new SolidColorBrush(Colors.White);
        SolidColorBrush transparent = new SolidColorBrush(Colors.White);
        int radioIndex = 0;
        int allTestScore = 0;
        public ListBox TestsList = new ListBox();
        SqlConnection sqlConnection;

        public StudentTestPassing(ListBox TestsListbox)
        {
            InitializeComponent();
            TestsList = TestsListbox;
            string connectionString = Connection.connectionString;
            sqlConnection = new SqlConnection(connectionString);
        }

        public async Task LoadTest(string testName)
        {
            //Открытие подключения
            await sqlConnection.OpenAsync();

            //Чтение id добавленного ТЕСТА в базе данных для удаления ВОПРОСОВ
            SqlDataReader dataReader = null;
            SqlCommand sqlCommandSELECT = new SqlCommand($"SELECT * From [Tests] WHERE Name=N'{testName}'", sqlConnection);
            try
            {
                dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                await dataReader.ReadAsync();
                int Tests_id = Convert.ToInt32(dataReader["Id"]);
                string Test_name = Convert.ToString(dataReader["Name"]);
                TestName.Text = Test_name;
                dataReader.Close();

                sqlCommandSELECT = new SqlCommand($"SELECT * FROM [Questions] WHERE Tests_id=N'{Tests_id}'", sqlConnection);
                dataReader = null;
                List<int> Questions_ids = new List<int>();
                List<string> Questions_names = new List<string>();
                List<string> Questions_types = new List<string>();
                List<int> Questions_points = new List<int>();
                allTestScore = 0;
                try
                {
                    dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                    while (await dataReader.ReadAsync())
                    {
                        Questions_ids.Add(Convert.ToInt32(dataReader["Id"]));
                        Questions_names.Add((String)(Convert.ToString(dataReader["Name"])).Trim(' '));
                        Questions_types.Add((String)(Convert.ToString(dataReader["Type"])).Trim(' '));
                        Questions_points.Add(Convert.ToInt32(dataReader["Points"]));
                        allTestScore += Convert.ToInt32(dataReader["Points"]);
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
                //цикл добавления вопросов
                for (int i = 0; i < Questions_ids.Count; i++)
                {
                    int QlistboxIndex = AddQuestion(Questions_types[i], false, Questions_names[i]);
                    ListBox answersList = null;
                    if (QListbox.Items[QlistboxIndex] is Grid grid)
                    {
                        if (grid.Children[1] is StackPanel stackPanel)
                        {
                            if (stackPanel.Children[0] is ListBox listBox)
                            {
                                answersList = listBox;
                            }
                        }
                    }
                    string Text = null;
                    string Correctness = null;

                    //цикл добавления ответов для каждого вопроса
                    sqlCommandSELECT = new SqlCommand($"SELECT * FROM [Answers] WHERE Questions_id=N'{Questions_ids[i]}'", sqlConnection);
                    dataReader = null;
                    try
                    {
                        dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            Text = Convert.ToString(dataReader["Text"]);
                            Correctness = "false";// (String)(Convert.ToString(dataReader["Correctness"])).Trim(' ');
                            if (Questions_types[i].Equals("Radio"))
                            {
                                if (answersList.Items.Count > 0)
                                {
                                    if (answersList.Items[0] is Grid answerGrid)
                                    {
                                        if (answerGrid.Children[0] is RadioButton rb)
                                        {
                                            answersList.Items.Add(AddRadioAnswer(rb.GroupName, Text, Correctness));
                                        }
                                    }
                                }
                                else
                                {
                                    answersList.Items.Add(AddRadioAnswer(radioIndex.ToString(), Text, Correctness));
                                    radioIndex++;
                                }
                            }
                            else if (Questions_types[i].Equals("Checkbox"))
                            {
                                answersList.Items.Add(AddCheckboxAnswer(Text, Correctness));
                            }
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
                }
            }
            catch (Exception)
            {
            }

            sqlConnection.Close();
        }

        private int AddQuestion(string type, bool addbutton = true, string Text = "")
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Grid rightGrid = new Grid();
            Grid.SetRowSpan(rightGrid, 2);
            Grid.SetColumn(rightGrid, 1);

            if (type.Equals("Radio"))
            {
                rightGrid.Children.Add(new TextBlock() { Text = "Один из списка\n\nОтметьте правильный ответ \nслева от варианта", Margin = new Thickness(10, 0, 10, 0), HorizontalAlignment = HorizontalAlignment.Left, FontSize = 15 });
            }
            else if (type.Equals("Checkbox"))
            {
                rightGrid.Children.Add(new TextBlock() { Text = "Несколько из списка\n\nОтметьте правильные ответы \nслева от варианта", Margin = new Thickness(10, 0, 10, 0), HorizontalAlignment = HorizontalAlignment.Left, FontSize = 15 });
            }

            StackPanel stackPanelQ = new StackPanel();
            stackPanelQ.Margin = new Thickness(5);

            Grid stackPanelQGrid = new Grid();

            transparent.Opacity = 0;
            stackPanelQGrid.Background = brushWatermarkBackground;

            stackPanelQGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            stackPanelQGrid.VerticalAlignment = VerticalAlignment.Center;

            TextBlock textBlock = new TextBlock();
            textBlock.Margin = new Thickness(2, 1, 0, 0);
            textBlock.Text = Text;
            textBlock.MinHeight = 20;

            stackPanelQGrid.Children.Add(textBlock);
            stackPanelQ.Children.Add(stackPanelQGrid);
            grid.Children.Add(stackPanelQ);

            StackPanel stackPanelA = new StackPanel();
            stackPanelA.Margin = new Thickness(5);

            ListBox answersList = new ListBox();
            answersList.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            answersList.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);

            if (addbutton)
            {
                if (rightGrid.Children[0] is TextBlock rightTextBlock)
                {
                    if (rightTextBlock.Text[0].Equals('О'))
                    {
                        if (answersList.Items.Count > 0)
                        {
                            if (answersList.Items[0] is Grid answerGrid)
                            {
                                if (answerGrid.Children[0] is RadioButton rb)
                                {
                                    answersList.Items.Add(AddRadioAnswer(rb.GroupName));
                                }
                            }
                        }
                        else
                        {
                            answersList.Items.Add(AddRadioAnswer(radioIndex.ToString()));
                            radioIndex++;
                        }
                    }
                    else if (rightTextBlock.Text[0].Equals('Н'))
                    {
                        answersList.Items.Add(AddCheckboxAnswer());
                    }
                }
            }

            stackPanelA.Children.Add(answersList);
            grid.Children.Add(stackPanelA);
            Grid.SetRow(stackPanelA, 1);

            grid.Children.Add(rightGrid);

            QListbox.Items.Add(grid);
            return QListbox.Items.Count - 1;
        }

        private Grid AddCheckboxAnswer(string Text = "", string Correctness = "false")
        {
            Grid aGrid = new Grid();
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            aGrid.ColumnDefinitions.Add(new ColumnDefinition());

            CheckBox checkBox = new CheckBox();
            checkBox.Margin = new Thickness(0, 2, 2, 0);
            if (Correctness.Equals("true"))
            {
                checkBox.IsChecked = true;
            }

            TextBlock textBlock1 = new TextBlock();
            textBlock1.Margin = new Thickness(2, 1, 0, 0);
            textBlock1.Text = Text;
            textBlock1.MinHeight = 20;

            aGrid.Children.Add(checkBox);
            aGrid.Children.Add(textBlock1);
            Grid.SetColumn(textBlock1, 1);

            return aGrid;
        }

        private Grid AddRadioAnswer(string group, string Text = "", string Correctness = "false")
        {
            Grid aGrid = new Grid();
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            aGrid.ColumnDefinitions.Add(new ColumnDefinition());

            RadioButton radioButton = new RadioButton();
            radioButton.Margin = new Thickness(0, 2, 2, 0);
            radioButton.GroupName = group;
            if (Correctness.Equals("true"))
            {
                radioButton.IsChecked = true;
            }

            TextBlock textBlock1 = new TextBlock();
            textBlock1.Margin = new Thickness(2, 1, 0, 0);
            textBlock1.Text = Text;
            textBlock1.MinHeight = 20;

            aGrid.Children.Add(radioButton);
            aGrid.Children.Add(textBlock1);
            Grid.SetColumn(textBlock1, 1);

            return aGrid;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            double studentScore = 0;

            //Подсчёт и сохранение результатов

            double mark = 10 * (studentScore / allTestScore);
            mark = Math.Round(mark);
            MessageBox.Show($"Вы набрали {studentScore} из {allTestScore} баллов \nВаша оценка: {mark} ", "Ваш результат", MessageBoxButton.OK, MessageBoxImage.Information);
            Owner.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Show();
        }
    }
}
