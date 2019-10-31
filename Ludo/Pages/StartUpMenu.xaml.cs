using System.Windows.Controls;

namespace Ludo
{
    /// <summary>
    /// Interaction logic for StartUpMenu.xaml
    /// </summary>
    public partial class StartUpMenu : Page
    {
        public StartUpMenu()
        {
            InitializeComponent();
            DataContext = new StartUpMenuViewModel();
        }
    }
}
