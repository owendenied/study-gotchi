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
        private static readonly TimeSpan RecentReminderWindow = TimeSpan.FromSeconds(8);
        private readonly TaskController _taskController;
        private readonly DispatcherTimer _timer;
        private bool _isEnabled = true;

        // Tracks which (taskId, thresholdMinutes) combos have already fired
        private readonly HashSet<(int taskId, int threshold)> _firedReminders = new();

        // Thresholds in minutes
        private static readonly int[] ReminderThresholds = { 10, 5, 1 };

        /// <summary>
        /// Raised when a task is approaching its deadline.
        /// Args: task name, minutes remaining.
        /// </summary>
        public event Action<string, int>? ReminderTriggered;

        public string? LastReminderTaskName { get; private set; }
        public int LastReminderMinutesLeft { get; private set; }
        public DateTime LastReminderTime { get; private set; } = DateTime.MinValue;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                if (!_isEnabled) Stop();
            }
        }

        public bool IsRunning => _timer.IsEnabled;

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
            if (!IsEnabled) return;

            _firedReminders.Clear();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _firedReminders.Clear();
            ClearLastReminder();
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public void Resume()
        {
            if (!IsEnabled) return;
            _timer.Start();
        }

        public void CheckNow(DateTime? now = null)
        {
            CheckReminders(now ?? DateTime.Now);
        }

        public bool TryGetRecentReminder(out string taskName, out int minutesLeft, DateTime? now = null)
        {
            taskName = LastReminderTaskName ?? string.Empty;
            minutesLeft = LastReminderMinutesLeft;

            if (LastReminderTaskName == null) return false;

            var referenceTime = now ?? DateTime.Now;
            return referenceTime - LastReminderTime <= RecentReminderWindow;
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            CheckReminders(DateTime.Now);
        }

        private void CheckReminders(DateTime now)
        {
            var tasks = _taskController.GetAllTasks();

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
                            TriggerReminder(task.Name, minutesLeft, now);
                        }
                    }
                }
            }
        }

        private void TriggerReminder(string taskName, int minutesLeft, DateTime firedAt)
        {
            LastReminderTaskName = taskName;
            LastReminderMinutesLeft = minutesLeft;
            LastReminderTime = firedAt;
            ReminderTriggered?.Invoke(taskName, minutesLeft);
        }

        private void ClearLastReminder()
        {
            LastReminderTaskName = null;
            LastReminderMinutesLeft = 0;
            LastReminderTime = DateTime.MinValue;
        }
    }
}
