// MainSetupForm.cs

using StudyGotchi.Managers;
using System.Drawing;

namespace StudyGotchi.Forms
{
    // The main setup screen for picking a pet and adding tasks.
    // Handles UI and talks to SessionManager.
    public partial class MainSetupForm : Form
    {
        private readonly SessionManager _manager;

        public MainSetupForm()
        {
            InitializeComponent();
            _manager = new SessionManager();
        }

        // Runs when "Start Session" is clicked.
        private void OnStartSession()
        {
            // TODO:
            // 1. Make sure a pet is picked and we have tasks
            // 2. _manager.StartSession()
            // 3. Open StudyWidgetForm with _manager
            // 4. Hide this window
            throw new NotImplementedException();
        }

        // Runs when "Add Task" is clicked.
        private void OnAddTask()
        {
            // TODO:
            // 1. Grab text and date from UI
            // 2. _manager.TaskManager.AddTask(...)
            // 3. Update the list view
            throw new NotImplementedException();
        }

        // Runs when a pet is clicked in the panel.
        private void OnPetSelected(int petIndex)
        {
            // TODO:
            // petIndex 0 = PetA, 1 = PetB, 2 = PetC
            // _manager.SetActivePet(selectedPet)
            throw new NotImplementedException();
        }
    }
}