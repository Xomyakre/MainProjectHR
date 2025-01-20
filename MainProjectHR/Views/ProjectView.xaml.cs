using MainProjectHR.DataBase;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MainProjectHR.Views.UserControls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace MainProjectHR.Views
{
    public partial class ProjectView : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<ReportViewModel> Reports { get; set; }

        private readonly DataBaseConnect _dbContext;

        public event PropertyChangedEventHandler? PropertyChanged;

        

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ProjectView()
        {
            InitializeComponent();
            _dbContext = new DataBaseConnect();
            LoadReports();
            DataContext = this;
        }

        private int _lastReportId;
        public int LastReportId
        {
            get => _lastReportId;
            set
            {
                _lastReportId = value;
                OnPropertyChanged(nameof(LastReportId));
            }
        }

        private int _totalEmployees;
        public int TotalEmployees
        {
            get => _totalEmployees;
            set
            {
                _totalEmployees = value;
                OnPropertyChanged(nameof(TotalEmployees));
            }
        }

        private int _totalProjects;
        public int TotalProjects
        {
            get => _totalProjects;
            set
            {
                _totalProjects = value;
                OnPropertyChanged(nameof(TotalProjects));
            }
        }

        private void LoadReports()
        {
            try
            {
                // Получение отчетов из базы
                var reports = _dbContext.Reports
                    .Select(report => new
                    {
                        report.ID,
                        Title = report.ReportType,
                        report.CreationDate,
                        report.Description,
                        report.FileContent
                    })
                    .ToList()
                    .Select(r => new ReportViewModel
                    {
                        ID = r.ID,
                        Title = r.Title,
                        CreationDate = r.CreationDate.ToString("dd.MM.yyyy"),
                        Description = r.Description,
                        FileContent = r.FileContent
                    })
                    .ToList();

                Reports = new ObservableCollection<ReportViewModel>(reports);

                // Добавляем подсчет сотрудников и проектов
                TotalEmployees = _dbContext.Employees.Count();
                TotalProjects = _dbContext.Projects.Count();
                LastReportId = _dbContext.Reports.Count();
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }






    }

    public class ReportViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string CreationDate { get; set; }
        public string Description { get; set; }

        // Свойство для содержимого файла
        public byte[] FileContent { get; set; }

        public ICommand DownloadCommand { get; }

        public ReportViewModel()
        {
            // Инициализация команды скачивания
            DownloadCommand = new RelayCommand(DownloadFile);
        }

        private void DownloadFile(object parameter)
        {
            var reportId = (int)parameter;
            try
            {
                var directoryPath = @"C:\Hi-Agent";
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }

                var filePath = System.IO.Path.Combine(directoryPath, $"Report_{reportId}.docx");

                using (WordprocessingDocument wordDocument =
                    WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    // Добавляем заголовок
                    Paragraph headingPara = body.AppendChild(new Paragraph());
                    Run headingRun = headingPara.AppendChild(new Run());
                    headingRun.AppendChild(new Text($"Отчет #{reportId}"));

                    // Добавляем дату
                    Paragraph datePara = body.AppendChild(new Paragraph());
                    Run dateRun = datePara.AppendChild(new Run());
                    dateRun.AppendChild(new Text($"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm}"));

                    // Добавляем название отчета
                    Paragraph titlePara = body.AppendChild(new Paragraph());
                    Run titleRun = titlePara.AppendChild(new Run());
                    titleRun.AppendChild(new Text($"Название: {Title}"));

                    // Добавляем описание
                    Paragraph descPara = body.AppendChild(new Paragraph());
                    Run descRun = descPara.AppendChild(new Run());
                    descRun.AppendChild(new Text($"Описание: {Description}"));

                    // Добавляем дополнительную информацию
                    Paragraph infoPara = body.AppendChild(new Paragraph());
                    Run infoRun = infoPara.AppendChild(new Run());
                    infoRun.AppendChild(new Text("\nДополнительная информация:"));

                    // Добавляем статистику
                    Paragraph statsPara = body.AppendChild(new Paragraph());
                    Run statsRun = statsPara.AppendChild(new Run());
                    statsRun.AppendChild(new Text($@"
- Дата создания отчета: {CreationDate}
- Идентификатор отчета: {ID}
- Размер файла: {(FileContent?.Length ?? 0) / 1024} КБ
- Сгенерировано системой Hi-Agent
- Время генерации: {DateTime.Now:HH:mm:ss}
Документ имеет очень-очень важную информацию"));

                    // Добавляем нижний колонтитул
                    Paragraph footerPara = body.AppendChild(new Paragraph());
                    Run footerRun = footerPara.AppendChild(new Run());
                    footerRun.AppendChild(new Text("\n\nДокумент сгенерирован автоматически. Требуется подпись ответственного лица."));
                }

                ShowNotification($"Отчет успешно сгенерирован и сохранен по пути: {filePath}");
            }
            catch (Exception ex)
            {
                ShowNotification($"Ошибка при генерации отчета: {ex.Message}", isError: true);
            }
        }

        private void ShowNotification(string message, bool isError = false)
        {
            var notification = new NotificationPopup(message);
            
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null) return;

            var container = new Grid { Background = System.Windows.Media.Brushes.Transparent };
            container.Children.Add(notification);

            var popup = new Popup
            {
                Child = container,
                PlacementTarget = mainWindow,
                Placement = PlacementMode.Top,
                HorizontalOffset = mainWindow.Width - 300,
                VerticalOffset = 120,
                IsOpen = true,
                AllowsTransparency = true,
                StaysOpen = false
            };

            if (popup.Parent is UIElement parent)
            {
                parent.SetValue(Panel.BackgroundProperty, System.Windows.Media.Brushes.Transparent);
            }

            var slideAnimation = new DoubleAnimation
            {
                From = 100,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            var fadeAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            notification.BeginAnimation(UIElement.OpacityProperty, fadeAnimation);
            ((notification.Content as System.Windows.Controls.Border)?.RenderTransform as TranslateTransform)?.BeginAnimation(TranslateTransform.XProperty, slideAnimation);

            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };

            timer.Tick += (s, e) =>
            {
                var slideOutAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(0.3),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };

                var fadeOutAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                fadeOutAnimation.Completed += (s2, e2) => popup.IsOpen = false;
                
                notification.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
                ((notification.Content as System.Windows.Controls.Border)?.RenderTransform as TranslateTransform)?.BeginAnimation(TranslateTransform.XProperty, slideOutAnimation);
                
                timer.Stop();
            };

            timer.Start();
        }

        

    }
}
