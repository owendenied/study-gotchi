using System.Collections.ObjectModel;
using System.Windows.Input;
using StudyGotchi.Controllers;
using StudyGotchi.Models;

namespace StudyGotchi.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private TaskController _taskController;
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
                // ensure UI thread
                System.Windows.Application.Current?.Dispatcher?.Invoke(() => Tasks.Add(t));
            };
        }

        public void AddTask(string name)
        {
            _taskController.AddTask(name);
        }
    }
}
