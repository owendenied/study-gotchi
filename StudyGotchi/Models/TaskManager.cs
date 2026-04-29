using System;
using System.Collections.Generic;
using System.Linq;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    public class TaskManager
    {
        private List<StudyTask> _tasks;
<<<<<<< HEAD
        private int _nextId = 1;

        public List<StudyTask> Tasks => _tasks;
=======
        private List<ITaskObserver> _observers = new List<ITaskObserver>();
        private int _nextId = 1;
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977

        public TaskManager()
        {
            _tasks = new List<StudyTask>();
        }

<<<<<<< HEAD
        public void AddTask(string name, DateTime deadline)
        {
            var newTask = new StudyTask(name, deadline);
            _tasks.Add(newTask);
        }

        public void AddTask(string name)
        {
            AddTask(name, DateTime.Now.AddDays(1));
=======
        public StudyTask AddTask(string name)
        {
            return AddTask(name, DateTime.Now.AddDays(1));
        }

        public StudyTask AddTask(string name, DateTime deadline)
        {
            var task = new StudyTask(_nextId++, name, deadline);
            _tasks.Add(task);
            return task;
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
        }

        public void CompleteTask(int id)
        {
<<<<<<< HEAD
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
=======
            var t = _tasks.Find(x => x.Id == id);
            if (t != null && !t.IsCompleted)
            {
                t.Complete();
                foreach (var o in _observers)
                    o.OnTaskCompleted(t);
            }
        }

        public void ClearTasks()
        {
            _tasks.Clear();
            _nextId = 1;
        }

        public List<StudyTask> GetOverdueTasks()
        {
            var now = DateTime.Now;
            return _tasks.FindAll(t => !t.IsCompleted && t.Deadline < now);
        }

        public List<StudyTask> GetAllTasks()
        {
            return new List<StudyTask>(_tasks);
        }

        public StudyTask? GetTaskById(int id)
        {
            return _tasks.Find(t => t.Id == id);
        }

        public void CheckForOverdueTasks()
        {
            var overdue = GetOverdueTasks();
            foreach (var t in overdue)
            {
                foreach (var o in _observers)
                    o.OnTaskOverdue(t);
            }
        }

        public void RegisterObserver(ITaskObserver observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
        }

        public void RemoveObserver(ITaskObserver observer)
        {
            if (_observers.Contains(observer)) _observers.Remove(observer);
        }
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
    }
}