using System;
using System.Collections.Generic;
using System.Linq;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    public class TaskManager
    {
        private List<StudyTask> _tasks;
        private readonly List<ITaskObserver> _observers = new();
        private readonly HashSet<int> _overduePenaltyTaskIds = new();
        private int _nextId = 1;

        public List<StudyTask> Tasks => _tasks;

        public TaskManager()
        {
            _tasks = new List<StudyTask>();
        }

        public StudyTask AddTask(string name)
        {
            return AddTask(name, DateTime.Now.AddDays(1), StudyTaskType.Activity);
        }

        public StudyTask AddTask(string name, DateTime deadline)
        {
            return AddTask(name, deadline, StudyTaskType.Activity);
        }

        public StudyTask AddTask(string name, DateTime deadline, StudyTaskType taskType)
        {
            var task = new StudyTask(_nextId++, name, deadline, taskType);
            _tasks.Add(task);
            return task;
        }

        public bool CompleteTask(int id)
        {
            var t = _tasks.Find(x => x.Id == id);
            if (t != null && !t.IsCompleted)
            {
                if (!t.Complete()) return false;

                foreach (var o in _observers)
                    o.OnTaskCompleted(t);

                _tasks.Remove(t);
                _overduePenaltyTaskIds.Remove(t.Id);
                return true;
            }

            return false;
        }

        public void ClearTasks()
        {
            _tasks.Clear();
            _overduePenaltyTaskIds.Clear();
            _nextId = 1;
        }

        public void ReplaceTasks(IEnumerable<StudyTask> tasks)
        {
            _tasks = tasks.Where(t => !t.IsCompleted).OrderBy(t => t.Id).ToList();
            _overduePenaltyTaskIds.Clear();
            _nextId = _tasks.Count == 0 ? 1 : _tasks.Max(t => t.Id) + 1;
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
                if (!_overduePenaltyTaskIds.Add(t.Id)) continue;

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
    }
}
