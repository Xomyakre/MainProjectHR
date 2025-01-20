using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MainProjectHR.DataBase;

namespace MainProjectHR
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Скрываем текущее окно
            this.Hide();
            
            var splashScreen = new SplashScreen();
            splashScreen.AuthenticationCompleted += (isAuthenticated) =>
            {
                Dispatcher.Invoke(() =>
                {
                    splashScreen.Close();

                    if (isAuthenticated)
                    {
                        var hrManager = new HrManager();
                        hrManager.Show();
                        this.Close(); // Закрываем окно авторизации при успешном входе
                    }
                    else
                    {
                        this.Show();
                        ErrorTextBlock.Visibility = Visibility.Visible;
                        ErrorTextBlock.Text = "Неверный логин или пароль.";
                        txtPassword.Password = string.Empty; // Очищаем поле пароля
                    }
                });
            };

            splashScreen.Show();
            splashScreen.StartAuthentication(username, password);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void signupBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccountWindow = new CreateAccount();
            createAccountWindow.ShowDialog();
        }
    }
}
