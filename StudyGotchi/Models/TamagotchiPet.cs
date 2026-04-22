using System;
using System.Windows.Controls;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    public abstract class TamagotchiPet : IPet, ITaskObserver
    {
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
        }

        // Abstract methods from IPet
        public abstract Image GetCurrentSprite();
        public abstract void OnLevelUp();
        public virtual string GetEvolutionStageName()
        {
            if (Level <= 10) return "Baby";
            if (Level <= 20) return "Teen";
            return "Adult";
        }

        // Interface methods from ITaskObserver
        public void OnTaskCompleted(StudyTask task) { throw new System.NotImplementedException(); }
        public void OnTaskOverdue(StudyTask task) { throw new System.NotImplementedException(); }
    }
}