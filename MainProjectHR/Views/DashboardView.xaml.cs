using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using MainProjectHR.DataBase;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace MainProjectHR.Views
{
    /// <summary>
    /// Логика взаимодействия для DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl, INotifyPropertyChanged
    {
        private double _ratingValue;
        private double _performanceValue;
        private double _activityValue;

        public double RatingValue
        {
            get => _ratingValue;
            set
            {
                _ratingValue = value;
                OnPropertyChanged(nameof(RatingValue));
            }
        }

        public double PerformanceValue
        {
            get => _performanceValue;
            set
            {
                _performanceValue = value;
                OnPropertyChanged(nameof(PerformanceValue));
            }
        }

        public double ActivityValue
        {
            get => _activityValue;
            set
            {
                _activityValue = value;
                OnPropertyChanged(nameof(ActivityValue));
            }
        }

        private readonly DataBaseConnect _dbContext;

        public DashboardView()
        {
            InitializeComponent();
            DataContext = this;
            _dbContext = new DataBaseConnect();
            
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            // Загрузка данных для графика (количество сотрудников по отделам)
            var departmentStats = _dbContext.Employees
                .GroupBy(e => e.DepartmentId)
                .Select(g => g.Count())
                .ToList();
            
            ChartValues = new ChartValues<double>(departmentStats.Select(x => (double)x));

            // Загрузка топ сотрудников
            var topEmployees = _dbContext.Employees
                
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.Experience)
                .Take(2)
                .ToList() // Сначала получаем список сотрудников
                .Select(e => new EmployeeItem // Затем преобразуем их в EmployeeItem
                {
                    Name = e.FullName,
                    Position = e.Position,
                    InitialLetter = e.FullName.Length > 0 ? e.FullName.Substring(0, 1) : "?",
                    Experience = e.Experience,
                    DepartmentName = e.Department?.departmentName ?? "Нет отдела",
                    RoleName = e.Role?.NameRole ?? "Нет роли"
                })
                .ToList();

            TopEmployees = new ObservableCollection<EmployeeItem>(topEmployees);

            // Загрузка статистики для инфокарточек
            TotalEmployees = _dbContext.Employees.Count(e => e.IsActive);
            AverageExperience = (int)_dbContext.Employees.Average(e => e.Experience);
            ActiveProjects = _dbContext.Projects.Count();

            // Добавляем расчет рейтинга (например, среднее значение опыта сотрудников)
            RatingValue = _dbContext.Employees
                .Where(e => e.IsActive)
                .Average(e => e.Experience);

            // Расчет производительности
            // Считаем как процент сотрудников с опытом выше среднего
            var averageExp = _dbContext.Employees.Average(e => e.Experience);
            var totalActive = _dbContext.Employees.Count(e => e.IsActive);
            var aboveAverage = _dbContext.Employees.Count(e => e.IsActive && e.Experience > averageExp);
            PerformanceValue = totalActive > 0 ? (double)aboveAverage / totalActive * 100 : 0;

            // Расчет активности
            // Считаем как процент активных проектов на одного сотрудника
            var totalProjects = _dbContext.Projects.Count();
            var totalEmployees = _dbContext.Employees.Count(e => e.IsActive);
            ActivityValue = totalEmployees > 0 ? 
                Math.Min((double)totalProjects / totalEmployees * 100, 100) : 0;
        }

        public ChartValues<double> ChartValues { get; set; }
        public ObservableCollection<EmployeeItem> TopEmployees { get; set; }
        public int TotalEmployees { get; set; }
        public int AverageExperience { get; set; }
        public int ActiveProjects { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EmployeeItem
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string InitialLetter { get; set; }
        public int Experience { get; set; }
        public string DepartmentName { get; set; }
        public string RoleName { get; set; }
    }
}
