using System;
using System.Collections.Generic;
using System.Linq;
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

<<<<<<< HEAD
        // Add a task without a specific deadline (default to end of day)
        public void AddTask(string name)
        {
            _taskManager.AddTask(new StudyTask(name, DateTime.Today.AddDays(1)));
        }

        // Add a task with a specific deadline
        public void AddTask(string name, DateTime deadline)
        {
            _taskManager.AddTask(new StudyTask(name, deadline));
=======
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
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
        }

        public void CompleteTask(int id)
        {
            var task = _taskManager.GetTaskById(id);
<<<<<<< HEAD
            if (task != null)
            {
                // This triggers the logic inside StudyTask to check if it's "Early"
                object value = task.Complete();

                // Note: Your UI code will then call pet.OnTaskCompleted(task)
                // to update the Hunger and XP based on the task status.
            }
=======
            _taskManager.CompleteTask(id);
            if (task != null) TaskCompleted?.Invoke(task);
        }

        public void ClearTasks()
        {
            _taskManager.ClearTasks();
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
        }

        public IReadOnlyList<StudyTask> GetAllTasks()
        {
<<<<<<< HEAD
            return _taskManager.Tasks;
=======
            return _taskManager.GetAllTasks().AsReadOnly();
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
        }

        public List<StudyTask> GetOverdueTasks()
        {
<<<<<<< HEAD
            return _taskManager.Tasks.Where(t => t.IsOverdue()).ToList();
=======
            return _taskManager.GetOverdueTasks();
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
        }

        public void CheckAndNotifyOverdue()
        {
<<<<<<< HEAD
            var overdue = GetOverdueTasks();
            foreach (var task in overdue)
            {
                // Logic to trigger a notification to the user
                Console.WriteLine($"ALERT: {task.TaskName} is overdue!");
            }
=======
            _taskManager.CheckForOverdueTasks();
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
        }

        public bool HasTasks()
        {
<<<<<<< HEAD
            return _taskManager.Tasks.Any();
=======
            return _taskManager.GetAllTasks().Count > 0;
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
        }

        public TaskManager GetTaskManager() { return _taskManager; }
    }
}