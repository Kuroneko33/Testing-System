using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestingSystem
{
    /// <summary>
    /// Interaction logic for EditTest.xaml
    /// </summary>
    public partial class EditTest : Window
    {
        SolidColorBrush brushWatermarkBackground = new SolidColorBrush(Colors.White);
        SolidColorBrush brushWatermarkForeground = new SolidColorBrush(Colors.LightSteelBlue);
        SolidColorBrush brushWatermarkBorder = new SolidColorBrush(Colors.Indigo);
        SolidColorBrush transparent = new SolidColorBrush(Colors.White);
        int radioIndex = 0;
        public ListBox TestsList = new ListBox();
        SqlConnection sqlConnection;

        public EditTest(ListBox TestsListbox)
        {
            InitializeComponent();
            TestsList = TestsListbox;
            string connectionString = Connection.connectionString;
            sqlConnection = new SqlConnection(connectionString);
        }
        
        private void TxtUserEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            for (int i = 0; i < QListbox.Items.Count; i++)
            {
                if (QListbox.Items[i] is Grid grid)
                {
                    if (grid.Children[0] is StackPanel stackPanel)
                    {
                        if (stackPanel.Children[0] is Grid qGrid)
                        {
                            if (textBox.Equals((TextBox)qGrid.Children[1]))
                            {
                                if (textBox.Text.Length != 0)
                                {
                                    TextBlock textBlock = (TextBlock)qGrid.Children[0];
                                    textBlock.Visibility = Visibility.Hidden;
                                }
                                else
                                {
                                    TextBlock textBlock = (TextBlock)qGrid.Children[0];
                                    textBlock.Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AnswerUserEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            for (int i = 0; i < QListbox.Items.Count; i++)
            {
                if (QListbox.Items[i] is Grid grid)
                {
                    if (grid.Children[1] is StackPanel stackPanel)
                    {
                        if (stackPanel.Children[0] is ListBox listBox)
                        {
                            for (int j = 0; j < listBox.Items.Count; j++)
                            {
                                if (listBox.Items[j] is Grid aGrid)
                                {
                                    if (textBox.Equals((TextBox)aGrid.Children[2]))
                                    {
                                        if (textBox.Text.Length != 0)
                                        {
                                            TextBlock textBlock = (TextBlock)aGrid.Children[1];
                                            textBlock.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            TextBlock textBlock = (TextBlock)aGrid.Children[1];
                                            textBlock.Visibility = Visibility.Visible;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void TestName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TestName.Text.Length != 0)
            {
                TestNameWatermark.Visibility = Visibility.Hidden;
            }
            else
            {
                TestNameWatermark.Visibility = Visibility.Visible;
            }
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
                TestNameWatermark.Visibility = Visibility.Hidden;
                dataReader.Close();

                sqlCommandSELECT = new SqlCommand($"SELECT * FROM [Questions] WHERE Tests_id=N'{Tests_id}'", sqlConnection);
                dataReader = null;
                List<int> Questions_ids = new List<int>();
                List<string> Questions_names = new List<string>();
                List<string> Questions_types = new List<string>();
                List<int> Questions_points = new List<int>();
                try
                {
                    dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                    while (await dataReader.ReadAsync())
                    {
                        Questions_ids.Add(Convert.ToInt32(dataReader["Id"]));
                        Questions_names.Add((String)(Convert.ToString(dataReader["Name"])).Trim(' '));
                        Questions_types.Add((String)(Convert.ToString(dataReader["Type"])).Trim(' '));
                        Questions_points.Add(Convert.ToInt32(dataReader["Points"]));
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
                            if (stackPanel.Children[1] is DockPanel dockPanel)
                            {
                                if (stackPanel.Children[0] is ListBox listBox)
                                {
                                    answersList = listBox;
                                }
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

        private void DelAnswer_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            for (int i = 0; i < QListbox.Items.Count; i++)
            {
                if (QListbox.Items[i] is Grid grid)
                {
                    if (grid.Children[1] is StackPanel stackPanel)
                    {
                        if (stackPanel.Children[0] is ListBox listBox)
                        {
                            for (int j = 0; j < listBox.Items.Count; j++)
                            {
                                if (listBox.Items[j] is Grid aGrid)
                                {
                                    if (button.Equals((Button)aGrid.Children[3]))
                                    {
                                        listBox.Items.RemoveAt(j);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Button pressedButton = new Button();
            for (int i = 0; i < QListbox.Items.Count; i++)
            {
                if (QListbox.Items[i] is Grid grid)
                {
                    if (grid.Children[1] is StackPanel stackPanel)
                    {
                        if (stackPanel.Children[1] is DockPanel dockPanel)
                        {
                            if (dockPanel.Children[0] is Button button1)
                            {
                                pressedButton = button1;
                            }
                            if (button.Equals(pressedButton))
                            {
                                if (stackPanel.Children[0] is ListBox listBox)
                                {
                                    if (grid.Children[2] is Grid rightGrid)
                                    {
                                        if (rightGrid.Children[0] is TextBlock rightTextBlock)
                                        {
                                            if (rightTextBlock.Text[0].Equals('О'))
                                            {
                                                if (listBox.Items.Count > 0)
                                                {
                                                    if (listBox.Items[0] is Grid answerGrid)
                                                    {
                                                        if (answerGrid.Children[0] is RadioButton rb)
                                                        {
                                                            listBox.Items.Add(AddRadioAnswer(rb.GroupName));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    listBox.Items.Add(AddRadioAnswer(radioIndex.ToString()));
                                                    radioIndex++;
                                                }
                                            }
                                            else if (rightTextBlock.Text[0].Equals('Н'))
                                            {
                                                listBox.Items.Add(AddCheckboxAnswer());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private Grid AddCheckboxAnswer(string Text = "", string Correctness = "false")
        {
            Grid aGrid = new Grid();
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            aGrid.ColumnDefinitions.Add(new ColumnDefinition());
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            CheckBox checkBox = new CheckBox();
            checkBox.Margin = new Thickness(0, 2, 2, 0);
            if (Correctness.Equals("true"))
            {
                checkBox.IsChecked = true;
            }

            TextBlock textBlock1 = new TextBlock();
            textBlock1.Margin = new Thickness(2, 1, 0, 0);
            textBlock1.Text = "Вариант ответа";
            textBlock1.Foreground = brushWatermarkForeground;
            textBlock1.MinHeight = 20;
            if (!Text.Equals(""))
            {
                textBlock1.Visibility = Visibility.Hidden;
            }

            TextBox textBox1 = new TextBox();
            textBox1.Background = transparent;
            textBox1.BorderBrush = brushWatermarkBorder;
            textBox1.Text = Text;
            textBox1.TextChanged += AnswerUserEntry_TextChanged;



            Button del = new Button();
            del.Height = 20;
            del.Margin = new Thickness(5, 0, 0, 0);
            del.Click += DelAnswer_Click;
            TextBlock deltext = new TextBlock();
            deltext.FontSize = 12;
            deltext.Text = " X ";
            del.Content = deltext;

            aGrid.Children.Add(checkBox);
            aGrid.Children.Add(textBlock1);
            Grid.SetColumn(textBlock1, 1);
            aGrid.Children.Add(textBox1);
            Grid.SetColumn(textBox1, 1);
            aGrid.Children.Add(del);
            Grid.SetColumn(del, 2);

            return aGrid;
        }

        private Grid AddRadioAnswer(string group, string Text = "", string Correctness = "false")
        {
            Grid aGrid = new Grid();
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            aGrid.ColumnDefinitions.Add(new ColumnDefinition());
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            RadioButton radioButton = new RadioButton();
            radioButton.Margin = new Thickness(0, 2, 2, 0);
            radioButton.GroupName = group;
            if (Correctness.Equals("true"))
            {
                radioButton.IsChecked = true;
            }

            TextBlock textBlock1 = new TextBlock();
            textBlock1.Margin = new Thickness(2, 1, 0, 0);
            textBlock1.Text = "Вариант ответа";
            textBlock1.Foreground = brushWatermarkForeground;
            textBlock1.MinHeight = 20;
            if (!Text.Equals(""))
            {
                textBlock1.Visibility = Visibility.Hidden;
            }

            TextBox textBox1 = new TextBox();
            textBox1.Background = transparent;
            textBox1.BorderBrush = brushWatermarkBorder;
            textBox1.TextChanged += AnswerUserEntry_TextChanged;
            textBox1.Text = Text;

            Button del = new Button();
            del.Height = 20;
            del.Margin = new Thickness(5, 0, 0, 0);
            del.Click += DelAnswer_Click;
            TextBlock deltext = new TextBlock();
            deltext.FontSize = 12;
            deltext.Text = " X ";
            del.Content = deltext;

            aGrid.Children.Add(radioButton);
            aGrid.Children.Add(textBlock1);
            Grid.SetColumn(textBlock1, 1);
            aGrid.Children.Add(textBox1);
            Grid.SetColumn(textBox1, 1);
            aGrid.Children.Add(del);
            Grid.SetColumn(del, 2);

            return aGrid;
        }

        private void DelqButton(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            for (int i = 0; i < QListbox.Items.Count; i++)
            {
                if (QListbox.Items[i] is Grid grid)
                {
                    if (grid.Children[2] is Grid rightGrid)
                    {
                        if (rightGrid.Children[1] is Button button1)
                        {
                            if (button.Equals((Button)button1))
                            {
                                QListbox.Items.RemoveAt(i);
                            }
                        }
                    }
                }
            }
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
            textBlock.Text = "Вопрос";
            textBlock.Foreground = brushWatermarkForeground;
            textBlock.Visibility = Visibility.Visible;
            textBlock.MinHeight = 20;
            if (!Text.Equals(""))
            {
                textBlock.Visibility = Visibility.Hidden;
            }

            TextBox textBox = new TextBox();
            textBox.MinHeight = 20;
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.Background = transparent;
            textBox.BorderBrush = brushWatermarkBorder;
            textBox.Text = Text;
            textBox.TextChanged += TxtUserEntry_TextChanged;

            stackPanelQGrid.Children.Add(textBlock);
            stackPanelQGrid.Children.Add(textBox);
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

            DockPanel dockButtons = new DockPanel();

            Button add = new Button();
            add.Height = 20;
            add.Margin = new Thickness(0, 10, 0, 0);
            add.Click += AddAnswer_Click;
            TextBlock addtext = new TextBlock();
            addtext.FontSize = 12;
            addtext.Text = "    Добавить вариант    ";
            add.Content = addtext;

            dockButtons.Children.Add(add);
            DockPanel.SetDock(add, Dock.Left);

            stackPanelA.Children.Add(answersList);
            stackPanelA.Children.Add(dockButtons);
            grid.Children.Add(stackPanelA);
            Grid.SetRow(stackPanelA, 1);

            Button delQ = new Button();
            delQ.Height = 30;
            delQ.Margin = new Thickness(5);
            delQ.VerticalAlignment = VerticalAlignment.Bottom;
            delQ.HorizontalAlignment = HorizontalAlignment.Right;
            delQ.Click += DelqButton;
            TextBlock delQtext = new TextBlock();
            delQtext.FontSize = 12;
            delQtext.Text = "   Удалить вопрос   ";
            delQ.Content = delQtext;
            rightGrid.Children.Add(delQ);

            grid.Children.Add(rightGrid);

            QListbox.Items.Add(grid);
            return QListbox.Items.Count - 1;
        }

        private void AddQ_Click(object sender, RoutedEventArgs e)
        {
            if (QType.SelectedIndex == 0)
            {
                AddQuestion("Radio");
            }
            else if (QType.SelectedIndex == 1)
            {
                AddQuestion("Checkbox");
            }
        }

        public async Task ClearOldTestData(string testName)
        {
            //Открытие подключения
            await sqlConnection.OpenAsync();

            //Чтение id добавленного ТЕСТА в базе данных для удаления ВОПРОСОВ
            SqlDataReader dataReader = null;
            SqlCommand sqlCommandSELECT = new SqlCommand($"SELECT [Id] From [Tests] WHERE Name=N'{testName}'", sqlConnection);
            try
            {
                dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                await dataReader.ReadAsync();
                int Tests_id = Convert.ToInt32(dataReader["Id"]);
                dataReader.Close();

                sqlCommandSELECT = new SqlCommand($"SELECT [Id] FROM [Questions] WHERE Tests_id=N'{Tests_id}'", sqlConnection);
                dataReader = null;
                List<int> Questions_ids = new List<int>();
                try
                {
                    dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                    while (await dataReader.ReadAsync())
                    {
                        Questions_ids.Add(Convert.ToInt32(dataReader["Id"]));
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
                foreach (int Questions_id in Questions_ids)
                {
                    SqlCommand sqlCommandDELETE_Answers = new SqlCommand($"DELETE FROM [Answers] WHERE [Questions_id]=@Questions_id", sqlConnection);
                    sqlCommandDELETE_Answers.Parameters.AddWithValue("Questions_id", Questions_id);
                    await sqlCommandDELETE_Answers.ExecuteNonQueryAsync();
                }

                SqlCommand sqlCommandDELETE = new SqlCommand($"DELETE FROM [Questions] WHERE [Tests_id]=@Tests_id", sqlConnection);
                sqlCommandDELETE.Parameters.AddWithValue("Tests_id", Tests_id);
                await sqlCommandDELETE.ExecuteNonQueryAsync();

                sqlCommandDELETE = new SqlCommand($"DELETE FROM [Tests] WHERE [Name]=@Name", sqlConnection);
                sqlCommandDELETE.Parameters.AddWithValue("Name", testName);
                await sqlCommandDELETE.ExecuteNonQueryAsync();

                for (int i = 0; i < TestsList.Items.Count; i++)
                {
                    if (TestsList.Items[i] is Grid grid)
                    {
                        if (grid.Children[0] is TextBlock testNameTextBox)
                        {
                            if (testName.Equals(testNameTextBox.Text))
                            {
                                TestsList.Items.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            sqlConnection.Close();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string CurTestName = TestName.Text;
            await ClearOldTestData(CurTestName);

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
            nameTextBlock.Text = CurTestName;
            nameTextBlock.Margin = new Thickness(10);
            Grid.SetColumn(nameTextBlock, 0);
            Grid.SetColumnSpan(nameTextBlock, 4);
            grid.Children.Add(nameTextBlock);
            TextBlock statusTextBlock = new TextBlock();
            statusTextBlock.FontSize = 20;
            statusTextBlock.Text = "Закрыт";
            statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            statusTextBlock.Opacity = 0.8;
            statusTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            statusTextBlock.VerticalAlignment = VerticalAlignment.Center;
            statusTextBlock.Margin = new Thickness(10);
            Grid.SetColumn(statusTextBlock, 4);
            grid.Children.Add(statusTextBlock);
            TestsList.Items.Add(grid);

            /////Сохранение данных теста в БД
            //Открытие подключения
            await sqlConnection.OpenAsync();

            //Добавление теста
            SqlCommand sqlCommandINSERT = new SqlCommand("INSERT INTO [Tests] (Name, Status)VALUES(@Name, @Status)", sqlConnection);
            sqlCommandINSERT.Parameters.AddWithValue("Name", CurTestName);
            sqlCommandINSERT.Parameters.AddWithValue("Status", "Закрыт");
            await sqlCommandINSERT.ExecuteNonQueryAsync();
            //Чтение id добавленного ТЕСТА в базе данных для добавления ВОПРОСОВ
            SqlCommand sqlCommandSELECT = new SqlCommand($"SELECT [Id] From [Tests] WHERE Name=N'{CurTestName}'", sqlConnection);
            SqlDataReader dataReader = null;
            dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
            await dataReader.ReadAsync();
            int Tests_id = Convert.ToInt32(dataReader["Id"]);
            dataReader.Close();
            //Добавление вопросов теста
            for (int i = 0; i < QListbox.Items.Count; i++)
            {
                if (QListbox.Items[i] is Grid mainGrid)
                {
                    if (mainGrid.Children[0] is StackPanel stackPanel)
                    {
                        if (stackPanel.Children[0] is Grid qGrid)
                        {
                            TextBox questionTextBox = qGrid.Children[1] as TextBox;
                            string CurQuestionName = questionTextBox.Text;
                            string Type = null;
                            int Points = 1;
                            if (mainGrid.Children[2] is Grid rightGrid)
                            {
                                if (rightGrid.Children[0] is TextBlock rightTextBlock)
                                {
                                    if (rightTextBlock.Text[0].Equals('О'))
                                    {
                                        Type = "Radio";
                                    }
                                    else if (rightTextBlock.Text[0].Equals('Н'))
                                    {
                                        Type = "Checkbox";
                                    }
                                }
                            }
                            sqlCommandINSERT = new SqlCommand("INSERT INTO [Questions] (Name, Type, Points, Tests_id)VALUES(@Name, @Type, @Points, @Tests_id)", sqlConnection);
                            sqlCommandINSERT.Parameters.AddWithValue("Name", CurQuestionName);
                            sqlCommandINSERT.Parameters.AddWithValue("Type", Type);
                            sqlCommandINSERT.Parameters.AddWithValue("Points", Points);
                            sqlCommandINSERT.Parameters.AddWithValue("Tests_id", Tests_id);
                            sqlCommandINSERT.ExecuteNonQuery();
                            //Чтение id добавленного ВОПРОСА в базе данных для добавления ВАРИАНТОВ ОТВЕТОВ
                            sqlCommandSELECT = new SqlCommand($"SELECT [Id] From [Questions] WHERE Name=N'{CurQuestionName}'", sqlConnection);
                            dataReader = null;
                            dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                            await dataReader.ReadAsync();
                            int Questions_id = Convert.ToInt32(dataReader["Id"]);
                            dataReader.Close();
                            //Добавление вариантов ответов для вопросов теста
                            //
                            if (mainGrid.Children[1] is StackPanel stackPanelA)
                            {
                                if (stackPanelA.Children[0] is ListBox listBoxA)
                                {
                                    for (int j = 0; j < listBoxA.Items.Count; j++)
                                    {
                                        if (listBoxA.Items[j] is Grid GridA)
                                        {
                                            TextBox textBoxA = (TextBox)GridA.Children[2];
                                            string AnswerText = textBoxA.Text;
                                            string Correctness = null;
                                            if (GridA.Children[0] is RadioButton radioButtonA)
                                            {
                                                if (radioButtonA.IsChecked.Value == true)
                                                {
                                                    Correctness = "true";
                                                }
                                                else
                                                {
                                                    Correctness = "false";
                                                }
                                            }
                                            else if(GridA.Children[0] is CheckBox checkBoxA)
                                            {
                                                if (checkBoxA.IsChecked.Value == true)
                                                {
                                                    Correctness = "true";
                                                }
                                                else
                                                {
                                                    Correctness = "false";
                                                }
                                            }

                                            sqlCommandINSERT = new SqlCommand("INSERT INTO [Answers] (Text, Correctness, Questions_id)VALUES(@Text, @Correctness, @Questions_id)", sqlConnection);
                                            sqlCommandINSERT.Parameters.AddWithValue("Text", AnswerText);
                                            sqlCommandINSERT.Parameters.AddWithValue("Correctness", Correctness);
                                            sqlCommandINSERT.Parameters.AddWithValue("Questions_id", Questions_id);
                                            sqlCommandINSERT.ExecuteNonQuery();
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }


            sqlConnection.Close();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                Console.WriteLine("Up");

            else if (e.Delta < 0)
                Console.WriteLine("Down");
        }
    }
}
