// SessionManager.cs

using StudyGotchi.Models;

namespace StudyGotchi.Managers
{
    // The main controller for the app. 
    // Holds the pet, the tasks, and runs the real-time clock for hunger.
    public class SessionManager
    {
        private TamagotchiPet? _activePet;
        private TaskManager _taskManager;
        private bool _sessionActive;

        // The background timer that runs every minute
        private System.Threading.Timer? _decayTimer;
        private const int DecayIntervalMs = 60_000;

        public SessionManager()
        {
            _taskManager = new TaskManager();
            _sessionActive = false;
        }

        public TamagotchiPet? ActivePet => _activePet;
        public TaskManager TaskManager => _taskManager;
        public bool SessionActive => _sessionActive;

        // Saves the chosen pet and connects it to the task list
        public void SetActivePet(TamagotchiPet pet)
        {
            _activePet = pet;
            _taskManager.RegisterObserver(pet);
        }

        // Starts the timer when we hit "Start Session"
        public void StartSession()
        {
            if (_activePet == null)
                throw new InvalidOperationException("No pet selected before starting session.");

            _sessionActive = true;

            // Timer runs in the background. Use Invoke() in forms to update UI!
            _decayTimer = new System.Threading.Timer(
                callback: OnDecayTick,
                state: null,
                dueTime: DecayIntervalMs,
                period: DecayIntervalMs
            );
        }

        // Stops the timer when we're done
        public void EndSession()
        {
            _sessionActive = false;
            _decayTimer?.Dispose();
            _decayTimer = null;
        }

        // Generates the final text block for the summary screen
        public string GetSessionSummary()
        {
            if (_activePet == null) return "No session data available.";

            int completed = _taskManager.Tasks.Count(t => t.IsCompleted);
            int total = _taskManager.Tasks.Count;

            return $"Pet: {_activePet.Name}\n"
                 + $"Evolution Stage: {_activePet.GetEvolutionStageName()}\n"
                 + $"Level: {_activePet.CurrentLevel}\n"
                 + $"Tasks Completed: {completed} / {total}\n"
                 + $"Hunger Remaining: {_activePet.HungerLevel}%";
        }

        // Runs every minute to drop hunger and check for missed tasks
        private void OnDecayTick(object? state)
        {
            if (!_sessionActive || _activePet == null) return;

            _activePet.DecayHunger();
            _taskManager.CheckForOverdueTasks();
        }
    }
}