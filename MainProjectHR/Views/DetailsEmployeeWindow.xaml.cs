using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MainProjectHR.DataBase;
using System.ComponentModel;


namespace MainProjectHR.Views
{
    public partial class DetailsEmployeeWindow : Window
    {
        private bool _isEditing = false;
        private EmployeeViewModel _currentEmployee;

        public DetailsEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            LoadEmployeeDetails(employee.ID);
            
            // Добавляем возможность перетаскивания окна
            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            };
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
            this.WindowState = this.WindowState == WindowState.Maximized 
                ? WindowState.Normal 
                : WindowState.Maximized;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _isEditing = !_isEditing;
            EditButton.Content = _isEditing ? "Применить" : "Редактировать";
            
            if (!_isEditing)
            {
                SaveEmployeeDetails();
            }
            
            // Обновляем состояние IsReadOnly для всех полей
            _currentEmployee.IsReadOnly = !_isEditing;
        }

        private void SaveEmployeeDetails()
        {
            using (var dbContext = new DataBaseConnect())
            {
                var employee = dbContext.Employees.Find(_currentEmployee.ID);
                if (employee != null)
                {
                    employee.FullName = _currentEmployee.FullName;
                    employee.Position = _currentEmployee.Position;
                    employee.Email = _currentEmployee.Email;
                    employee.Phone = _currentEmployee.Phone;
                    employee.Address = _currentEmployee.Address;
                    employee.EmergencyContact = _currentEmployee.EmergencyContact;
                    employee.Salary = (decimal)_currentEmployee.Salary;
                    employee.MaritalStatus = (int)_currentEmployee.MaritalStatus;
                    employee.Experience = (int)_currentEmployee.Experience;
                    
                    dbContext.SaveChanges();
                }
            }
        }

        private void LoadEmployeeDetails(int employeeId)
        {
            _currentEmployee = GetEmployeeDetails(employeeId);
            _currentEmployee.IsReadOnly = true; // По умолчанию поля только для чтения
            this.DataContext = _currentEmployee;
        }

        private EmployeeViewModel GetEmployeeDetails(int employeeId)
        {
            using (var dbContext = new DataBaseConnect())
            {
                var employee = dbContext.Employees
                    .Where(e => e.ID == employeeId)
                    .Select(e => new EmployeeViewModel
                    {
                        ID = e.ID,
                        FullName = e.FullName,
                        Position = e.Position,
                        Department = e.Department.departmentName ?? "Не указан",
                        Email = e.Email,
                        Phone = e.Phone,
                        Address = e.Address,
                        EmergencyContact = e.EmergencyContact,
                        Salary = e.Salary,
                        MaritalStatus = e.MaritalStatus,
                        Experience = e.Experience,
                        Status = e.Status.statusName ?? "Не указан",
                        ActiveProjects = dbContext.Projects
                            .Where(p => p.EmployeeId == e.ID)
                            .Select(p => new ProjectViewModel 
                            { 
                                Name = p.Name, 
                                Role = p.Role 
                            })
                            .ToList(),
                        Achievements = dbContext.Achievements
                            .Where(a => a.EmployeeId == e.ID)
                            .Select(a => new AchievementViewModel
                            {
                                Title = a.Title,
                            })
                            .ToList()
                    })
                    .FirstOrDefault();

                return employee;
            }
        }

        public class EmployeeViewModel : INotifyPropertyChanged
        {
            public int ID { get; set; }
            public string FullName { get; set; }
            public string Position { get; set; }
            public string Department { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string BirthDate { get; set; }
            public string EmergencyContact { get; set; }
            public decimal? Salary { get; set; }
            public int? MaritalStatus { get; set; }
            public int? Experience { get; set; }
            public string Status { get; set; }
            public List<ProjectViewModel> ActiveProjects { get; set; }
            public List<AchievementViewModel> Achievements { get; set; }

            private bool _isReadOnly;
            public bool IsReadOnly
            {
                get => _isReadOnly;
                set
                {
                    _isReadOnly = value;
                    OnPropertyChanged(nameof(IsReadOnly));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class ProjectViewModel
        {
            public string Name { get; set; }
            public string Role { get; set; }
        }

        public class AchievementViewModel
        {
            public string Title { get; set; }
            public string Date { get; set; }
        }
    }
} 