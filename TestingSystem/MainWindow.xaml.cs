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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolidColorBrush brushWatermarkBackground = new SolidColorBrush(Colors.White);
        SolidColorBrush brushWatermarkForeground = new SolidColorBrush(Colors.LightSteelBlue);
        SolidColorBrush brushWatermarkBorder = new SolidColorBrush(Colors.Indigo);
        SolidColorBrush transparent = new SolidColorBrush(Colors.White);
        public ObservableCollection<int> ints { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void txtUserEntry_TextChanged(object sender, TextChangedEventArgs e)
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


        private void ComboBox_Selected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void answerUserEntry_TextChanged(object sender, TextChangedEventArgs e)
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
                                if (button.Equals(button1))
                                {
                                    if (stackPanel.Children[0] is ListBox listBox)
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

        private Grid AddCheckboxAnswer()
        {
            Grid aGrid = new Grid();
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            aGrid.ColumnDefinitions.Add(new ColumnDefinition());
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            CheckBox checkBox = new CheckBox();
            checkBox.Margin = new Thickness(0, 2, 2, 0);

            TextBlock textBlock1 = new TextBlock();
            textBlock1.Margin = new Thickness(2, 1, 0, 0);
            textBlock1.Text = "Вариант ответа";
            textBlock1.Foreground = brushWatermarkForeground;
            textBlock1.MinHeight = 20;

            TextBox textBox1 = new TextBox();
            textBox1.Background = transparent;
            textBox1.BorderBrush = brushWatermarkBorder;
            textBox1.TextChanged += answerUserEntry_TextChanged;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

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
            textBox.TextChanged += txtUserEntry_TextChanged;

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
            if (true)
            {
                Grid aGrid = AddCheckboxAnswer();
                answersList.Items.Add(aGrid);
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

            Grid rightGrid = new Grid();
            Grid.SetRow(rightGrid, 1);
            Grid.SetColumn(rightGrid, 1);

            if (true)
            {
                rightGrid.Children.Add(new TextBlock() { Text = "Отметьте правильные ответы \nслева от варианта", Margin = new Thickness(10, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, FontSize = 20 });
            }

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
    }
}
