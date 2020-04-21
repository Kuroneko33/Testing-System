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
using System.Windows.Shapes;

namespace TestingSystem
{
    /// <summary>
    /// Interaction logic for TeacherTestsEditor.xaml
    /// </summary>
    public partial class TeacherTestsEditor : Window
    {
        public ListBox TestsList = new ListBox();
        public TeacherTestsEditor()
        {
            InitializeComponent();
            TestsList = TestsListBox;
        }

        private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            new EditTest(TestsList) { Owner = this }.Show();
        }

        private void OpenTest_Click(object sender, RoutedEventArgs e)
        {
            if (TestsListBox.SelectedIndex >= 0)
            {
                if (TestsListBox.Items[TestsListBox.SelectedIndex] is Grid grid)
                {
                    if (grid.Children[1] is TextBlock textBlock)
                    {
                        textBlock.Text = "Открыт";
                        textBlock.Foreground = new SolidColorBrush(Colors.Green);
                    }
                }
            }
        }

        private void CloseTest_Click(object sender, RoutedEventArgs e)
        {
            if (TestsListBox.SelectedIndex >= 0)
            {
                if (TestsListBox.Items[TestsListBox.SelectedIndex] is Grid grid)
                {
                    if (grid.Children[1] is TextBlock textBlock)
                    {
                        textBlock.Text = "Закрыт";
                        textBlock.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
            }
        }

        private void EditTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteTest_Click(object sender, RoutedEventArgs e)
        {
            if (TestsListBox.SelectedIndex >= 0)
            {
                TestsList.Items.RemoveAt(TestsListBox.SelectedIndex);
            }
        }
    }
}
