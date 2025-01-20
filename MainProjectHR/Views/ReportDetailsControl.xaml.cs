using System;
using System.Windows;
using System.Windows.Controls;
using MainProjectHR.DataBase;
using System.Linq;

namespace MainProjectHR.Views
{
    public partial class ReportDetailsControl : UserControl
    {
        public JobClient Candidate { get; }
        private readonly DataBaseConnect _dbContext;

        public ReportDetailsControl(JobClient candidate)
        {
            InitializeComponent();

            Candidate = candidate;
            DataContext = Candidate;
            _dbContext = new DataBaseConnect(); // Подключение к базе данных
        }

        private void OnAcceptCandidate(object sender, RoutedEventArgs e)
        {
            try
            {
                _dbContext.ExecuteQuery($@"
                    INSERT INTO Employees (
                        full_name, 
                        position, 
                        phone, 
                        email, 
                        hire_date, 
                        department_id, 
                        status_id, 
                        role_id,
                        salary,
                        marital_status,
                        experience
                    )
                    VALUES (
                        '{Candidate.FullName}', 
                        '{Candidate.SpecialtyName}', 
                        '{Candidate.Phone}', 
                        '{Candidate.Email}', 
                        GETDATE(), 
                        {Candidate.DepartmentId}, 
                        {Candidate.Status}, 
                        {Candidate.RoleId},
                        50000,
                        0,
                        0
                    );
                    
                    DELETE FROM JobClients WHERE ID = {Candidate.ID};
                ");

                MessageBox.Show("Кандидат успешно принят на работу!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                var mainViewModel = (MainViewModel)Application.Current.MainWindow.DataContext;
                mainViewModel.NavigateToApplicantsView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении кандидата: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnRejectCandidate(object sender, RoutedEventArgs e)
        {
            try
            {
                // Удаляем кандидата из таблицы JobClients
                _dbContext.ExecuteQuery($"DELETE FROM JobClients WHERE ID = {Candidate.ID};");

                MessageBox.Show("Заявка кандидата отклонена.", "Отклонено", MessageBoxButton.OK, MessageBoxImage.Information);

                // Вернуться на список кандидатов
                var mainViewModel = (MainViewModel)Application.Current.MainWindow.DataContext;
                mainViewModel.NavigateToApplicantsView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении кандидата: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Находим главное окно и его DataContext
            var mainWindow = Application.Current.Windows.OfType<HrManager>().FirstOrDefault();
            if (mainWindow?.DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.SwitchToApplicantsCommand.Execute(null);
            }
            else
            {
                // Если что-то пошло не так, хотя бы покажем сообщение
                MessageBox.Show("Не удалось выполнить переход к списку кандидатов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
