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
            new EditTest(TestsList) {Owner = this}.Show();
        }
    }
}
