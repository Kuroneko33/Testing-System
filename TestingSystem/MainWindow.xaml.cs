using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtUserEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length != 0)
            {
                WatermarkTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                WatermarkTextBlock.Visibility = Visibility.Visible;
            }
        }


        private void ComboBox_Selected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void answerUserEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            TextBlock textBlock = new TextBlock();

            for (int i = 0; i < answersList.Items.Count; i++)
            {
                if (answersList.Items[i] is Grid grid)
                {
                    if (textBox.Equals((TextBox)grid.Children[2]))
                    {
                        textBlock = (TextBlock)grid.Children[1];
                    }
                }
            }

            if (textBox.Text.Length != 0)
            {
                textBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                textBlock.Visibility = Visibility.Visible;
            }
        }

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            answersList.Items.Add(new TextBlock().Text = "test");
            Grid grid = (Grid)answersList.Items[0];
            CheckBox checkBox = (CheckBox)grid.Children[0];
            checkBox.IsChecked = false;
            TextBox textBox = (TextBox)grid.Children[2];
            textBox.Text = "test";

            Grid grid1 = (Grid)answersList.Items[1];
            TextBox textBox1 = (TextBox)grid1.Children[2];
            textBox1.Text = "test";
        }
    }
}
