using System;

namespace StudyGotchi.Models
{
    public class StudyTask
    {
        private string _taskName;
        private DateTime _deadline;
        private bool _isCompleted;

        public void Complete() { throw new NotImplementedException(); }
        public TimeSpan GetTimeRemaining() { throw new NotImplementedException(); }
        public bool IsOverdue() { throw new NotImplementedException(); }
    }
}