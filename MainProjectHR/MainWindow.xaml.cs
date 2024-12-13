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

            // Скрываем MainWindow и запускаем SplashScreen
            this.Hide();
            SplashScreen splashScreen = new SplashScreen();

            // Подписываемся на событие завершения проверки
            splashScreen.AuthenticationCompleted += (isAuthenticated) =>
            {
                // Используем Dispatcher для выполнения кода в UI-потоке
                Dispatcher.Invoke(() =>
                {
                    splashScreen.Close();

                    if (isAuthenticated)
                    {
                        // Открываем HrManager при успешной авторизации
                        var HrManager = new HrManager();
                        HrManager.Show();
                    }
                    else
                    {
                        // Показываем MainWindow снова при ошибке авторизации
                        this.Show();

                        // Показываем ошибку авторизации в TextBlock
                        ErrorTextBlock.Visibility = Visibility.Visible;
                        ErrorTextBlock.Text = "Неверный логин или пароль.";
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
            // Создаем и показываем окно регистрации
            CreateAccount createAccountWindow = new CreateAccount();
            createAccountWindow.ShowDialog();  // Показываем окно как модальное
        }
    }
}
