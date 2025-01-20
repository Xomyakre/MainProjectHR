using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ShowError("Все поля должны быть заполнены");
                return;
            }

            // Проверка длины логина
            if (login.Length <= 3)
            {
                ShowError("Логин должен содержать более 3 символов");
                return;
            }

            // Проверка пароля
            if (password.Length < 8)
            {
                ShowError("Пароль должен быть не менее 8 символов");
                return;
            }

            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
            {
                ShowError("Пароль должен содержать хотя бы одну заглавную букву, одну строчную букву и одну цифру");
                return;
            }

            // Проверка совпадения паролей
            if (password != confirmPassword)
            {
                ShowError("Пароли не совпадают");
                return;
            }

            // Проверка на пробелы
            if (login.Contains(" ") || password.Contains(" "))
            {
                ShowError("Логин и пароль не могут содержать пробелы");
                return;
            }

            registrationProgressBar.Visibility = Visibility.Visible;
            registrationProgressBar.Value = 0;

            // Заполнение прогрессбара
            for (int i = 0; i <= 80; i += 20)
            {
                registrationProgressBar.Value = i;
                await Task.Delay(100);
            }

            try
            {
                bool result = _dbContext.AddUser(login, password);
                
                if (result)
                {
                    registrationProgressBar.Value = 100;
                    ShowSuccess("Пользователь успешно зарегистрирован");
                    await Task.Delay(1000);
                    this.Hide();
                }
                else
                {
                    ShowError("Пользователь с таким логином уже существует");
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при регистрации: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            MessageTextBlock.Text = message;
            MessageTextBlock.Foreground = Brushes.Red;
            MessageTextBlock.Visibility = Visibility.Visible;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void ShowSuccess(string message)
        {
            MessageTextBlock.Text = message;
            MessageTextBlock.Foreground = Brushes.Green;
            MessageTextBlock.Visibility = Visibility.Visible;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
