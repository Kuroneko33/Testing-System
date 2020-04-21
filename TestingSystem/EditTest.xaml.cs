using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<int> ints { get; set; }
        public ListBox TestsList = new ListBox();
        public EditTest(ListBox TestsListbox)
        {
            InitializeComponent();
            TestsList = TestsListbox;
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

        private void ComboBox_Selected(object sender, SelectionChangedEventArgs e)
        {

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
            Button pressedButton1 = new Button();
            Button pressedButton2 = new Button();
            int mode = -1;
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
                                pressedButton1 = button1;
                            }
                            if (dockPanel.Children[2] is Button button2)
                            {
                                pressedButton2 = button2;
                            }
                            if (button.Equals(pressedButton1))
                            {
                                mode = 1;
                            }
                            else if (button.Equals(pressedButton2))
                            {
                                mode = 0;
                            }
                            if (mode != -1)
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
                                                            listBox.Items.Add(AddRadioAnswer(rb.GroupName, mode));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    listBox.Items.Add(AddRadioAnswer(radioIndex.ToString(), mode));
                                                    radioIndex++;
                                                }
                                            }
                                            else if (rightTextBlock.Text[0].Equals('Н'))
                                            {
                                                listBox.Items.Add(AddCheckboxAnswer(mode));
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

        private Grid AddCheckboxAnswer(int mode)
        {
            Grid aGrid = new Grid();
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            aGrid.ColumnDefinitions.Add(new ColumnDefinition());
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            CheckBox checkBox = new CheckBox();
            checkBox.Margin = new Thickness(0, 2, 2, 0);

            TextBlock textBlock1 = new TextBlock();
            textBlock1.Margin = new Thickness(2, 1, 0, 0);
            if (mode == 1)
            {
                textBlock1.Text = "Вариант ответа";
            }
            else
            {
                textBlock1.Text = "Другое";
            }
            textBlock1.Foreground = brushWatermarkForeground;
            textBlock1.MinHeight = 20;

            TextBox textBox1 = new TextBox();
            textBox1.Background = transparent;
            textBox1.BorderBrush = brushWatermarkBorder;
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

        private Grid AddRadioAnswer(string group, int mode)
        {
            Grid aGrid = new Grid();
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            aGrid.ColumnDefinitions.Add(new ColumnDefinition());
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            RadioButton radioButton = new RadioButton();
            radioButton.Margin = new Thickness(0, 2, 2, 0);
                
            
            radioButton.GroupName = group;

            TextBlock textBlock1 = new TextBlock();
            textBlock1.Margin = new Thickness(2, 1, 0, 0);
            if (mode == 1)
            {
                textBlock1.Text = "Вариант ответа";
            }
            else
            {
                textBlock1.Text = "Другое";
            }
            textBlock1.Foreground = brushWatermarkForeground;
            textBlock1.MinHeight = 20;

            TextBox textBox1 = new TextBox();
            textBox1.Background = transparent;
            textBox1.BorderBrush = brushWatermarkBorder;
            textBox1.TextChanged += AnswerUserEntry_TextChanged;

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

        private void AddQ_Click(object sender, RoutedEventArgs e)
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Grid rightGrid = new Grid();
            Grid.SetRowSpan(rightGrid, 2);
            Grid.SetColumn(rightGrid, 1);

            if (QType.SelectedIndex == 0)
            {
                rightGrid.Children.Add(new TextBlock() { Text = "Один из списка\n\nОтметьте правильный ответ \nслева от варианта", Margin = new Thickness(10, 0, 10, 0), HorizontalAlignment = HorizontalAlignment.Left, FontSize = 15 });
            }
            else if (QType.SelectedIndex == 1)
            {
                rightGrid.Children.Add(new TextBlock() { Text = "Несколько из списка\n\nОтметьте правильные ответы \nслева от варианта", Margin = new Thickness(10, 0, 10, 0), HorizontalAlignment = HorizontalAlignment.Left, FontSize = 15 });
            }

            StackPanel stackPanelQ = new StackPanel();
            stackPanelQ.Margin = new Thickness(5);

            Grid stackPanelQGrid = new Grid();
            
            transparent.Opacity = 0;
            stackPanelQGrid.Background = brushWatermarkBackground;

            Style styleEntryFieldStyle = new Style();
            stackPanelQGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            stackPanelQGrid.VerticalAlignment = VerticalAlignment.Center;

            TextBlock textBlock = new TextBlock();
            textBlock.Margin = new Thickness(2, 1, 0, 0);
            textBlock.Text = "Вопрос";
            textBlock.Foreground = brushWatermarkForeground;
            textBlock.Visibility = Visibility.Visible;
            textBlock.MinHeight = 20;

            TextBox textBox = new TextBox();
            textBox.MinHeight = 20;
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.Background = transparent;
            textBox.BorderBrush = brushWatermarkBorder;
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



            ////////////////
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
                                answersList.Items.Add(AddRadioAnswer(rb.GroupName,1));
                            }
                        }
                    }
                    else
                    {
                        answersList.Items.Add(AddRadioAnswer(radioIndex.ToString(), 1));
                        radioIndex++;
                    }
                }
                else if (rightTextBlock.Text[0].Equals('Н'))
                {
                    answersList.Items.Add(AddCheckboxAnswer(1));
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

            dockButtons.Children.Add(new TextBlock() { Text = "   Или   ", Margin = new Thickness(0, 10, 0, 0)});

            Button addOther = new Button();
            addOther.Height = 20;
            addOther.Margin = new Thickness(0, 10, 0, 0);
            addOther.Click += AddAnswer_Click;
            TextBlock addOthertext = new TextBlock();
            addOthertext.FontSize = 12;
            addOthertext.Text = "Добавить вариант другое";
            addOther.Content = addOthertext;

            dockButtons.Children.Add(addOther);
            DockPanel.SetDock(addOther, Dock.Right);

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
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TestsList.Items.Add(TestName.Text);
            this.Close();
        }
    }
}
