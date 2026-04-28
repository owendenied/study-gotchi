using System;

namespace StudyGotchi.Models
{
    public class StudyTask
    {
        private string _taskName;
        private DateTime _deadline;
        private bool _isCompleted;
        private int _id;

<<<<<<< HEAD
        public bool IsCompletedEarly { get; private set; }

        public StudyTask(string name, DateTime deadline)
        {
            _taskName = name;
=======
        public StudyTask(int id, string name, DateTime deadline)
        {
            _id = id;
            _taskName = name ?? string.Empty;
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
            _deadline = deadline;
            _isCompleted = false;
        }

<<<<<<< HEAD
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
=======
        public int Id => _id;
        public string Name => _taskName;
        public DateTime Deadline => _deadline;
        public bool IsCompleted => _isCompleted;

        public void Complete()
        {
            _isCompleted = true;
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
        }

        public TimeSpan GetTimeRemaining()
        {
            return _deadline - DateTime.Now;
        }

        public bool IsOverdue()
        {
<<<<<<< HEAD
            // If not finished and current time is past deadline
            return !_isCompleted && DateTime.Now > _deadline;
        }

        // Getters so other classes can read the private data
        public string TaskName => _taskName;
        public bool IsCompleted => _isCompleted;
=======
            return !_isCompleted && DateTime.Now > _deadline;
        }
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
    }
}