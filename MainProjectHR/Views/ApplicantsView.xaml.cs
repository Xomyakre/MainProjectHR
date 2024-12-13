using MainProjectHR.DataBase;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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



        //private void DetailsButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Ищем кандидата, у которого установлен CheckBox
        //    var selectedCandidate = _dbContext.JobClients.FirstOrDefault(jc => jc.IsSelected);

        //    if (selectedCandidate != null)
        //    {
        //        if (DataContext is MainViewModel viewModel)
        //        {
        //            // Передаём выбранного кандидата в команду переключения
        //            viewModel.SwitchToCandidateDetailsCommand.Execute(selectedCandidate);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Не удалось переключить вид. Проверьте DataContext.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Выберите кандидата с помощью чекбокса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //}


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
                Skills = jc.skills,
                Status = jc.status,
                IsSelected = false // Сбрасываем выбор
            }).ToList();

            CandidatesDataGrid.ItemsSource = candidates;
        }



        private void RoleFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedSpecialty = RoleFilterComboBox.SelectedItem as string;
            LoadCandidates(selectedSpecialty);
        }
    }

    public class JobClient
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string SpecialtyName { get; set; }
        public string Skills { get; set; }
        public int Status { get; set; }
        public bool IsSelected { get; set; } // Для CheckBox
    }


}
