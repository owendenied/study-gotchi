// SessionSummaryForm.cs

using StudyGotchi.Managers;

namespace StudyGotchi.Forms
{
    // The final screen showing how much you got done.
    public partial class SessionSummaryForm : Form
    {
        private readonly SessionManager _manager;

        public SessionSummaryForm(SessionManager manager)
        {
            InitializeComponent();
            _manager = manager;
        }

        // Sets up the UI when the form opens
        public void DisplaySummary()
        {
            // TODO:
            // 1. Get text from _manager.GetSessionSummary()
            // 2. Put the text into the UI labels
            // 3. Show the pet's final form in the PictureBox
            throw new NotImplementedException();
        }
    }
}