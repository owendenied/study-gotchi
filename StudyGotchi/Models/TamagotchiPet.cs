using System;
using System.Windows.Controls;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    public abstract class TamagotchiPet : IPet, ITaskObserver
    {
<<<<<<< HEAD
        protected string _name;
        protected int _hungerLevel = 100;
        protected int _xp = 0;
        protected int _currentLevel = 1;
        protected string _evolutionStage = "Baby";

        public void CompleteTask(bool finishedEarly)
        {
            double xpMultiplier = (_hungerLevel <= 20) ? 0.5 : 1.0;

            if (finishedEarly)
            {
                xpMultiplier *= 2.0;
                _hungerLevel += 20;
            }
            else
            {
                _hungerLevel += 10;
            }

            _xp += (int)(50 * xpMultiplier);
            if (_hungerLevel > 100) _hungerLevel = 100;

            if (_xp >= 100)
            {
                LevelUp();
            }
        }

        public void DecayHunger(bool isFocusMode)
        {
            int drainAmount = isFocusMode ? 10 : 5;
            _hungerLevel -= drainAmount;

            if (_hungerLevel < 0) _hungerLevel = 0;
        }

        public void LevelUp()
        {
            _currentLevel++;
            _xp = 0;
            OnLevelUp();
        }

        public void OnTaskCompleted(StudyTask task)
        {
            CompleteTask(task.IsCompletedEarly);
        }

        public void OnTaskOverdue(StudyTask task)
        {
            _hungerLevel -= 15;
            if (_hungerLevel < 0) _hungerLevel = 0;
=======
        protected string _name = "Buddy";
        protected int _hungerLevel;
        protected int _xp;
        protected int _currentLevel;
        protected string _evolutionStage = "Baby";

        public string Name 
        { 
            get => _name; 
            set => _name = value; 
        }

        public int HungerLevel 
        { 
            get => _hungerLevel; 
            set { _hungerLevel = Math.Clamp(value, 0, 100); }
        }

        public int Experience 
        { 
            get => _xp; 
            set { _xp = value; }
        }

        public int Level 
        { 
            get => _currentLevel; 
            set { _currentLevel = Math.Clamp(value, 1, 30); }
        }

        public void SetName(string name) => Name = name;

        public virtual void CompleteTask() 
        { 
            HungerLevel += 20;
            Experience += 25;
            if (Experience >= 100)
            {
                Experience -= 100;
                LevelUp();
            }
        }
        
        public virtual void DecayHunger(int amount) 
        { 
            if (HungerLevel > 0)
            {
                HungerLevel -= amount;
            }
            else
            {
                Experience = Math.Max(0, Experience - amount);
            }
        }

        public virtual void LevelUp() 
        { 
            Level++;
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
        }

        public abstract Image GetCurrentSprite();
        public abstract void OnLevelUp();
<<<<<<< HEAD
        public abstract string GetEvolutionStageName();
=======
        public virtual string GetEvolutionStageName()
        {
            if (Level <= 10) return "Baby";
            if (Level <= 20) return "Teen";
            return "Adult";
        }

        // Interface methods from ITaskObserver
        public void OnTaskCompleted(StudyTask task) { throw new System.NotImplementedException(); }
        public void OnTaskOverdue(StudyTask task) { throw new System.NotImplementedException(); }
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
    }
}