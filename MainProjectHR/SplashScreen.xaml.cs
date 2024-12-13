using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using MainProjectHR.DataBase;

namespace MainProjectHR
{
    public partial class SplashScreen : Window
    {
        public event Action<bool> AuthenticationCompleted;

        private string _username;
        private string _password;

        public SplashScreen()
        {
            InitializeComponent();
        }

        public void StartAuthentication(string username, string password)
        {
            _username = username;
            _password = password;

            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isAuthenticated = AuthenticateUser(_username, _password);
            Thread.Sleep(2000); // Имитируем время загрузки

            // Вызываем событие завершения аутентификации с результатом
            AuthenticationCompleted?.Invoke(isAuthenticated);
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (var context = new DataBase.DataBaseConnect())
            {
                var user = context.AuthCredentials
                    .FirstOrDefault(u => u.login == username && u.password_hash == password);
                return user != null;
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
