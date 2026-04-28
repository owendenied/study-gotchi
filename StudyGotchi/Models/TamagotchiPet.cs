using System.Windows.Controls;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    public abstract class TamagotchiPet : IPet, ITaskObserver
    {
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
        }

        public abstract Image GetCurrentSprite();
        public abstract void OnLevelUp();
        public abstract string GetEvolutionStageName();
    }
}