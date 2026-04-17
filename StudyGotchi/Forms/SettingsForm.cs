// SettingsForm.cs

using StudyGotchi.Managers;

namespace StudyGotchi.Forms
{
    // Quick settings window to name the pet or change difficulty.
    public partial class SettingsForm : Form
    {
        private readonly SessionManager _manager;

        public SettingsForm(SessionManager manager)
        {
            InitializeComponent();
            _manager = manager;
        }

        // Runs when hitting "Save"
        private void OnSaveSettings()
        {
            // TODO:
            // 1. Set _manager.ActivePet.Name from the text box
            // 2. Update the hunger speed from the trackbar (need to add a setter for this)
            // 3. Close the window
            throw new NotImplementedException();
        }

        // Space to add extra pet-specific tweaks later if we have time
        private void AdjustPetSettings()
        {
            // TODO: extra settings
            throw new NotImplementedException();
        }
    }
}