using System;

namespace StudyGotchi.Models
{
    public class StudyTask
    {
        private string _taskName;
        private DateTime _deadline;
        private bool _isCompleted;
        private int _id;

        public StudyTask(int id, string name, DateTime deadline)
        {
            _id = id;
            _taskName = name ?? string.Empty;
            _deadline = deadline;
            _isCompleted = false;
        }

        public int Id => _id;
        public string Name => _taskName;
        public DateTime Deadline => _deadline;
        public bool IsCompleted => _isCompleted;

        public void Complete()
        {
            _isCompleted = true;
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