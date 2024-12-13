using System.Windows.Controls;

namespace MainProjectHR.Views
{
    public partial class CandidateDetailsView : UserControl
    {
        public CandidateDetailsView(JobClient candidate)
        {
            InitializeComponent();
            DataContext = candidate; // Устанавливаем выбранного кандидата как DataContext
        }
    }
}
