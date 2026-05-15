using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using StudyGotchi.Controllers;
using StudyGotchi.Models;

namespace StudyGotchi.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private readonly TaskController _taskController;
        public ObservableCollection<StudyTask> Tasks { get; } = new ObservableCollection<StudyTask>();

        public ICommand AddTaskCommand { get; }

        public TasksViewModel(TaskController taskController)
        {
            _taskController = taskController;
            AddTaskCommand = new RelayCommand(_ => AddTask("New Task"));

            // load existing
            foreach (var t in _taskController.GetTaskManager().GetAllTasks())
                Tasks.Add(t);

            // subscribe to additions
            _taskController.TaskAdded += t =>
            {
                RunOnUiThread(() => Tasks.Add(t));
            };

            // When a task is completed: wait for the fade animation (0.6s) then remove it
            _taskController.TaskCompleted += t =>
            {
                var timer = new DispatcherTimer { Interval = System.TimeSpan.FromSeconds(0.6) };
                timer.Tick += (s, e) =>
                {
                    timer.Stop();
                    RunOnUiThread(() => Tasks.Remove(t));
                };
                timer.Start();
            };

            // When tasks are cleared, clear the UI collection
            _taskController.TasksCleared += () =>
            {
                RunOnUiThread(Tasks.Clear);
            };
        }

        public void AddTask(string name)
        {
            _taskController.AddTask(name);
        }

        private static void RunOnUiThread(Action action)
        {
            var dispatcher = System.Windows.Application.Current?.Dispatcher;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                action();
                return;
            }

            dispatcher.Invoke(action);
        }
    }
}
