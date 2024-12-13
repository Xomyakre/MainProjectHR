using MainProjectHR.DataBase;
using System.Collections.ObjectModel;
using System.Linq;
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

        public class Employee
        {
            public int ID { get; set; }
            public string FullName { get; set; }
            public string Position { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }
    }
}
