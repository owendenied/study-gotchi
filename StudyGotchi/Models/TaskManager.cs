using System;
using System.Collections.Generic;
using System.Linq;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    public class TaskManager
    {
        private List<StudyTask> _tasks;
        private int _nextId = 1;

        public List<StudyTask> Tasks => _tasks;

        public TaskManager()
        {
            _tasks = new List<StudyTask>();
        }

        public void AddTask(string name, DateTime deadline)
        {
            var newTask = new StudyTask(name, deadline);
            _tasks.Add(newTask);
        }

        public void AddTask(string name)
        {
            AddTask(name, DateTime.Now.AddDays(1));
        }

        public void CompleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.Complete();
            }
        }

        public List<StudyTask> GetOverdueTasks()
        {
            return _tasks.Where(t => t.IsOverdue()).ToList();
        }

        internal StudyTask GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void RegisterObserver(ITaskObserver observer) { }
        public void RemoveObserver(ITaskObserver observer) { }
    }
}