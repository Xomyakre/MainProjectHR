using MainProjectHR.DataBase;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MainProjectHR.Views
{
    public partial class ProjectView : UserControl
    {
        
        public ObservableCollection<Report> Reports { get; set; }
        public Report SelectedReport { get; set; }
        

        private readonly DataBaseConnect _dbContext;

        public ProjectView()
        {
            InitializeComponent();
            _dbContext = new DataBaseConnect();
            LoadReports();
            
        }

        private void LoadReports()
        {
            try
            {
                // Загружаем данные из базы
                var reports = _dbContext.Reports
                    .Select(report => new
                    {
                        report.ID,
                        Title = report.ReportType, // Используем поле ReportType как Title
                        report.CreationDate,
                        report.Description
                    })
                    .ToList() // Выполняем запрос и загружаем данные в память
                    .Select(r => new ReportViewModel
                    {
                        ID = r.ID,
                        Title = r.Title,
                        CreationDate = r.CreationDate.ToString("dd.MM.yyyy"), // Форматирование даты после загрузки
                        Description = r.Description,
                        IconImage = "/Images/u1.png" 
                    })
                    .ToList();

                // Привязываем данные к DataGrid
                ReportsDataGrid.ItemsSource = reports;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is Report selectedReport)
            {
                // Передача выбранного отчета в метод переключения
                if (DataContext is MainViewModel viewModel)
                {
                    viewModel.SwitchToReportDetailsCommand.Execute(selectedReport);
                }
            }
        }


        public class ReportViewModel
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string CreationDate { get; set; }
            public string Description { get; set; }
            public string IconImage { get; set; }
        }

        private void ReportsDataGrid_Selected(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is Report selectedReport)
            {
                // Передача выбранного отчета в метод переключения
                if (DataContext is MainViewModel viewModel)
                {
                    viewModel.SwitchToReportDetailsCommand.Execute(selectedReport);
                }
            }

        }
    }
}