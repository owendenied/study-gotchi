using System.Windows.Controls;
using StudyGotchi.Interfaces;

namespace StudyGotchi.Models
{
    public abstract class TamagotchiPet : IPet, ITaskObserver
    {
        protected string _name;
        protected int _hungerLevel;
        protected int _xp;
        protected int _currentLevel;
        protected string _evolutionStage;

        public void CompleteTask() { throw new System.NotImplementedException(); }
        public void DecayHunger(int amount) { throw new System.NotImplementedException(); }
        public void LevelUp() { throw new System.NotImplementedException(); }

        // Abstract methods from IPet
        public abstract Image GetCurrentSprite();
        public abstract void OnLevelUp();
        public abstract string GetEvolutionStageName();

        // Interface methods from ITaskObserver
        public void OnTaskCompleted(StudyTask task) { throw new System.NotImplementedException(); }
        public void OnTaskOverdue(StudyTask task) { throw new System.NotImplementedException(); }
    }
}