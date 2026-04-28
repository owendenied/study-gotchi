using System;

namespace StudyGotchi.Models
{
    public class StudyTask
    {
        private string _taskName;
        private DateTime _deadline;
        private bool _isCompleted;

        public bool IsCompletedEarly { get; private set; }

        public StudyTask(string name, DateTime deadline)
        {
            _taskName = name;
            _deadline = deadline;
            _isCompleted = false;
        }

        public void Complete()
        {
            _isCompleted = true;

            if (DateTime.Now < _deadline)
            {
                IsCompletedEarly = true;
            }
            else
            {
                IsCompletedEarly = false;
            }
        }

        public TimeSpan GetTimeRemaining()
        {
            return _deadline - DateTime.Now;
        }

        public bool IsOverdue()
        {
            // If not finished and current time is past deadline
            return !_isCompleted && DateTime.Now > _deadline;
        }

        // Getters so other classes can read the private data
        public string TaskName => _taskName;
        public bool IsCompleted => _isCompleted;
    }
}