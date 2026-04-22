using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class PetSelectionView : UserControl
    {
        public PetSelectionView()
        {
            InitializeComponent();
        }

        private void BtnChoose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // after selecting a pet, go to task setup
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToTaskSetup();
        }
    }
}