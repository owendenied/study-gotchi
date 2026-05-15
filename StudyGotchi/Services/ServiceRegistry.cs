using System;
using System.Linq;
using StudyGotchi.Controllers;
using StudyGotchi.Interfaces;
using StudyGotchi.Models;
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
        public static AppStatePersistenceService PersistenceService { get; private set; } = null!;
        public static bool IsFullScreenPreferred { get; set; }
        public static int TotalSessionsCompleted { get; private set; }
        public static int CurrentSessionStreak { get; private set; }
        public static string LastStatusMessage { get; private set; } = string.Empty;

        public static void Initialize()
        {
            PetController = new PetController();
            TaskController = new TaskController();
            SessionController = new SessionController(PetController, TaskController);
            ReminderService = new ReminderService(TaskController);
            AudioService = new AudioService();
            PersistenceService = new AppStatePersistenceService();

            LoadState();

            TasksViewModel = new TasksViewModel(TaskController);
            SessionViewModel = new SessionViewModel(SessionController);
            PetSelectionViewModel = new PetSelectionViewModel();
            DashboardViewModel = new DashboardViewModel(PetController, TasksViewModel, SessionViewModel);
            TaskController.GetTaskManager().RegisterObserver(new OverduePenaltyObserver());

            TaskController.TaskAdded += _ => SaveState();
            TaskController.TasksCleared += SaveState;
            TaskController.TaskCompleted += task =>
            {
                PetController.CompleteTask(task.Id, task.IsCompletedEarly, task.BaseXpReward);
                DashboardViewModel.Refresh();
                AudioService.PlaySfx("sfx_complete");
                SaveState();
            };

            SessionController.SessionStarted += () =>
            {
                TaskController.CheckAndNotifyOverdue();
                ReminderService.Start();
                AudioService.PlayBgm();
                AudioService.PlaySfx("sfx_start");
                SaveState();
            };

            SessionController.PauseStateChanged += paused =>
            {
                if (paused)
                {
                    ReminderService.Pause();
                    AudioService.PauseBgm();
                }
                else
                {
                    TaskController.CheckAndNotifyOverdue();
                    ReminderService.Resume();
                    AudioService.ResumeBgm();
                }
                SaveState();
            };

            SessionController.SessionEnded += () =>
            {
                TotalSessionsCompleted++;
                CurrentSessionStreak++;
                ReminderService.Stop();
                AudioService.StopBgm();
                AudioService.PlaySfx("sfx_end");
                SaveState();
            };

            SessionController.SessionReset += () =>
            {
                ReminderService.Stop();
                AudioService.StopBgm();
                SaveState();
            };

            ReminderService.ReminderTriggered += (taskName, mins) =>
            {
                AudioService.PlaySfx("sfx_reminder");
            };

            PetController.PetLeveledUp += () =>
            {
                DashboardViewModel.Refresh();
                AudioService.PlaySfx("sfx_levelup");
                SaveState();
            };

            SessionController.StatsUpdated += () =>
            {
                TaskController.CheckAndNotifyOverdue();
                DashboardViewModel.Refresh();
                SaveState();
            };

            SyncSessionServicesAfterRestore();
        }

        public static void ResetApp()
        {
            SessionController.ResetSession();
            TaskController.ClearTasks();
            TasksViewModel.Tasks.Clear();
            DashboardViewModel.Refresh();
            SessionViewModel.RefreshSummary();
            SaveState();
            LastStatusMessage = "Save data reset.";
        }

        public static void ResetSaveData()
        {
            PersistenceService.Reset();
            TotalSessionsCompleted = 0;
            CurrentSessionStreak = 0;
            IsFullScreenPreferred = false;
            LastStatusMessage = "Save data reset.";

            PetController.ClearActivePet();
            TaskController.ClearTasks();
            SessionController.ResetSession();
            AudioService.IsMuted = false;
            ReminderService.IsEnabled = true;
            SessionController.HungerDecayRate = 2;
            DashboardViewModel.Refresh();
            SessionViewModel.RefreshSummary();
            SaveState();
        }

        public static void SaveState()
        {
            if (PersistenceService == null) return;

            var state = new AppState
            {
                Pet = PetController.HasActivePet()
                    ? new PetState
                    {
                        PetType = PetController.GetActivePetType(),
                        Name = PetController.GetPetName(),
                        HungerLevel = PetController.GetHungerLevel(),
                        Experience = PetController.GetXp(),
                        Level = PetController.GetLevel()
                    }
                    : null,
                Settings = new SettingsState
                {
                    HungerDecayRate = SessionController.HungerDecayRate,
                    IsMuted = AudioService.IsMuted,
                    RemindersEnabled = ReminderService.IsEnabled,
                    IsFullScreen = IsFullScreenPreferred
                },
                Stats = new StatsState
                {
                    TotalSessionsCompleted = TotalSessionsCompleted,
                    CurrentSessionStreak = CurrentSessionStreak
                },
                Session = new SessionState
                {
                    IsSessionActive = SessionController.IsSessionActive(),
                    IsPaused = SessionController.IsPaused,
                    ElapsedSeconds = SessionController.CurrentElapsedTime.TotalSeconds,
                    TasksCompletedThisSession = SessionController.TasksCompletedThisSession,
                    XpEarnedThisSession = SessionController.XpEarnedThisSession,
                    StartHunger = SessionController.StartHunger,
                    StartLevel = SessionController.StartLevel,
                    EndHunger = SessionController.EndHunger,
                    EndLevel = SessionController.EndLevel,
                    LastSessionDurationSeconds = SessionController.LastSessionDuration.TotalSeconds
                }
            };

            foreach (var task in TaskController.GetAllTasks().Where(t => !t.IsCompleted))
            {
                state.Tasks.Add(new TaskState
                {
                    Id = task.Id,
                    Name = task.Name,
                    Deadline = task.Deadline,
                    IsCompleted = task.IsCompleted,
                    IsCompletedEarly = task.IsCompletedEarly,
                    TaskType = task.TaskType.ToString()
                });
            }

            PersistenceService.Save(state);
            LastStatusMessage = "Progress saved.";
        }

        private static void LoadState()
        {
            var state = PersistenceService.Load();
            LastStatusMessage = "Save loaded.";

            if (state.Pet != null)
            {
                PetController.RestoreActivePet(
                    state.Pet.PetType,
                    state.Pet.Name,
                    state.Pet.HungerLevel,
                    state.Pet.Experience,
                    state.Pet.Level);
            }

            TaskController.GetTaskManager().ReplaceTasks(state.Tasks.ConvertAll(t =>
                new StudyTask(
                    t.Id,
                    t.Name,
                    t.Deadline,
                    Enum.TryParse<StudyTaskType>(t.TaskType, out var taskType) ? taskType : StudyTaskType.Activity,
                    t.IsCompleted,
                    t.IsCompletedEarly)));

            SessionController.HungerDecayRate = Math.Clamp(state.Settings.HungerDecayRate, 1, 3);
            ReminderService.IsEnabled = state.Settings.RemindersEnabled;
            AudioService.IsMuted = state.Settings.IsMuted;
            IsFullScreenPreferred = state.Settings.IsFullScreen;
            TotalSessionsCompleted = Math.Max(0, state.Stats.TotalSessionsCompleted);
            CurrentSessionStreak = Math.Max(0, state.Stats.CurrentSessionStreak);

            SessionController.RestoreSession(
                state.Session.IsSessionActive,
                state.Session.IsPaused,
                TimeSpan.FromSeconds(Math.Max(0, state.Session.ElapsedSeconds)),
                Math.Max(0, state.Session.TasksCompletedThisSession),
                Math.Max(0, state.Session.XpEarnedThisSession),
                state.Session.StartHunger,
                state.Session.StartLevel,
                state.Session.EndHunger,
                state.Session.EndLevel,
                TimeSpan.FromSeconds(Math.Max(0, state.Session.LastSessionDurationSeconds)));
        }

        private static void SyncSessionServicesAfterRestore()
        {
            if (!SessionController.IsSessionActive())
            {
                ReminderService.Stop();
                AudioService.StopBgm();
                return;
            }

            if (SessionController.IsPaused)
            {
                ReminderService.Pause();
                AudioService.PauseBgm();
                return;
            }

            TaskController.CheckAndNotifyOverdue();
            ReminderService.Start();
            AudioService.PlayBgm();
        }

        private sealed class OverduePenaltyObserver : ITaskObserver
        {
            public void OnTaskCompleted(StudyTask task)
            {
            }

            public void OnTaskOverdue(StudyTask task)
            {
                PetController.ApplyOverduePenalty();
                DashboardViewModel.Refresh();
                SaveState();
            }
        }
    }
}
