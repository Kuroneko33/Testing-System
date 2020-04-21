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
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void NameLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameLogin.Text.Length > 0)
            {
                NameLoginWatermark.Opacity = 0;
            }
            else
            {
                NameLoginWatermark.Opacity = 1;
            }
        }

        private void FName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FName.Text.Length > 0)
            {
                FNameWatermark.Opacity = 0;
            }
            else
            {
                FNameWatermark.Opacity = 1;
            }
        }

        private void Group_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Group.Text.Length > 0)
            {
                GroupWatermark.Opacity = 0;
            }
            else
            {
                GroupWatermark.Opacity = 1;
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Password.Password.Length > 0)
            {
                PasswordWatermark.Opacity = 0;
            }
            else
            {
                PasswordWatermark.Opacity = 1;
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if(radioButton.IsChecked.Value == true)
            {
                if (radioButton.Content.Equals("Преподаватель"))
                {
                    FName.Visibility = Visibility.Hidden;
                    FNameWatermark.Visibility = Visibility.Hidden;
                    Group.Visibility = Visibility.Hidden;
                    GroupWatermark.Visibility = Visibility.Hidden;
                    Password.Visibility = Visibility.Visible;
                    PasswordWatermark.Visibility = Visibility.Visible;
                }
                else
                {
                    Password.Visibility = Visibility.Hidden;
                    PasswordWatermark.Visibility = Visibility.Hidden;
                    FName.Visibility = Visibility.Visible;
                    FNameWatermark.Visibility = Visibility.Visible;
                    Group.Visibility = Visibility.Visible;
                    GroupWatermark.Visibility = Visibility.Visible;
                }
            }    
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (RBTeacher.IsChecked.Value == true)
            {
                new TeacherTestsEditor().Show();
                this.Close();
            }
            else if (RBStudent.IsChecked.Value == true)
            {
                this.Close();
            }
            //Обработка авторизации и запуск соответствующих форм
            //EditTest editTest = new EditTest();
            //editTest.Show();
        }
    }
}
