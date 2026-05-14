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
