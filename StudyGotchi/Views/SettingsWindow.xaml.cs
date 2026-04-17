// SettingsWindow.xaml.cs

using StudyGotchi.Managers;
using System.Windows;

namespace StudyGotchi.Views
{
    // Logic for the settings menu
    public partial class SettingsWindow : Window
    {
        private readonly SessionManager _manager;

        public SettingsWindow(SessionManager manager)
        {
            InitializeComponent();
            _manager = manager;

            // Load the current pet's name if we already made one
            if (_manager.ActivePet != null)
                PetNameInput.Text = _manager.ActivePet.Name;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the pet name
            if (_manager.ActivePet != null)
            {
                string name = PetNameInput.Text.Trim();
                if (!string.IsNullOrEmpty(name))
                    _manager.ActivePet.Name = name;
            }

            // TODO: Hook up the difficulty slider to the manager when we add that feature

            this.Close();
        }
    }
}