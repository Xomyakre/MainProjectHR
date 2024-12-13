using MainProjectHR.DataBase;
using MainProjectHR.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static MainProjectHR.Views.MainViewModel;

namespace MainProjectHR
{
    public partial class HrManager : Window
    {
        private Button lastClickedButton = null;
        private DataBaseConnect _dbContext;
        private MainViewModel _viewModel;

        public HrManager()
        {
            InitializeComponent();
            _viewModel = new MainViewModel(); // Инициализируем ViewModel
            DataContext = _viewModel; // Устанавливаем DataContext
            LoadEmployees();
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем ссылку на кнопку, на которую кликнули
            Button clickedButton = sender as Button;

            // Если была нажата другая кнопка, то восстанавливаем исходное состояние для предыдущей кнопки
            if (lastClickedButton != null && lastClickedButton != clickedButton)
            {
                lastClickedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
                lastClickedButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D0C0FF"));
            }

            // Изменяем цвета для текущей кнопки
            clickedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e91b3"));
            clickedButton.Foreground = new SolidColorBrush(Colors.White);

            // Сохраняем текущую кнопку как последнюю нажимавшуюся
            lastClickedButton = clickedButton;
        }

        private void LoadEmployees()
        {
            using (_dbContext = new DataBaseConnect())
            {
                // Загружаем данные сотрудников из базы данных
                var employees = _dbContext.Employees
                    .Select(e => new Employee
                    {
                        ID = e.ID,
                        FullName = e.FullName,
                        Position = e.Position,
                        Phone = e.Phone,
                        Email = e.Email
                    })
                    .ToList();

                // Устанавливаем цвет для каждого сотрудника
                

                // Привязываем данные к DataGrid
                //membersDataGrid.ItemsSource = new ObservableCollection<Employee>(employees);
            }
        }


        private void LogOut(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }

    public class Employee
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
       
    }
}
