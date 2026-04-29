using System;
using System.Windows.Controls;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    public abstract class TamagotchiPet : IPet, ITaskObserver
    {
        protected string _name = "Buddy";
        protected int _hungerLevel = 100;
        protected int _xp = 0;
        protected int _currentLevel = 1;
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
            CompleteTask(false);
        }

        public virtual void CompleteTask(bool finishedEarly)
        {
            double xpMultiplier = (_hungerLevel <= 20) ? 0.5 : 1.0;

            if (finishedEarly)
            {
                xpMultiplier *= 2.0;
                HungerLevel += 20;
            }
            else
            {
                HungerLevel += 10;
            }

            Experience += (int)(50 * xpMultiplier);

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
            OnLevelUp();
        }

        public abstract Image GetCurrentSprite();
        public abstract void OnLevelUp();

        public virtual string GetEvolutionStageName()
        {
            if (Level <= 10) return "Baby";
            if (Level <= 20) return "Teen";
            return "Adult";
        }

        public void OnTaskCompleted(StudyTask task) 
        { 
            CompleteTask(task.IsCompletedEarly);
        }

        public void OnTaskOverdue(StudyTask task) 
        { 
            HungerLevel -= 15;
        }
    }
}