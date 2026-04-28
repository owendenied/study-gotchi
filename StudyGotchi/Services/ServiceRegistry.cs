using StudyGotchi.Controllers;
using StudyGotchi.ViewModels;

namespace StudyGotchi.Services
{
    public static class ServiceRegistry
    {
        public static TaskController TaskController { get; private set; } = null!;
        public static PetController PetController { get; private set; } = null!;
        public static TasksViewModel TasksViewModel { get; private set; } = null!;
        public static DashboardViewModel DashboardViewModel { get; private set; } = null!;
        public static SessionViewModel SessionViewModel { get; private set; } = null!;
        public static PetSelectionViewModel PetSelectionViewModel { get; private set; } = null!;
        public static SessionController SessionController { get; private set; } = null!;

        public static void Initialize()
        {
            PetController = new PetController();
            TaskController = new TaskController();
            TasksViewModel = new TasksViewModel(TaskController);
            SessionController = new SessionController(PetController, TaskController);
            SessionViewModel = new SessionViewModel(SessionController);
            PetSelectionViewModel = new PetSelectionViewModel();
            DashboardViewModel = new DashboardViewModel(PetController, TasksViewModel, SessionViewModel);

            // Link events
            TaskController.TaskCompleted += _ => 
            {
                PetController.CompleteTask(0);
                DashboardViewModel.Refresh();
            };
            
            SessionController.StatsUpdated += () => DashboardViewModel.Refresh();
        }

        public static void ResetApp()
        {
            TaskController.ClearTasks();
            TasksViewModel.Tasks.Clear();
            DashboardViewModel.Refresh();
        }
    }
}
