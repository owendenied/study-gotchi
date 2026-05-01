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
        public DateTime Deadline => _deadline;
        public bool IsCompleted => _isCompleted;

        public string DisplayDeadline 
        {
            get 
            {
                if (_deadline.Date == DateTime.Today)
                    return $"Due Today at {_deadline:h:mm tt}";
                else if (_deadline.Date == DateTime.Today.AddDays(1))
                    return $"Due Tomorrow at {_deadline:h:mm tt}";
                else
                    return $"Due {_deadline:MMM d} at {_deadline:h:mm tt}";
            }
        }

        public void Complete()
        {
            _isCompleted = true;
            IsCompletedEarly = DateTime.Now < _deadline;
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