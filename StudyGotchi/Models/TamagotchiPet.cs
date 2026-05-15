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
            CompleteTask(finishedEarly, StudyTaskType.Activity.GetBaseXpReward());
        }

        public virtual int CompleteTask(bool finishedEarly, int baseXpReward)
        {
            // Starving penalty: halve XP gain if hunger is critically low
            int xpReward = CalculateTaskXp(finishedEarly, baseXpReward);

            if (finishedEarly)
            {
                HungerLevel += 25;
            }
            else
            {
                HungerLevel += 15;
            }

            Experience += xpReward;

            // Level up for every 100 XP accumulated
            while (Experience >= 100)
            {
                Experience -= 100;
                LevelUp();
            }

            return xpReward;
        }

        public int CalculateTaskXp(bool finishedEarly, int baseXpReward)
        {
            double xpMultiplier = (_hungerLevel <= 20) ? 0.5 : 1.0;
            if (finishedEarly) xpMultiplier *= 2.0;
            return (int)(baseXpReward * xpMultiplier);
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
            // Thresholds tuned for a realistic 1-2 hour study session (~5-10 tasks)
            // Baby: level 1-3, Teen: level 4-6, Adult: level 7+
            if (Level <= 3) return "Baby";
            if (Level <= 6) return "Teen";
            return "Adult";
        }

        public virtual string GetMoodName()
        {
            if (HungerLevel >= 80) return "Happy";
            if (HungerLevel >= 40) return "Idle";
            return "Sad";
        }

        public void OnTaskCompleted(StudyTask task) 
        { 
            CompleteTask(task.IsCompletedEarly, task.BaseXpReward);
        }

        public void OnTaskOverdue(StudyTask task) 
        { 
            HungerLevel -= 15;
        }
    }
}
