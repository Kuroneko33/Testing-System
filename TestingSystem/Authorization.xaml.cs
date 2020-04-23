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
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        SqlConnection sqlConnection;
        public Authorization()
        {
            InitializeComponent();
            string connectionString = Connection.connectionString;
            sqlConnection = new SqlConnection(connectionString);
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

        private void Surname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Surname.Text.Length > 0)
            {
                SurnameWatermark.Opacity = 0;
            }
            else
            {
                SurnameWatermark.Opacity = 1;
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

        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Number.Text.Length > 0)
            {
                NumberWatermark.Opacity = 0;
            }
            else
            {
                NumberWatermark.Opacity = 1;
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
                    Surname.Visibility = Visibility.Collapsed;
                    SurnameWatermark.Visibility = Visibility.Collapsed;
                    Group.Visibility = Visibility.Collapsed;
                    GroupWatermark.Visibility = Visibility.Collapsed;
                    Number.Visibility = Visibility.Collapsed;
                    NumberWatermark.Visibility = Visibility.Collapsed;
                    Password.Visibility = Visibility.Visible;
                    PasswordWatermark.Visibility = Visibility.Visible;
                }
                else
                {
                    Password.Visibility = Visibility.Collapsed;
                    PasswordWatermark.Visibility = Visibility.Collapsed;
                    Surname.Visibility = Visibility.Visible;
                    SurnameWatermark.Visibility = Visibility.Visible;
                    Group.Visibility = Visibility.Visible;
                    GroupWatermark.Visibility = Visibility.Visible;
                    Number.Visibility = Visibility.Visible;
                    NumberWatermark.Visibility = Visibility.Visible;
                }
            }    
        }

        private async void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (RBTeacher.IsChecked.Value == true)
            {
                bool authorization = false;

                await sqlConnection.OpenAsync();
                SqlDataReader dataReader = null;
                SqlCommand sqlCommandSELECT = new SqlCommand($"SELECT * From [Teachers]", sqlConnection);

                try
                {
                    dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                    while (await dataReader.ReadAsync())
                    {
                        if (NameLogin.Text.Equals((String)(Convert.ToString(dataReader["Login"])).Trim(' ')) && Password.Password.Equals((String)(Convert.ToString(dataReader["Password"])).Trim(' ')))
                        {
                            authorization = true;
                            break;
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

                if (authorization)
                {
                    new TeacherTestsEditor().Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Введены неверные данные!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                sqlConnection.Close();

            }
            else if (RBStudent.IsChecked.Value == true)
            {
                bool check = true;
                int student_id = 0;
                string name = NameLogin.Text;
                string surname = Surname.Text;
                string group = Group.Text;
                string number = Number.Text;
                if (!(number.Length == 8 || number.Length == 0))
                {
                    check = false;
                    MessageBox.Show("Введены неверные данные!\nДлина номера зачётной книжки должна быть равна 9\n(или 0 для теста)", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (check)
                {
                    await sqlConnection.OpenAsync();
                    //Добавление студента в базу
                    SqlCommand sqlCommandINSERT = new SqlCommand("INSERT INTO [Students] (Name, Surname, StGroup, Number) VALUES (@Name, @Surname, @StGroup, @Number)", sqlConnection);
                    sqlCommandINSERT.Parameters.AddWithValue("Name", name);
                    sqlCommandINSERT.Parameters.AddWithValue("Surname", surname);
                    sqlCommandINSERT.Parameters.AddWithValue("StGroup", group);
                    sqlCommandINSERT.Parameters.AddWithValue("Number", number);
                    await sqlCommandINSERT.ExecuteNonQueryAsync();

                    //Чтение Id только что добавленного студента
                    SqlDataReader dataReader = null;
                    SqlCommand sqlCommandSELECT = new SqlCommand($"SELECT TOP 1 [Id] From [Students] ORDER BY Id DESC", sqlConnection);

                    try
                    {
                        dataReader = await sqlCommandSELECT.ExecuteReaderAsync();
                        await dataReader.ReadAsync();
                        student_id = Convert.ToInt32(dataReader["Id"]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        dataReader.Close();
                    }

                    sqlConnection.Close();

                    new StudentTestSelect(student_id).Show();
                    this.Close();
                }
            }
        }
    }
}
