using System.IO;
using StudyGotchi.Controllers;
using StudyGotchi.Interfaces;
using StudyGotchi.Models;
using StudyGotchi.Services;

namespace StudyGotchi.Tests;

public class CoreGameLogicTests
{
    [Fact]
    public void CompletingEarlyTaskAppliesEarlyPetReward()
    {
        var petController = new PetController();
        var taskController = new TaskController();
        petController.SetActivePet(0, "Tester");

        taskController.TaskCompleted += task =>
            petController.CompleteTask(task.Id, task.IsCompletedEarly);

        var task = taskController.AddTask("Finish notes", DateTime.Now.AddHours(1));
        taskController.CompleteTask(task.Id);

        Assert.Equal(4, petController.GetLevel());
        Assert.Equal(0, petController.GetXp());
        Assert.Equal(100, petController.GetHungerLevel());
    }

    [Fact]
    public void CompletingSameTaskTwiceOnlyRaisesCompletionOnce()
    {
        var taskController = new TaskController();
        var completionEvents = 0;
        taskController.TaskCompleted += _ => completionEvents++;

        var task = taskController.AddTask("One shot", DateTime.Now.AddHours(1));
        taskController.CompleteTask(task.Id);
        taskController.CompleteTask(task.Id);

        Assert.Equal(1, completionEvents);
    }

    [Fact]
    public void CompletingTaskRemovesItFromActiveTasks()
    {
        var taskController = new TaskController();

        var task = taskController.AddTask("Remove after reward", DateTime.Now.AddHours(1));
        taskController.CompleteTask(task.Id);

        Assert.Empty(taskController.GetAllTasks());
    }

    [Fact]
    public void OverdueTaskPenaltyFiresOncePerTask()
    {
        var manager = new TaskManager();
        var observer = new CountingTaskObserver();
        manager.RegisterObserver(observer);

        manager.AddTask("Past due", DateTime.Now.AddMinutes(-5));
        manager.CheckForOverdueTasks();
        manager.CheckForOverdueTasks();

        Assert.Equal(1, observer.OverdueCount);
    }

    [Fact]
    public void ReplacingTasksPreservesNextTaskId()
    {
        var manager = new TaskManager();
        manager.ReplaceTasks(new[]
        {
            new StudyTask(3, "Saved task", DateTime.Now.AddHours(1), StudyTaskType.Activity, false, false)
        });

        var added = manager.AddTask("Next task", DateTime.Now.AddHours(2));

        Assert.Equal(4, added.Id);
    }

    [Fact]
    public void ReplacingTasksDropsCompletedSavedTasks()
    {
        var manager = new TaskManager();
        manager.ReplaceTasks(new[]
        {
            new StudyTask(1, "Done task", DateTime.Now.AddHours(1), StudyTaskType.Activity, true, true),
            new StudyTask(2, "Active task", DateTime.Now.AddHours(1), StudyTaskType.Project, false, false)
        });

        var active = manager.GetAllTasks();

        Assert.Single(active);
        Assert.Equal("Active task", active[0].Name);
    }

    [Fact]
    public void TaskTypeRewardsFollowExpectedHierarchy()
    {
        Assert.True(StudyTaskType.Project.GetBaseXpReward() > StudyTaskType.Activity.GetBaseXpReward());
        Assert.True(StudyTaskType.Activity.GetBaseXpReward() > StudyTaskType.WrittenWork.GetBaseXpReward());
    }

    [Fact]
    public void EarlyCompletionDoublesTaskTypeReward()
    {
        var pet = new PetA();

        var awarded = pet.CalculateTaskXp(true, StudyTaskType.Project.GetBaseXpReward());

        Assert.Equal(500, awarded);
    }

    [Fact]
    public void StarvationPenaltyHalvesTaskTypeReward()
    {
        var pet = new PetA { HungerLevel = 10 };

        var awarded = pet.CalculateTaskXp(false, StudyTaskType.Project.GetBaseXpReward());

        Assert.Equal(125, awarded);
    }

    [Fact]
    public void SessionSummaryTracksCompletedTaskStats()
    {
        var petController = new PetController();
        var taskController = new TaskController();
        petController.SetActivePet(0, "Tester");
        var sessionController = new SessionController(petController, taskController);

        taskController.TaskCompleted += task =>
            petController.CompleteTask(task.Id, task.IsCompletedEarly, task.BaseXpReward);

        sessionController.StartSession();
        var task = taskController.AddTask("Project", DateTime.Now.AddHours(1), StudyTaskType.Project);
        taskController.CompleteTask(task.Id);
        sessionController.EndSession();

        Assert.Equal(1, sessionController.TasksCompletedThisSession);
        Assert.Equal(500, sessionController.XpEarnedThisSession);
        Assert.True(sessionController.LevelChange > 0);
    }

