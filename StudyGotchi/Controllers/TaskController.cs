using System;
using System.Collections.Generic;
using StudyGotchi.Models;

namespace StudyGotchi.Controllers
{
    public class TaskController
    {
        private TaskManager _taskManager;

        public event Action<StudyTask>? TaskAdded;
        public event Action<StudyTask>? TaskCompleted;

        public TaskController()
        {
            _taskManager = new TaskManager();
        }

        public StudyTask AddTask(string name)
        {
            var t = _taskManager.AddTask(name);
            TaskAdded?.Invoke(t);
            return t;
        }

        public StudyTask AddTask(string name, DateTime deadline)
        {
            var t = _taskManager.AddTask(name, deadline);
            TaskAdded?.Invoke(t);
            return t;
        }

        public void CompleteTask(int id)
        {
            _taskManager.CompleteTask(id);
            var t = _taskManager.GetOverdueTasks().Find(x => x.Id == id);
            // best effort: raise completed event with task if found in collection
            TaskCompleted?.Invoke(t);
        }

        public IReadOnlyList<StudyTask> GetAllTasks()
        {
            return _taskManager.GetOverdueTasks().AsReadOnly();
        }

        public List<StudyTask> GetOverdueTasks()
        {
            return _taskManager.GetOverdueTasks();
        }

        public void CheckAndNotifyOverdue()
        {
            _taskManager.CheckForOverdueTasks();
        }

        public bool HasTasks()
        {
            // naive implementation
            return _taskManager.GetOverdueTasks().Count > 0;
        }
        public TaskManager GetTaskManager() { return _taskManager; }
    }
}