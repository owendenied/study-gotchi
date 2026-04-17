// StudyTask.cs

namespace StudyGotchi.Models
{
    // Just a basic data class for a single task. No UI stuff here.
    public class StudyTask
    {
        private readonly int _id;
        private string _taskName;
        private DateTime _deadline;
        private bool _isCompleted;

        public StudyTask(int id, string taskName, DateTime deadline)
        {
            _id = id;
            _taskName = taskName;
            _deadline = deadline;
            _isCompleted = false;
        }

        public int Id => _id;
        public string TaskName { get => _taskName; set => _taskName = value; }
        public DateTime Deadline { get => _deadline; set => _deadline = value; }
        public bool IsCompleted => _isCompleted;

        // Marks the task done. 
        public void Complete()
        {
            _isCompleted = true;
        }

        // How much time is left before it's late?
        public TimeSpan GetTimeRemaining()
        {
            TimeSpan remaining = _deadline - DateTime.Now;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }

        // Is it past the deadline and still not done?
        public bool IsOverdue()
        {
            return !_isCompleted && DateTime.Now > _deadline;
        }

        public override string ToString() => _taskName;
    }
}