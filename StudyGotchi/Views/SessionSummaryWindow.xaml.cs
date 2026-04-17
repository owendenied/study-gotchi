// SessionSummaryWindow.xaml.cs

using StudyGotchi.Managers;
using System.Windows;

namespace StudyGotchi.Views
{
    // Code for the end-of-session summary screen
    public partial class SessionSummaryWindow : Window
    {
        private readonly SessionManager _manager;

        public SessionSummaryWindow(SessionManager manager)
        {
            InitializeComponent();
            _manager = manager;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplaySummary();
        }

        // Grabs the final numbers from the manager and puts them on screen
        public void DisplaySummary()
        {
            if (_manager.ActivePet == null) return;

            var pet = _manager.ActivePet;
            int completed = _manager.TaskManager.Tasks.Count(t => t.IsCompleted);
            int total = _manager.TaskManager.Tasks.Count;

            SummaryPetName.Text = pet.Name;
            SummaryEvolution.Text = pet.GetEvolutionStageName();
            SummaryLevel.Text = $"Level {pet.CurrentLevel}";
            SummaryTasks.Text = $"{completed} / {total}";
            SummaryHunger.Text = $"{pet.HungerLevel}%";

            // TODO: Uncomment when art is ready
            // FinalPetSprite.Source = ConvertToBitmapImage(pet.GetCurrentSprite());
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the whole app
            Application.Current.Shutdown();
        }
    }
}