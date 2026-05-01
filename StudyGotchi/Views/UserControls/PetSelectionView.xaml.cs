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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Optional: button click handler from eume's branch if there is a specific button
        }
    }
}