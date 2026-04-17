// StudyWidgetForm.cs

using StudyGotchi.Managers;

namespace StudyGotchi.Forms
{
    // The small floating window while studying. 
    // Remember to set TopMost = true in the designer!
    public partial class StudyWidgetForm : Form
    {
        private readonly SessionManager _manager;

        public StudyWidgetForm(SessionManager manager)
        {
            InitializeComponent();
            _manager = manager;
        }

        // Updates the progress bars and pet picture. 
        // Called when a timer ticks or a task is done.
        public void UpdatePetDisplay()
        {
            // TODO:
            // 1. Show _manager.ActivePet.GetCurrentSprite()
            // 2. Update Hunger bar
            // 3. Update XP bar
            // 4. Make hunger bar change colors if it gets too low
            throw new NotImplementedException();
        }

        // When a checkbox is clicked in the list
        private void OnTaskChecked(int taskId)
        {
            // TODO:
            // 1. Tell task manager it's done: _manager.TaskManager.CompleteTask(taskId)
            // 2. Refresh UI to show the new XP
            throw new NotImplementedException();
        }

        // When we want to stop studying
        private void OnEndSession()
        {
            // TODO:
            // 1. _manager.EndSession()
            // 2. Open SessionSummaryForm
            // 3. Close this small widget
            throw new NotImplementedException();
        }
    }
}