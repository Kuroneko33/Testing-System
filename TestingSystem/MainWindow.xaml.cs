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

            if (textBox.Text.Length != 0)
            {
                WatermarkAnswer.Visibility = Visibility.Hidden;
            }
            else
            {
                WatermarkAnswer.Visibility = Visibility.Visible;
            }
        }
    }
}
