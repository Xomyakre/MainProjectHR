using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using MainProjectHR.DataBase;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

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
            StartLogoRotationWithDelay();
        }

        private async void StartLogoRotationWithDelay()
        {
            
            await Task.Delay(100);
            
            // Создаем и запускаем анимацию вращения
            var rotation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(2),
                RepeatBehavior = RepeatBehavior.Forever
            };

            LogoRotation.BeginAnimation(RotateTransform.AngleProperty, rotation);
        }

        public void StartAuthentication(string username, string password)
        {
            _username = username;
            _password = password;

            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += (s, e) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AuthenticationCompleted?.Invoke((bool)e.Result);
                });
            };
            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isAuthenticated = AuthenticateUser(_username, _password);
            Thread.Sleep(1000);
            e.Result = isAuthenticated;
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
