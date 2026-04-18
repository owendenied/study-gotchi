using System;
using System.Collections.Generic;
using StudyGotchi.Models;

namespace StudyGotchi.Controllers
{
    public class TaskController
    {
        private TaskManager _taskManager;

        public TaskController()
        {
            _taskManager = new TaskManager();
        }

        public void AddTask(string name) { throw new NotImplementedException(); }
        public void AddTask(string name, DateTime deadline) { throw new NotImplementedException(); }
        public void CompleteTask(int id) { throw new NotImplementedException(); }
        public IReadOnlyList<StudyTask> GetAllTasks() { throw new NotImplementedException(); }
        public List<StudyTask> GetOverdueTasks() { throw new NotImplementedException(); }
        public void CheckAndNotifyOverdue() { throw new NotImplementedException(); }
        public bool HasTasks() { throw new NotImplementedException(); }
        public TaskManager GetTaskManager() { return _taskManager; }
    }
}