    [Fact]
    public void ResetSessionDoesNotRaiseCompletedSessionEvent()
    {
        var petController = new PetController();
        var taskController = new TaskController();
        var sessionController = new SessionController(petController, taskController);
        var completedSessions = 0;
        var resetSessions = 0;
        sessionController.SessionEnded += () => completedSessions++;
        sessionController.SessionReset += () => resetSessions++;

        sessionController.StartSession();
        sessionController.ResetSession();

        Assert.Equal(0, completedSessions);
        Assert.Equal(1, resetSessions);
        Assert.False(sessionController.IsSessionActive());
        Assert.Equal(TimeSpan.Zero, sessionController.LastSessionDuration);
    }

    [Fact]
    public void PausedRestoredSessionRemainsActiveAndPaused()
    {
        var petController = new PetController();
        var taskController = new TaskController();
        var sessionController = new SessionController(petController, taskController);
        var pauseEvents = 0;
        sessionController.PauseStateChanged += paused =>
        {
            if (paused) pauseEvents++;
        };

        sessionController.RestoreSession(
            isSessionActive: true,
            isPaused: true,
            elapsedTime: TimeSpan.FromMinutes(12),
            tasksCompleted: 2,
            xpEarned: 300,
            startHunger: 90,
            startLevel: 1,
            endHunger: 90,
            endLevel: 1,
            lastSessionDuration: TimeSpan.Zero);

        Assert.True(sessionController.IsSessionActive());
        Assert.True(sessionController.IsPaused);
        Assert.Equal(TimeSpan.FromMinutes(12), sessionController.CurrentElapsedTime);
        Assert.Equal(2, sessionController.TasksCompletedThisSession);
        Assert.Equal(300, sessionController.XpEarnedThisSession);
        Assert.Equal(1, pauseEvents);
    }

    [Fact]
    public void ReminderThresholdsFireOnceUntilNewSessionStarts()
    {
        var taskController = new TaskController();
        var reminderService = new ReminderService(taskController);
        var now = DateTime.Now;
        taskController.AddTask("Due soon", now.AddMinutes(10));
        var fired = 0;
        reminderService.ReminderTriggered += (_, minutesLeft) =>
        {
            fired++;
            Assert.Equal(10, minutesLeft);
        };

        reminderService.Start();
        reminderService.CheckNow(now);
        reminderService.Pause();
        reminderService.Resume();
        reminderService.CheckNow(now);

        Assert.Equal(1, fired);

        reminderService.Stop();
        reminderService.Start();
        reminderService.CheckNow(now);

        Assert.Equal(2, fired);
    }

    [Fact]
    public void ReminderServiceKeepsRecentReminderForLateWidgetSubscribers()
    {
        var taskController = new TaskController();
        var reminderService = new ReminderService(taskController);
        var now = DateTime.Now;
        taskController.AddTask("Widget reminder", now.AddMinutes(5));

        reminderService.Start();
        reminderService.CheckNow(now);

        Assert.True(reminderService.TryGetRecentReminder(out var taskName, out var minutesLeft, now.AddSeconds(7)));
        Assert.Equal("Widget reminder", taskName);
        Assert.Equal(5, minutesLeft);

        Assert.False(reminderService.TryGetRecentReminder(out _, out _, now.AddSeconds(9)));
    }

    [Fact]
    public void StoppingReminderServiceClearsRecentReminder()
    {
        var taskController = new TaskController();
        var reminderService = new ReminderService(taskController);
        var now = DateTime.Now;
        taskController.AddTask("Clear me", now.AddMinutes(1));

        reminderService.Start();
        reminderService.CheckNow(now);
        reminderService.Stop();

        Assert.False(reminderService.TryGetRecentReminder(out _, out _, now.AddSeconds(1)));
    }

    [Fact]
    public void TaskControllerNormalizesUnsafeTaskNames()
    {
        var taskController = new TaskController();
        var longName = new string('A', TaskController.MaxTaskNameLength + 10);

        var blank = taskController.AddTask("   ");
        var trimmed = taskController.AddTask("  Read chapter  ");
        var capped = taskController.AddTask(longName);

        Assert.Equal("Untitled Task", blank.Name);
        Assert.Equal("Read chapter", trimmed.Name);
        Assert.Equal(TaskController.MaxTaskNameLength, capped.Name.Length);
    }

