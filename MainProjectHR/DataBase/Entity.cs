using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainProjectHR.DataBase
{
    [Table("AccessRights")]
    public class AccessRight
    {
        [Key]
        public int ID { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("access_level")]
        public string accessLevel { get; set; }
    }

    [Table("Departments")]
    public class Department
    {
        [Key]
        public int ID { get; set; }

        [Column("department_name")]
        public string departmentName { get; set; }
        
    }

    [Table("employee_statuses")]
    public class EmployeeStatus
    {
        [Key]
        public int ID { get; set; }

        [Column("status_name")]
        public string statusName { get; set; }
    }

    [Table("Employees")]
    public class Employee
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("full_name")]
        [Required]
        public string FullName { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("hire_date")]
        public DateTime? HireDate { get; set; }

        [Column("department_id")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } // Навигационное свойство

        [Column("status_id")]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public virtual EmployeeStatus Status { get; set; } // Навигационное свойство

        [Column("role_id")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } // Навигационное свойство

        [Column("photo")]
        public byte[] Photo { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        [Column("emergency_contact")]
        public string EmergencyContact { get; set; }

        [Column("salary")]
        public decimal Salary { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("experience")]
        public int Experience { get; set; }

        [Column("marital_status")]
        public int MaritalStatus { get; set; }
    }


    [Table("JobClients")]
    public class JobClient
    {
        [Key]
        public int ID { get; set; }

        [Column("specialty_name")]
        public string SpecialtyName { get; set; }

        [Column("skills")]
        public string Skills { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        public int Status { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        public bool IsSelected { get; set; }

        [Column("Images")]
        public byte[] Images { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }
    }


    [Table("Reports")]
    public class Report
    {
        [Key]
        public int ID { get; set; }

        [Column("project_id")]
        public int ProjectId { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("report_type")]
        public string ReportType { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Column("FileContent")]
        public byte[] FileContent { get; set; }
    }



    [Table("Roles")]
    public class Role
    {
        [Key]
        public int ID { get; set; }

        [Column("role_name")]
        public string NameRole { get; set; }
    }

    [Table("Projects")]
    public class Project
    {
        [Key]
        public int ID { get; set; }

        [Column("name")]
        public string Name { get; set; }


        [Column("employee_id")]
        public int EmployeeId { get; set; }

        public string Role { get; set; }
    }

    [Table("Achievements")]
    public class Achievement
    {
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        [Column("achievement_date")]
        public DateTime AchievementDate { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }
    }


    [Table("Vacations")]
    public class Vacation
    {
        [Key]
        public int ID { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Column("vacation_status")]
        public string VacationStatus { get; set; }

        [Column("used_days")]
        public int UsedDays { get; set; }

        [Column("remaining_days")]
        public int RemainingDays { get; set; }
    }



    [Table("AuthCredentials")]
    public class AuthCredentials
    {
        [Key]
        public int ID { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Required]
        public string login { get; set; }

        [Required]
        public string password_hash { get; set; }
    }
}
