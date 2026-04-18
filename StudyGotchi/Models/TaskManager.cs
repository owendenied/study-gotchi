using System;
using System.Collections.Generic;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    public class TaskManager
    {
        private List<StudyTask> _tasks;

        public TaskManager()
        {
            _tasks = new List<StudyTask>();
        }

        public void AddTask(string name) { throw new NotImplementedException(); }
        public void AddTask(string name, DateTime deadline) { throw new NotImplementedException(); }
        public void CompleteTask(int id) { throw new NotImplementedException(); }
        public List<StudyTask> GetOverdueTasks() { throw new NotImplementedException(); }
        public void CheckForOverdueTasks() { throw new NotImplementedException(); }
        public void RegisterObserver(ITaskObserver observer) { throw new NotImplementedException(); }
        public void RemoveObserver(ITaskObserver observer) { throw new NotImplementedException(); }
    }
}