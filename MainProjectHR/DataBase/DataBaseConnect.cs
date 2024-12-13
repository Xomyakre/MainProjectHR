using System;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Net;

namespace MainProjectHR.DataBase
{
    public class DataBaseConnect : DbContext
    {
        public DataBaseConnect() : base(@"Server=SEVERMEDVED\SQLEXPRESS;Database=DB-Sever;Integrated Security=True;")
        {
        }

        public DbSet<AuthCredentials> AuthCredentials { get; set; }
        public DbSet<AccessRight> AccessRights { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeStatus> EmployeeStatuses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobClient> JobClients { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Role> Roles { get; set; }

        // Метод для проверки логина и пароля
        public bool CheckCredentials(string login, string password)
        {
            var user = AuthCredentials.FirstOrDefault(u => u.login == login && u.password_hash == password);
            return user != null;
        }

        // Метод для добавления нового пользователя
        public bool AddUser(string login, string password)
        {
            var existingUser = AuthCredentials.FirstOrDefault(u => u.login == login);
            if (existingUser != null)
            {
                return false;
            }

            var newUser = new AuthCredentials
            {
                login = login,
                password_hash = password
            };

            AuthCredentials.Add(newUser);
            SaveChanges();

            return true;
        }
    }
}
