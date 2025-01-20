using System;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Net;
using System.Data.SqlClient;
using MainProjectHR.Views;

namespace MainProjectHR.DataBase
{
    public class DataBaseConnect : DbContext
    {
        private readonly string connectionString;

        public DataBaseConnect() : base(@"Server=SEVERMEDVED\SQLEXPRESS;Database=DB-Sever;Integrated Security=True;")
        {
            connectionString = @"Server=SEVERMEDVED\SQLEXPRESS;Database=DB-Sever;Integrated Security=True;";
        }

        public DbSet<AuthCredentials> AuthCredentials { get; set; }
        public DbSet<AccessRight> AccessRights { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeStatus> EmployeeStatuses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobClient> JobClients { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Vacation> Vacations { get; set; }

        // Метод для проверки логина и пароля
        public bool CheckCredentials(string login, string password)
        {
            var user = AuthCredentials.FirstOrDefault(u => u.login == login && u.password_hash == password);
            return user != null;
        }

        // Обновленный метод для добавления нового пользователя
        public bool AddUser(string login, string password)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    // Проверяем, существует ли пользователь
                    var existingUser = AuthCredentials.FirstOrDefault(u => u.login == login);
                    if (existingUser != null)
                    {
                        return false;
                    }

                    // Создаем базового сотрудника
                    var newEmployee = new Employee
                    {
                        FullName = "Новый Сотрудник",
                        Position = "Стажер",
                        DepartmentId = 1, // Базовый отдел
                        StatusId = 1, // Базовый статус
                        RoleId = 1, // Базовая роль
                        IsActive = true,
                        HireDate = DateTime.Now,
                        Experience = 0,
                        MaritalStatus = 1 // Базовый статус
                    };

                    Employees.Add(newEmployee);
                    SaveChanges(); // Сохраняем, чтобы получить ID сотрудника

                    // Создаем учетные данные
                    var newUser = new AuthCredentials
                    {
                        login = login,
                        password_hash = password,
                        EmployeeId = newEmployee.ID // Используем ID созданного сотрудника
                    };

                    AuthCredentials.Add(newUser);
                    SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public void ExecuteQuery(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Добавляем метод для получения строки подключения
        public string GetConnectionString()
        {
            return connectionString;
        }
    }
}
