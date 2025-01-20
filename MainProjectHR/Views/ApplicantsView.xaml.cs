using MainProjectHR.DataBase;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MainProjectHR.Views
{
    public partial class ApplicantsView : UserControl
    {
        private readonly DataBaseConnect _dbContext;

        public ApplicantsView()
        {
            InitializeComponent();
            _dbContext = new DataBaseConnect();
            InitializeFilters();
            LoadCandidates();
        }

        private JobClient _selectedCandidate;

        private void CandidatesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранного кандидата
            _selectedCandidate = CandidatesDataGrid.SelectedItem as JobClient;
        }


        private void InitializeFilters()
        {
            // Загрузка списка уникальных специальностей
            var specialties = _dbContext.JobClients
                                        .Select(jc => jc.SpecialtyName)
                                        .Distinct()
                                        .ToList();

            specialties.Insert(0, "Все специальности"); // Добавляем пункт "Все специальности"
            RoleFilterComboBox.ItemsSource = specialties;
            RoleFilterComboBox.SelectedIndex = 0;
        }






        private void LoadCandidates(string specialty = null)
        {
            var query = _dbContext.JobClients.AsQueryable();

            if (!string.IsNullOrEmpty(specialty) && specialty != "Все специальности")
            {
                query = query.Where(jc => jc.SpecialtyName == specialty);
            }

            var candidates = query.Select(jc => new JobClient
            {
                ID = jc.ID,
                FullName = jc.FullName,
                SpecialtyName = jc.SpecialtyName,
                Skills = jc.Skills,
                Status = jc.Status,
                Phone = jc.Phone, 
                Email = jc.Email, 
                IsSelected = false,
                Images = jc.Images,
                DepartmentId = jc.DepartmentId,
                RoleId = jc.RoleId
            }).ToList();

            CandidatesDataGrid.ItemsSource = candidates;
        }




        private void RoleFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedSpecialty = RoleFilterComboBox.SelectedItem as string;
            LoadCandidates(selectedSpecialty);
        }

        private void CandidatesDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CandidatesDataGrid.SelectedItem is JobClient selectedCandidate)
            {
                // Находим главное окно и его DataContext
                var mainWindow = Application.Current.Windows.OfType<HrManager>().FirstOrDefault();
                if (mainWindow?.DataContext is MainViewModel mainViewModel)
                {
                    mainViewModel.CurrentCandidate = selectedCandidate;
                    mainViewModel.SwitchToReportDetailsCommand.Execute(selectedCandidate);
                }
            }
        }

    }

    public class JobClient : INotifyPropertyChanged
    {
        private int _status;
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int ID { get; set; }
        public string FullName { get; set; }
        public string SpecialtyName { get; set; }
        public string Skills { get; set; }
        public int Status 
        { 
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusColor));
                    OnPropertyChanged(nameof(StatusMessage));
                }
            }
        }
        public bool IsSelected { get; set; }
        public byte[] Images { get; set; }
        public string Phone { get; set; } 
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }

        public string StatusColor => Status >= 5 ? "#4CAF50" : "#F44336";
        public string StatusMessage => Status >= 5 ? "Это лучший кандидат" : "Этот кандидат нам не подходит";
    }



}
