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
        public StudyTaskType TaskType { get; }

        public StudyTask(int id, string name, DateTime deadline)
            : this(id, name, deadline, StudyTaskType.Activity)
        {
        }

        public StudyTask(int id, string name, DateTime deadline, StudyTaskType taskType)
        {
            _id = id;
            _taskName = name ?? string.Empty;
            _deadline = deadline;
            _isCompleted = false;
            TaskType = taskType;
        }

        public StudyTask(int id, string name, DateTime deadline, bool isCompleted, bool isCompletedEarly)
            : this(id, name, deadline, StudyTaskType.Activity, isCompleted, isCompletedEarly)
        {
        }

        public StudyTask(int id, string name, DateTime deadline, StudyTaskType taskType, bool isCompleted, bool isCompletedEarly)
        {
            _id = id;
            _taskName = name ?? string.Empty;
            _deadline = deadline;
            _isCompleted = isCompleted;
            IsCompletedEarly = isCompletedEarly;
            TaskType = taskType;
        }

        public int Id => _id;
        public string Name => _taskName;
        public DateTime Deadline => _deadline;
        public bool IsCompleted => _isCompleted;
        public string TypeLabel => TaskType.GetDisplayName();
        public int BaseXpReward => TaskType.GetBaseXpReward();
        public string TypeChipColor => TaskType.GetChipColor();

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

        public bool Complete()
        {
            if (_isCompleted) return false;

            _isCompleted = true;
            IsCompletedEarly = DateTime.Now < _deadline;
            return true;
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
