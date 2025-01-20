using MainProjectHR.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Windows.Controls;

namespace MainProjectHR.Views
{
    /// <summary>
    /// Логика взаимодействия для DetailsEmployee.xaml
    /// </summary>
    public partial class DetailsEmployee : UserControl
    {
        private DataBaseConnect _dbContext;
        public Employee SelectedEmployee { get; }

        public DetailsEmployee(Employee employee)
        {
            InitializeComponent();

            SelectedEmployee = employee ?? throw new ArgumentNullException(nameof(employee)); // Проверяем, что сотрудник передан
            LoadEmployeeDetails(SelectedEmployee.ID); // Загружаем детали на основе ID
        }

        private void LoadEmployeeDetails(int employeeId)
        {
            var employeeDetails = GetEmployeeDetails(employeeId);
            this.DataContext = employeeDetails; // Привязываем ViewModel к DataContext
        }

        public EmployeeViewModel GetEmployeeDetails(int employeeId)
        {
            using (_dbContext = new DataBaseConnect())
            {
                var employee = _dbContext.Employees
                    .Where(e => e.ID == employeeId)
                    .Select(e => new
                    {
                        e.ID,
                        e.FullName,
                        e.Position,
                        DepartmentName = e.Department.departmentName,
                        e.Email,
                        e.Phone,
                        e.Address,
                        BirthDate = SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", e.BirthDate)) + "-" +
                                    SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", e.BirthDate)) + "-" +
                                    SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", e.BirthDate)),
                        e.EmergencyContact,
                        e.Salary,
                        e.MaritalStatus,
                        e.Experience,
                        StatusName = e.Status.statusName,
                        ActiveProjects = _dbContext.Projects
                            .Where(p => p.EmployeeId == e.ID)
                            .Select(p => new { p.Name, p.Role }),
                        Achievements = _dbContext.Achievements
                            .Where(a => a.EmployeeId == e.ID)
                            .Select(a => new {
                                a.Title,
                                AchievementDate = SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", a.AchievementDate)) + "-" +
                                                  SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", a.AchievementDate)) + "-" +
                                                  SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", a.AchievementDate))
                            })
                    })
                    .Select(e => new EmployeeViewModel
                    {
                        ID = e.ID,
                        FullName = e.FullName,
                        Position = e.Position,
                        Department = e.DepartmentName ?? "Не указан",
                        Email = e.Email,
                        Phone = e.Phone,
                        Address = e.Address,
                        BirthDate = e.BirthDate,
                        EmergencyContact = e.EmergencyContact,
                        Salary = e.Salary,
                        MaritalStatus = e.MaritalStatus,
                        Experience = e.Experience,
                        Status = e.StatusName ?? "Не указан",
                        ActiveProjects = e.ActiveProjects
                            .Select(p => new ProjectViewModel { Name = p.Name, Role = p.Role })
                            .ToList(),
                        Achievements = e.Achievements
                            .Select(a => new AchievementViewModel
                            {
                                Title = a.Title,
                                AchievementDate = a.AchievementDate
                            })
                            .ToList()
                    })
                    .FirstOrDefault();

                return employee;
            }
        }
    }

    // ViewModel для отображения данных сотрудника
    public class EmployeeViewModel
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
        public decimal Salary { get; set; }
        public int MaritalStatus { get; set; }
        public int Experience { get; set; }
        public string Status { get; set; }
        public List<ProjectViewModel> ActiveProjects { get; set; }
        public List<AchievementViewModel> Achievements { get; set; }
    }

    public class ProjectViewModel
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }

    public class AchievementViewModel
    {
        public string Title { get; set; }
        public string AchievementDate { get; set; }
    }
}
