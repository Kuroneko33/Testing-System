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
        int Tests_id = 0;
        int Student_id = 0;
        int radioIndex = 0;
        int allTestScore = 0;
        public ListBox TestsList = new ListBox();
        SqlConnection sqlConnection;

        public StudentTestPassing(ListBox TestsListbox, int Student_id)
        {
            InitializeComponent();
            TestsList = TestsListbox;
            string connectionString = Connection.connectionString;
            sqlConnection = new SqlConnection(connectionString);
            this.Student_id = Student_id;
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
                Tests_id = Convert.ToInt32(dataReader["Id"]);
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
                    int QlistboxIndex = AddQuestion(Questions_types[i], Questions_points[i], Questions_names[i]);
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
                            Correctness = (String)(Convert.ToString(dataReader["Correctness"])).Trim(' ');
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

        private int AddQuestion(string Type, int Points, string Text = "")
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Grid rightGrid = new Grid();
            Grid.SetRowSpan(rightGrid, 2);
            Grid.SetColumn(rightGrid, 1);

            if (Type.Equals("Radio"))
            {
                rightGrid.Children.Add(new TextBlock() { Text = "Один из списка\n\nОтметьте правильный ответ \nслева от варианта", Margin = new Thickness(10, 0, 10, 0), HorizontalAlignment = HorizontalAlignment.Left, FontSize = 15 });
            }
            else if (Type.Equals("Checkbox"))
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
            textBlock.TextWrapping = TextWrapping.Wrap;

            stackPanelQGrid.Children.Add(textBlock);
            stackPanelQ.Children.Add(stackPanelQGrid);
            grid.Children.Add(stackPanelQ);

            StackPanel stackPanelA = new StackPanel();
            stackPanelA.Margin = new Thickness(5);

            ListBox answersList = new ListBox();
            answersList.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            answersList.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            answersList.SelectionChanged += AnswersList_SelectionChanged;

            stackPanelA.Children.Add(answersList);
            grid.Children.Add(stackPanelA);
            Grid.SetRow(stackPanelA, 1);

            TextBlock points = new TextBlock();
            points.Text = Points.ToString() + " баллов";
            points.FontSize = 15;
            points.VerticalAlignment = VerticalAlignment.Top;
            points.HorizontalAlignment = HorizontalAlignment.Right;

            rightGrid.Children.Add(points);
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

            CheckBox gostCheckBox = new CheckBox();
            gostCheckBox.Margin = new Thickness(0, 2, 2, 0);
            gostCheckBox.Visibility = Visibility.Collapsed;
            if (Correctness.Equals("true"))
            {
                gostCheckBox.IsChecked = true;
            }

            TextBlock textBlock1 = new TextBlock();
            textBlock1.Margin = new Thickness(2, 1, 0, 0);
            textBlock1.Text = Text;
            textBlock1.MinHeight = 20;

            aGrid.Children.Add(checkBox);
            aGrid.Children.Add(textBlock1);
            aGrid.Children.Add(gostCheckBox);
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

            CheckBox gostCheckBox = new CheckBox();
            gostCheckBox.Margin = new Thickness(0, 2, 2, 0);
            gostCheckBox.Visibility = Visibility.Collapsed;
            if (Correctness.Equals("true"))
            {
                gostCheckBox.IsChecked = true;
            }

            TextBlock textBlock1 = new TextBlock();
            textBlock1.Margin = new Thickness(2, 1, 0, 0);
            textBlock1.Text = Text;
            textBlock1.MinHeight = 20;

            aGrid.Children.Add(radioButton);
            aGrid.Children.Add(textBlock1);
            aGrid.Children.Add(gostCheckBox);
            Grid.SetColumn(textBlock1, 1);

            return aGrid;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            double studentScore = 0;

            //Подсчёт и сохранение результатов
            for (int i = 0; i < QListbox.Items.Count; i++)
            {
                ListBox answersList = null;
                int QPoints = 0;
                if (QListbox.Items[i] is Grid grid)
                {
                    if (grid.Children[1] is StackPanel stackPanel)
                    {
                        if (stackPanel.Children[0] is ListBox listBox)
                        {
                            answersList = listBox;
                        }
                    }
                    if (grid.Children[2] is Grid rightGrig)
                    {

                        if (rightGrig.Children[1] is TextBlock pointsTextBlock)
                        {
                            QPoints = Convert.ToInt32(pointsTextBlock.Text.Substring(0, pointsTextBlock.Text.IndexOf(" ")));
                        }
                    }
                }
                bool addPoints = true;
                for (int j = 0; j < answersList.Items.Count; j++)
                {
                    if (answersList.Items[j] is Grid aGrid)
                    {
                        bool userAnswer = false;
                        bool correctAnswer = false;
                        if (aGrid.Children[0] is CheckBox checkBox)
                        {
                            userAnswer = (bool)checkBox.IsChecked;
                        }
                        else if (aGrid.Children[0] is RadioButton radioButton)
                        {
                            userAnswer = (bool)radioButton.IsChecked;
                        }
                        if (aGrid.Children[2] is CheckBox correctAnswerCheckBox)
                        {
                            correctAnswer = (bool)correctAnswerCheckBox.IsChecked;
                        }
                        if (userAnswer!= correctAnswer)
                        {
                            addPoints = false;
                            break;
                        }
                    }
                }
                if (addPoints)
                {
                    studentScore += QPoints;
                }
            }

            double mark = 10 * (studentScore / allTestScore);
            mark = Math.Round(mark);
            MessageBox.Show($"Вы набрали {studentScore} из {allTestScore} баллов \nВаша оценка: {mark} ", "Ваш результат", MessageBoxButton.OK, MessageBoxImage.Information);

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommandINSERT = new SqlCommand("INSERT INTO [StudentsTestResults] (Tests_id, Students_id, StudentScore, AllTestScore, Mark)VALUES(@Tests_id, @Students_id, @StudentScore, @AllTestScore, @Mark)", sqlConnection);
            sqlCommandINSERT.Parameters.AddWithValue("Tests_id", Tests_id);
            sqlCommandINSERT.Parameters.AddWithValue("Students_id", Student_id);
            sqlCommandINSERT.Parameters.AddWithValue("StudentScore", studentScore);
            sqlCommandINSERT.Parameters.AddWithValue("AllTestScore", allTestScore);
            sqlCommandINSERT.Parameters.AddWithValue("Mark", mark);
            await sqlCommandINSERT.ExecuteNonQueryAsync();
            sqlConnection.Close();

            Owner.Show();
            this.Close();
        }

        private void AnswersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox answersList = (ListBox)sender;
            if (answersList.Items[answersList.SelectedIndex] is Grid aGrid)
            {
                if (aGrid.Children[0] is RadioButton radioButton)
                {
                    radioButton.IsChecked = !radioButton.IsChecked;
                }
                else if(aGrid.Children[0] is CheckBox checkBox)
                {
                    checkBox.IsChecked = !checkBox.IsChecked;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Show();
        }
    }
}