    [Fact]
    public void PersistenceRoundTripKeepsTaskTypeAndStats()
    {
        var savePath = Path.Combine(Path.GetTempPath(), $"studygotchi-{Guid.NewGuid()}.json");
        var persistence = new AppStatePersistenceService(savePath);

        persistence.Save(new AppState
        {
            Tasks =
            {
                new TaskState
                {
                    Id = 1,
                    Name = "Poster",
                    Deadline = DateTime.Now.AddHours(1),
                    TaskType = StudyTaskType.Project.ToString()
                }
            },
            Stats = new StatsState
            {
                TotalSessionsCompleted = 2,
                CurrentSessionStreak = 2
            }
        });

        var loaded = persistence.Load();

        Assert.Equal(StudyTaskType.Project.ToString(), loaded.Tasks[0].TaskType);
        Assert.Equal(2, loaded.Stats.TotalSessionsCompleted);
        persistence.Reset();
    }

    [Fact]
    public void CorruptedSaveIsBackedUpAndFreshStateLoads()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"studygotchi-{Guid.NewGuid()}");
        var savePath = Path.Combine(directory, "state.json");
        Directory.CreateDirectory(directory);
        File.WriteAllText(savePath, "{not json");

        var persistence = new AppStatePersistenceService(savePath);
        var loaded = persistence.Load();

        Assert.Empty(loaded.Tasks);
        Assert.False(File.Exists(savePath));
        Assert.Single(Directory.GetFiles(directory, "state.json.*.bak"));
    }

    [Fact]
    public void PersistenceNormalizesInvalidLoadedState()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"studygotchi-{Guid.NewGuid()}");
        var savePath = Path.Combine(directory, "state.json");
        Directory.CreateDirectory(directory);
        File.WriteAllText(savePath, """
        {
          "Pet": {
            "PetType": "PetA",
            "Name": "This pet name is much too long for the UI",
            "HungerLevel": 500,
            "Experience": -20,
            "Level": 99
          },
          "Tasks": [
            {
              "Id": 1,
              "Name": "",
              "Deadline": "2030-01-01T12:00:00",
              "IsCompleted": false,
              "IsCompletedEarly": false,
              "TaskType": "Activity"
            }
          ],
          "Settings": {
            "HungerDecayRate": 99,
            "IsMuted": false,
            "RemindersEnabled": true,
            "IsFullScreen": false
          },
          "Stats": {
            "TotalSessionsCompleted": -4,
            "CurrentSessionStreak": -2
          },
          "Session": {
            "IsSessionActive": true,
            "IsPaused": true,
            "ElapsedSeconds": -10,
            "TasksCompletedThisSession": -1,
            "XpEarnedThisSession": -50,
            "StartHunger": 100,
            "StartLevel": 1,
            "EndHunger": 100,
            "EndLevel": 1,
            "LastSessionDurationSeconds": -3
          }
        }
        """);

        var loaded = new AppStatePersistenceService(savePath).Load();

        Assert.Equal(PetController.MaxPetNameLength, loaded.Pet!.Name.Length);
        Assert.Equal(100, loaded.Pet.HungerLevel);
        Assert.Equal(0, loaded.Pet.Experience);
        Assert.Equal(30, loaded.Pet.Level);
        Assert.Equal("Untitled Task", loaded.Tasks[0].Name);
        Assert.Equal(3, loaded.Settings.HungerDecayRate);
        Assert.Equal(0, loaded.Stats.TotalSessionsCompleted);
        Assert.Equal(0, loaded.Stats.CurrentSessionStreak);
        Assert.Equal(0, loaded.Session.ElapsedSeconds);
        Assert.Equal(0, loaded.Session.TasksCompletedThisSession);
        Assert.Equal(0, loaded.Session.XpEarnedThisSession);
        Assert.Equal(0, loaded.Session.LastSessionDurationSeconds);
    }

    private sealed class CountingTaskObserver : ITaskObserver
    {
        public int OverdueCount { get; private set; }

        public void OnTaskCompleted(StudyTask task)
        {
        }

        public void OnTaskOverdue(StudyTask task)
        {
            OverdueCount++;
        }
    }
}
