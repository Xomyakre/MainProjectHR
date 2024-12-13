using MainProjectHR.DataBase;
using System.Windows;

namespace MainProjectHR
{
    public partial class AddEmployeeWindow : Window
    {
        public Employee NewEmployee { get; private set; }

        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем нового сотрудника
            NewEmployee = new Employee
            {
                FullName = FullNameTextBox.Text,
                Position = PositionTextBox.Text,
                Phone = PhoneTextBox.Text,
                Email = EmailTextBox.Text
            };

            if (string.IsNullOrEmpty(NewEmployee.FullName) || string.IsNullOrEmpty(NewEmployee.Position) ||
                string.IsNullOrEmpty(NewEmployee.Phone) || string.IsNullOrEmpty(NewEmployee.Email))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            DialogResult = true; // Закрываем окно и возвращаем результат
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
