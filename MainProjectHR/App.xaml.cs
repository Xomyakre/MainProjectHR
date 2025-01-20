using MainProjectHR.DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MainProjectHR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DataBaseConnect _dbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Инициализируем контекст базы данных
            _dbContext = new DataBaseConnect();
            
            try
            {
                _dbContext.Database.Connection.Open();
                _dbContext.Database.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
                Shutdown();
                return;
            }
        }
    }
}
