// TaskManager.cs

using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    // Holds the list of tasks and tells the pet when things happen.
    public class TaskManager
    {
        private readonly List<StudyTask> _tasks;
        private readonly List<ITaskObserver> _observers;
        private int _nextId = 1;

        public TaskManager()
        {
            _tasks = new List<StudyTask>();
            _observers = new List<ITaskObserver>();
        }

        // Exposing the tasks as read-only so other classes don't mess with the list directly
        public IReadOnlyList<StudyTask> Tasks => _tasks.AsReadOnly();

        // Adds the pet to our notification list
        public void RegisterObserver(ITaskObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void RemoveObserver(ITaskObserver observer)
        {
            _observers.Remove(observer);
        }

        // Add a task with a default 24h deadline
        public void AddTask(string name)
        {
            AddTask(name, DateTime.Now.AddHours(24));
        }

        // Add a task with a specific deadline
        public void AddTask(string name, DateTime deadline)
        {
            var task = new StudyTask(_nextId++, name, deadline);
            _tasks.Add(task);
        }

        // Mark done and tell the pet to give XP
        public void CompleteTask(int id)
        {
            StudyTask? task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null || task.IsCompleted) return;

            task.Complete();
            NotifyTaskCompleted(task);
        }

        // Grab everything that's late
        public List<StudyTask> GetOverdueTasks()
        {
            return _tasks.Where(t => t.IsOverdue()).ToList();
        }

        // Tell the pet we finished a task
        private void NotifyTaskCompleted(StudyTask task)
        {
            foreach (var observer in _observers)
                observer.OnTaskCompleted(task);
        }

        // Tell the pet we missed a task
        private void NotifyTaskOverdue(StudyTask task)
        {
            foreach (var observer in _observers)
                observer.OnTaskOverdue(task);
        }

        // Called by the timer every minute to penalize for late stuff
        public void CheckForOverdueTasks()
        {
            foreach (var task in GetOverdueTasks())
                NotifyTaskOverdue(task);
        }
    }
}