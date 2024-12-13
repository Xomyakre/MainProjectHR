using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MainProjectHR.DataBase;

namespace MainProjectHR
{
    public partial class CreateAccount : Window
    {
        private readonly DataBaseConnect _dbContext;

        public CreateAccount()
        {
            InitializeComponent();
            _dbContext = new DataBaseConnect();
        }

        private async void CreateAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = txtNewUsername.Text;
            string password = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Проверка совпадения паролей
            if (password != confirmPassword)
            {
                MessageTextBlock.Text = "Пароли не совпадают";
                MessageTextBlock.Foreground = Brushes.Red;
                MessageTextBlock.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                MessageTextBlock.Visibility = Visibility.Collapsed;
            }

            registrationProgressBar.Visibility = Visibility.Visible;
            registrationProgressBar.Value = 0;

            // Заполнение прогрессбара
            for (int i = 0; i <= 80; i += 20)
            {
                registrationProgressBar.Value = i;
                await Task.Delay(100);
            }

            // Выполнение добавления пользователя в базу данных
            bool userAdded = _dbContext.AddUser(login, password);

            // Завершаем прогрессбар на 100%
            registrationProgressBar.Value = 100;

            // Выводим сообщение об успехе или ошибке
            if (userAdded)
            {
                MessageTextBlock.Text = "Пользователь успешно зарегистрирован.";
                MessageTextBlock.Foreground = Brushes.Green; // Успешное сообщение зелёным цветом
                MessageTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                MessageTextBlock.Text = "Пользователь с таким логином уже существует";
                MessageTextBlock.Foreground = Brushes.Red; // Сообщение об ошибке красным цветом
                MessageTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
