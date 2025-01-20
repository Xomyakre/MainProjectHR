using System.Windows.Controls;
using System.Windows;

namespace MainProjectHR.Views.UserControls
{
    public partial class NotificationPopup : UserControl
    {
        public NotificationPopup(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }
    }
}