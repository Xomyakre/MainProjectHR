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

        public string module { get; set; }
    }

    [Table("Departments")]
    public class Department
    {
        [Key]
        public int ID { get; set; }

        public string name { get; set; }
        
    }

    [Table("EmployeeStatuses")]
    public class EmployeeStatus
    {
        [Key]
        public int ID { get; set; }

        public string name { get; set; }
    }

    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int ID { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("hire_date")]
        public DateTime HireDate { get; set; }

        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }
    }

    [Table("JobClients")]
    public class JobClient
    {
        [Key]
        public int ID { get; set; }

        [Column("specialty_name")]
        public string SpecialtyName { get; set; }

        
        public string skills { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        public int status { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        public bool IsSelected { get; set; }
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
        public String Description { get; set; }



        [Column("author_id")]
        public int AuthorId { get; set; }
    }

    [Table("Roles")]
    public class Role
    {
        [Key]
        public int ID { get; set; }

        [Column("name_role")]
        public string NameRole { get; set; }
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
