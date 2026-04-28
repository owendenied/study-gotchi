using System;
using System.Collections.Generic;
using System.Linq;
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

        // Add a task without a specific deadline (default to end of day)
        public void AddTask(string name)
        {
            _taskManager.AddTask(new StudyTask(name, DateTime.Today.AddDays(1)));
        }

        // Add a task with a specific deadline
        public void AddTask(string name, DateTime deadline)
        {
            _taskManager.AddTask(new StudyTask(name, deadline));
        }

        public void CompleteTask(int id)
        {
            var task = _taskManager.GetTaskById(id);
            if (task != null)
            {
                // This triggers the logic inside StudyTask to check if it's "Early"
                object value = task.Complete();

                // Note: Your UI code will then call pet.OnTaskCompleted(task)
                // to update the Hunger and XP based on the task status.
            }
        }

        public IReadOnlyList<StudyTask> GetAllTasks()
        {
            return _taskManager.Tasks;
        }

        public List<StudyTask> GetOverdueTasks()
        {
            return _taskManager.Tasks.Where(t => t.IsOverdue()).ToList();
        }

        public void CheckAndNotifyOverdue()
        {
            var overdue = GetOverdueTasks();
            foreach (var task in overdue)
            {
                // Logic to trigger a notification to the user
                Console.WriteLine($"ALERT: {task.TaskName} is overdue!");
            }
        }

        public bool HasTasks()
        {
            return _taskManager.Tasks.Any();
        }

        public TaskManager GetTaskManager() { return _taskManager; }
    }
}