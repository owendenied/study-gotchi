using System;
using System.Collections.Generic;
using System.Windows.Threading;
using StudyGotchi.Controllers;

namespace StudyGotchi.Services
{
    /// <summary>
    /// Fires deadline-approach reminders at 10 min, 5 min, and 1 min remaining.
    /// Each (taskId, threshold) pair fires at most once per session.
    /// </summary>
    public class ReminderService
    {
        private readonly TaskController _taskController;
        private readonly DispatcherTimer _timer;

        // Tracks which (taskId, thresholdMinutes) combos have already fired
        private readonly HashSet<(int taskId, int threshold)> _firedReminders = new();

        // Thresholds in minutes
        private static readonly int[] ReminderThresholds = { 10, 5, 1 };

        /// <summary>
        /// Raised when a task is approaching its deadline.
        /// Args: task name, minutes remaining.
        /// </summary>
        public event Action<string, int>? ReminderTriggered;

        public ReminderService(TaskController taskController)
        {
            _taskController = taskController;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            _timer.Tick += OnTimerTick;
        }

        public void Start()
        {
            _firedReminders.Clear();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _firedReminders.Clear();
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            var tasks = _taskController.GetAllTasks();
            var now = DateTime.Now;

            foreach (var task in tasks)
            {
                if (task.IsCompleted) continue;

                var remaining = task.Deadline - now;
                int minutesLeft = (int)Math.Ceiling(remaining.TotalMinutes);

                foreach (int threshold in ReminderThresholds)
                {
                    // Fire if we just crossed the threshold window (within 30s tolerance)
                    if (minutesLeft <= threshold && minutesLeft > threshold - 1)
                    {
                        var key = (task.Id, threshold);
                        if (!_firedReminders.Contains(key))
                        {
                            _firedReminders.Add(key);
                            ReminderTriggered?.Invoke(task.Name, minutesLeft);
                        }
                    }
                }
            }
        }
    }
}
