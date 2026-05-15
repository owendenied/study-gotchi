using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace StudyGotchi.Services
{
    public class AppState
    {
        public PetState? Pet { get; set; }
        public List<TaskState> Tasks { get; set; } = new();
        public SettingsState Settings { get; set; } = new();
        public StatsState Stats { get; set; } = new();
        public SessionState Session { get; set; } = new();
    }

    public class PetState
    {
        public string PetType { get; set; } = "PetA";
        public string Name { get; set; } = "Buddy";
        public int HungerLevel { get; set; } = 100;
        public int Experience { get; set; }
        public int Level { get; set; } = 1;
    }

    public class TaskState
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCompletedEarly { get; set; }
        public string TaskType { get; set; } = "Activity";
    }

    public class SettingsState
    {
        public int HungerDecayRate { get; set; } = 2;
        public bool IsMuted { get; set; }
        public bool RemindersEnabled { get; set; } = true;
        public bool IsFullScreen { get; set; }
    }

    public class StatsState
    {
        public int TotalSessionsCompleted { get; set; }
        public int CurrentSessionStreak { get; set; }
    }

    public class SessionState
    {
        public bool IsSessionActive { get; set; }
        public bool IsPaused { get; set; }
        public double ElapsedSeconds { get; set; }
        public int TasksCompletedThisSession { get; set; }
        public int XpEarnedThisSession { get; set; }
        public int StartHunger { get; set; }
        public int StartLevel { get; set; }
        public int EndHunger { get; set; }
        public int EndLevel { get; set; }
        public double LastSessionDurationSeconds { get; set; }
    }

    public class AppStatePersistenceService
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public string SavePath { get; }

        public AppStatePersistenceService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var saveDirectory = Path.Combine(appData, "StudyGotchi");
            Directory.CreateDirectory(saveDirectory);
            SavePath = Path.Combine(saveDirectory, "state.json");
        }

        public AppStatePersistenceService(string savePath)
        {
            var saveDirectory = Path.GetDirectoryName(savePath);
            if (!string.IsNullOrEmpty(saveDirectory)) Directory.CreateDirectory(saveDirectory);
            SavePath = savePath;
        }

        public AppState Load()
        {
            if (!File.Exists(SavePath)) return new AppState();

            try
            {
                var json = File.ReadAllText(SavePath);
                return NormalizeState(JsonSerializer.Deserialize<AppState>(json, JsonOptions) ?? new AppState());
            }
            catch
            {
                BackupCorruptedSave();
                return new AppState();
            }
        }

        public void Save(AppState state)
        {
            var json = JsonSerializer.Serialize(state, JsonOptions);
            var directory = Path.GetDirectoryName(SavePath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

            var tempPath = $"{SavePath}.{Guid.NewGuid():N}.tmp";
            File.WriteAllText(tempPath, json);

            if (File.Exists(SavePath))
            {
                File.Replace(tempPath, SavePath, null);
            }
            else
            {
                File.Move(tempPath, SavePath);
            }
        }

        public void Reset()
        {
            if (File.Exists(SavePath)) File.Delete(SavePath);
        }

        private void BackupCorruptedSave()
        {
            if (!File.Exists(SavePath)) return;

            var backupPath = $"{SavePath}.{DateTime.Now:yyyyMMddHHmmss}.bak";
            File.Move(SavePath, backupPath, overwrite: true);
        }

        private static AppState NormalizeState(AppState state)
        {
            state.Settings.HungerDecayRate = Math.Clamp(state.Settings.HungerDecayRate, 1, 3);
            state.Stats.TotalSessionsCompleted = Math.Max(0, state.Stats.TotalSessionsCompleted);
            state.Stats.CurrentSessionStreak = Math.Max(0, state.Stats.CurrentSessionStreak);
            state.Session.ElapsedSeconds = Math.Max(0, state.Session.ElapsedSeconds);
            state.Session.TasksCompletedThisSession = Math.Max(0, state.Session.TasksCompletedThisSession);
            state.Session.XpEarnedThisSession = Math.Max(0, state.Session.XpEarnedThisSession);
            state.Session.LastSessionDurationSeconds = Math.Max(0, state.Session.LastSessionDurationSeconds);

            if (state.Pet != null)
            {
                state.Pet.Name = Controllers.PetController.NormalizePetName(state.Pet.Name);
                state.Pet.HungerLevel = Math.Clamp(state.Pet.HungerLevel, 0, 100);
                state.Pet.Experience = Math.Max(0, state.Pet.Experience);
                state.Pet.Level = Math.Clamp(state.Pet.Level, 1, 30);
            }

            foreach (var task in state.Tasks)
            {
                if (string.IsNullOrWhiteSpace(task.Name))
                {
                    task.Name = "Untitled Task";
                }
                else if (task.Name.Length > Controllers.TaskController.MaxTaskNameLength)
                {
                    task.Name = task.Name[..Controllers.TaskController.MaxTaskNameLength];
                }
            }

            return state;
        }
    }
}
