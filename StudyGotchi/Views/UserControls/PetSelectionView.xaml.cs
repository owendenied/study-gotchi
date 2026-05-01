using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class PetSelectionView : UserControl
    {
        public PetSelectionView()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                StudyGotchi.Services.ServiceRegistry.AudioService?.PlaySfx("sfx_hover");
            }
        }

    }
}