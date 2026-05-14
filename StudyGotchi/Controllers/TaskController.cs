using System;
using System.Collections.Generic;
using System.Linq;
using StudyGotchi.Models;

namespace StudyGotchi.Controllers
{
    public class TaskController
    {
        public const int MaxTaskNameLength = 120;
        private TaskManager _taskManager;

        public event Action<StudyTask>? TaskAdded;
        public event Action<StudyTask>? TaskCompleted;
        public event Action? TasksCleared;

        public TaskController()
        {
            _taskManager = new TaskManager();
        }

        public StudyTask AddTask(string name)
        {
            var t = _taskManager.AddTask(NormalizeTaskName(name));
            TaskAdded?.Invoke(t);
            return t;
        }

        public StudyTask AddTask(string name, DateTime deadline)
        {
            return AddTask(name, deadline, StudyTaskType.Activity);
        }

        public StudyTask AddTask(string name, DateTime deadline, StudyTaskType taskType)
        {
            var t = _taskManager.AddTask(NormalizeTaskName(name), deadline, taskType);
            TaskAdded?.Invoke(t);
            return t;
        }

        public void CompleteTask(int id)
        {
            var task = _taskManager.GetTaskById(id);
            if (task != null && _taskManager.CompleteTask(id))
            {
                TaskCompleted?.Invoke(task);
            }
        }

        public void ClearTasks()
        {
            _taskManager.ClearTasks();
            TasksCleared?.Invoke();
        }

        public IReadOnlyList<StudyTask> GetAllTasks()
        {
            return _taskManager.GetAllTasks().AsReadOnly();
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
            return _taskManager.GetAllTasks().Count > 0;
        }

        public TaskManager GetTaskManager() { return _taskManager; }

        public void ReplaceTasks(IEnumerable<StudyTask> tasks)
        {
            _taskManager.ReplaceTasks(tasks);
            TasksCleared?.Invoke();

            foreach (var task in _taskManager.GetAllTasks())
            {
                if (!task.IsCompleted) TaskAdded?.Invoke(task);
            }
        }

        private static string NormalizeTaskName(string? name)
        {
            var normalized = string.IsNullOrWhiteSpace(name) ? "Untitled Task" : name.Trim();
            return normalized.Length <= MaxTaskNameLength
                ? normalized
                : normalized[..MaxTaskNameLength];
        }
    }
}
