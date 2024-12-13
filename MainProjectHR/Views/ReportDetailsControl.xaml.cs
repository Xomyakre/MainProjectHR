using System.Windows.Controls;

namespace MainProjectHR.Views
{
    public partial class ReportDetailsControl : UserControl
    {
        public ReportDetailsControl(object reportDetails)
        {
            InitializeComponent();
            DataContext = reportDetails; // Устанавливаем данные в контекст
        }
    }
}
