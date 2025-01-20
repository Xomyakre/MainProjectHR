using MainProjectHR.DataBase;
using System.Windows;

namespace MainProjectHR
{
    public partial class EditEmployeeWindow : Window
    {
        public Employee CurrentEmployee { get; private set; }

        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            CurrentEmployee = employee;
            DataContext = CurrentEmployee;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true; // Устанавливаем успешный результат
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }
        // Добавляем возможность перетаскивания окна
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
