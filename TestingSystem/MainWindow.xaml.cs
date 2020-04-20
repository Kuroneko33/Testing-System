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

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            /*answersList.Items.Add(new TextBlock().Text = "test");
            Grid grid = (Grid)answersList.Items[0];
            CheckBox checkBox = (CheckBox)grid.Children[0];
            checkBox.IsChecked = false;
            TextBox textBox = (TextBox)grid.Children[2];
            textBox.Text = "test";

            Grid grid1 = (Grid)answersList.Items[1];
            TextBox textBox1 = (TextBox)grid1.Children[2];
            textBox1.Text = "test";*/
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
            SolidColorBrush brushWatermarkBackground = new SolidColorBrush(Colors.White);
            SolidColorBrush brushWatermarkForeground = new SolidColorBrush(Colors.LightSteelBlue);
            SolidColorBrush brushWatermarkBorder = new SolidColorBrush(Colors.Indigo);
            SolidColorBrush transparent = new SolidColorBrush(Colors.White);
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
            answersList.HorizontalAlignment = HorizontalAlignment.Stretch;
            answersList.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);

            Grid aGrid = new Grid();
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            aGrid.ColumnDefinitions.Add(new ColumnDefinition());
            aGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            ////////////////
            if (true)
            {
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

                aGrid.Children.Add(checkBox);
                aGrid.Children.Add(textBlock1);
                Grid.SetColumn(textBlock1, 1);
                aGrid.Children.Add(textBox1);
                Grid.SetColumn(textBox1, 1);
            }

            answersList.Items.Add(aGrid);
            
            stackPanelA.Children.Add(answersList);
            grid.Children.Add(stackPanelA);
            Grid.SetRow(stackPanelA, 1);
            QListbox.Items.Add(grid);
        }
    }
}
