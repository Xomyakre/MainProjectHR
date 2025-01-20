using MainProjectHR.DataBase;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MainProjectHR.Views
{
    public partial class EmployeeView : UserControl
    {
        private CollectionViewSource _employeesViewSource;
        private DataBaseConnect _dbContext;

        public EmployeeView()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (_dbContext = new DataBaseConnect())
            {
                // Получаем данные из базы данных
                var employees = _dbContext.Employees
                    .Select(e => new Employee
                    {
                        ID = e.ID,
                        FullName = e.FullName,
                        Position = e.Position,
                        Phone = e.Phone,
                        Email = e.Email
                    })
                    .ToList();

                // Создаём источник данных
                _employeesViewSource = new CollectionViewSource { Source = new ObservableCollection<Employee>(employees) };
                _employeesViewSource.Filter += EmployeesFilter;

                // Устанавливаем источник данных для DataGrid
                membersDataGrid.ItemsSource = _employeesViewSource.View;
            }
        }

        private void EmployeesFilter(object sender, FilterEventArgs e)
        {
            if (e.Item is Employee employee)
            {
                var filterText = textBoxSearch.Text.ToLower();
                e.Accepted = string.IsNullOrWhiteSpace(filterText) ||
                             employee.FullName.ToLower().Contains(filterText) ||
                             employee.Position.ToLower().Contains(filterText) ||
                             employee.Email.ToLower().Contains(filterText) ||
                             employee.Phone.ToLower().Contains(filterText);
            }
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _employeesViewSource?.View.Refresh();
        }

        

        private void DeleteEmployee(Employee employee)
        {
            if (employee == null) return;

            using (_dbContext = new DataBaseConnect())
            {
                var dbEmployee = _dbContext.Employees.FirstOrDefault(e => e.ID == employee.ID);
                if (dbEmployee != null)
                {
                    _dbContext.Employees.Remove(dbEmployee);
                    _dbContext.SaveChanges();
                }
            }

            // Удаляем пользователя из коллекции для обновления DataGrid
            var collection = (ObservableCollection<Employee>)_employeesViewSource.Source;
            collection.Remove(employee);
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var confirmationDialog = new DeleteConfirmationWindow();
            confirmationDialog.ShowDialog();

            if (confirmationDialog.Result)
            {
                if (sender is Button button && button.DataContext is Employee employee)
                {
                    DeleteEmployee(employee);
                }
            }
        }

        private void EditEmployee(Employee employee)
        {
            if (employee == null) return;

            var editWindow = new EditEmployeeWindow(employee);
            if (editWindow.ShowDialog() == true)
            {
                using (_dbContext = new DataBaseConnect())
                {
                    var dbEmployee = _dbContext.Employees.FirstOrDefault(e => e.ID == employee.ID);
                    if (dbEmployee != null)
                    {
                        dbEmployee.FullName = employee.FullName;
                        dbEmployee.Position = employee.Position;
                        dbEmployee.Email = employee.Email;
                        dbEmployee.Phone = employee.Phone;
                        dbEmployee.Salary = 50000;
                        _dbContext.SaveChanges();
                    }
                }

                // Обновляем источник данных
                _employeesViewSource.View.Refresh();
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Employee employee)
            {
                EditEmployee(employee);
            }
        }

        private void DetailsEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Employee employee)
            {
                var detailsWindow = new DetailsEmployeeWindow(employee);
                detailsWindow.ShowDialog();
            }
        }

    }

}
