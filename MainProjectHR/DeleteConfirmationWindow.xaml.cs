using System.Windows;

namespace MainProjectHR
{
    public partial class DeleteConfirmationWindow : Window
    {
        public bool Result { get; private set; }

        public DeleteConfirmationWindow()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}