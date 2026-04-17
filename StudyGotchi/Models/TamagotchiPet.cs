// TamagotchiPet.cs

using System.Drawing;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    // Base class for all the pets. Handles the math for XP and hunger.
    public abstract class TamagotchiPet : IPet, ITaskObserver
    {
        public const int MaxHunger = 100;
        public const int MinHunger = 0;
        public const int XpPerTask = 20;
        public const int HungerPenaltyOverdue = 15;
        public const int HungerDecayPerTick = 2;

        protected string _name;
        protected int _hungerLevel;
        protected int _xp;
        protected int _currentLevel;
        protected string _evolutionStage;

        protected TamagotchiPet(string name)
        {
            _name = name;
            _hungerLevel = MaxHunger;
            _xp = 0;
            _currentLevel = 1;
            _evolutionStage = "Baby";
        }

        public string Name { get => _name; set => _name = value; }
        public int HungerLevel => _hungerLevel;
        public int Xp => _xp;
        public int CurrentLevel => _currentLevel;
        public string EvolutionStage => _evolutionStage;

        // Give XP and check if we leveled up
        public void CompleteTask()
        {
            _xp += XpPerTask;
            CheckLevelUp();
        }

        // Slowly drains hunger. Can't go below 0.
        public void DecayHunger(int amount = HungerDecayPerTick)
        {
            _hungerLevel = Math.Max(MinHunger, _hungerLevel - amount);
        }

        // Check if we hit the XP limit
        private void CheckLevelUp()
        {
            if (_xp >= GetXpThresholdForNextLevel())
            {
                _xp = 0;
                _currentLevel++;
                OnLevelUp();
            }
        }

        // Flat 100 XP per level unless we change it later
        protected virtual int GetXpThresholdForNextLevel() => 100;

        // Triggered by the TaskManager when a task is checked off
        public void OnTaskCompleted(StudyTask task)
        {
            CompleteTask();
        }

        // Triggered by TaskManager if a task goes late
        public void OnTaskOverdue(StudyTask task)
        {
            DecayHunger(HungerPenaltyOverdue);
        }

        // The specific pet classes will fill these in
        public abstract Image GetCurrentSprite();
        public abstract void OnLevelUp();
        public abstract string GetEvolutionStageName();
    }
}