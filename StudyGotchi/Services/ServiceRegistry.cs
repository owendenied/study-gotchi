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
        public static ReminderService ReminderService { get; private set; } = null!;
        public static AudioService AudioService { get; private set; } = null!;

        public static void Initialize()
        {
            PetController = new PetController();
            TaskController = new TaskController();
            TasksViewModel = new TasksViewModel(TaskController);
            SessionController = new SessionController(PetController, TaskController);
            SessionViewModel = new SessionViewModel(SessionController);
            PetSelectionViewModel = new PetSelectionViewModel();
            DashboardViewModel = new DashboardViewModel(PetController, TasksViewModel, SessionViewModel);
            ReminderService = new ReminderService(TaskController);
            AudioService = new AudioService();

            // ── Task completed ──────────────────────────────────────────────
            TaskController.TaskCompleted += _ =>
            {
                PetController.CompleteTask(0);
                DashboardViewModel.Refresh();
                AudioService.PlaySfx("sfx_complete");
            };

            // ── Session started ─────────────────────────────────────────────
            SessionController.SessionStarted += () =>
            {
                ReminderService.Start();
                AudioService.PlayBgm();
                AudioService.PlaySfx("sfx_start");
            };

            // ── Session paused / resumed ────────────────────────────────────
            SessionController.PauseStateChanged += paused =>
            {
                if (paused) AudioService.PauseBgm();
                else AudioService.ResumeBgm();
            };

            // ── Session ended ───────────────────────────────────────────────
            SessionController.SessionEnded += () =>
            {
                ReminderService.Stop();
                AudioService.StopBgm();
                AudioService.PlaySfx("sfx_end");
            };

            // ── Pet levelled up ─────────────────────────────────────────────
            PetController.PetLeveledUp += () =>
            {
                DashboardViewModel.Refresh();
                AudioService.PlaySfx("sfx_levelup");
            };

            // ── Periodic stats refresh (hunger decay etc.) ──────────────────
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
