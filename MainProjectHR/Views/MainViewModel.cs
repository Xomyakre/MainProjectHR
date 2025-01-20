using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using MainProjectHR.DataBase;

namespace MainProjectHR.Views
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        public ObservableCollection<ProjectCard> ProjectCards { get; set; }

        private JobClient _currentCandidate;
        public JobClient CurrentCandidate
        {
            get => _currentCandidate;
            set
            {
                _currentCandidate = value;
                OnPropertyChanged();
            }
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            // Устанавливаем Dashboard по умолчанию
            CurrentView = new Views.DashboardView();

            // Инициализируем команды
            SwitchToDashboardCommand = new RelayCommand(_ => SwitchToDashboard());
            SwitchToApplicantsCommand = new RelayCommand(_ => SwitchToApplicants());
            SwitchToEmployeeCommand = new RelayCommand(_ => SwitchToEmployee());
            SwitchToProjectCommand = new RelayCommand(_ => SwitchToProject());
            SwitchToMessagesCommand = new RelayCommand(_ => SwitchToMessages());
            SwitchToReportDetailsCommand = new RelayCommand(SwitchToReportDetails);

            ProjectCards = new ObservableCollection<ProjectCard>
            {
                new ProjectCard
                {
                    ProjectImage = "Images/project1.png",
                    ProjectName = "Project Alpha",
                    Workers = new List<string> { "Alice", "Bob", "Charlie" }
                },
                new ProjectCard
                {
                    ProjectImage = "Images/project2.png",
                    ProjectName = "Project Beta",
                    Workers = new List<string> { "Diana", "Ethan" }
                }
            };
        }

        public ICommand SwitchToDashboardCommand { get; }
        public ICommand SwitchToApplicantsCommand { get; }
        public ICommand SwitchToEmployeeCommand { get; }
        public ICommand SwitchToProjectCommand { get; }
        public ICommand SwitchToMessagesCommand { get; }
        public ICommand SwitchToReportDetailsCommand { get; }

        private void SwitchToDashboard() => CurrentView = new Views.DashboardView();
        private void SwitchToApplicants() => CurrentView = new Views.ApplicantsView();
        private void SwitchToEmployee() => CurrentView = new Views.EmployeeView();
        private void SwitchToProject() => CurrentView = new Views.ProjectView();
        private void SwitchToMessages() => CurrentView = new Views.MessagesView();

        private void SwitchToReportDetails(object parameter)
        {
            if (parameter is JobClient candidate)
            {
                CurrentCandidate = candidate;
                CurrentView = new Views.ReportDetailsControl(candidate);
            }
        }

        public void NavigateToReportDetails(JobClient candidate)
        {
            CurrentView = new ReportDetailsControl(candidate);
        }

        public void NavigateToApplicantsView()
        {
            CurrentView = new ApplicantsView();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
