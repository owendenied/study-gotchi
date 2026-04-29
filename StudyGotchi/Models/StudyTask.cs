using System;

namespace StudyGotchi.Models
{
    public class StudyTask
    {
        private string _taskName;
        private DateTime _deadline;
        private bool _isCompleted;
        private int _id;

        public bool IsCompletedEarly { get; private set; }

        public StudyTask(int id, string name, DateTime deadline)
        {
            _id = id;
            _taskName = name ?? string.Empty;
            _deadline = deadline;
            _isCompleted = false;
        }

        public int Id => _id;
        public string Name => _taskName;
        public string TaskName => _taskName; // Kept for compatibility with HEAD usages
        public DateTime Deadline => _deadline;
        public bool IsCompleted => _isCompleted;

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
            return !_isCompleted && DateTime.Now > _deadline;
        }
    }
}