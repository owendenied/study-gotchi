using System.Windows.Controls;
using StudyGotchi.Models;

namespace StudyGotchi.Controllers
{
    public class PetController
    {
        private TamagotchiPet _activePet;

        public void SetActivePet(int petIndex, string name) 
        {
            // Simple mapping for now
            switch (petIndex)
            {
                case 0: _activePet = new PetA(); break;
                case 1: _activePet = new PetB(); break;
                case 2: _activePet = new PetC(); break;
                default: _activePet = new PetA(); break;
            }
            _activePet.SetName(name);
        }

        public TamagotchiPet GetActivePet() { return _activePet; }
        public bool HasActivePet() { return _activePet != null; }
        public string GetPetName() { return _activePet?.Name ?? "Buddy"; }
        public int GetHungerLevel() { return _activePet?.HungerLevel ?? 100; }
        public int GetXp() { return _activePet?.Experience ?? 0; }
        public int GetLevel() { return _activePet?.Level ?? 1; }
        public string GetEvolutionStageName() { return _activePet?.GetEvolutionStageName() ?? "Baby Stage"; }
        
        public string GetSpritePath() 
        {
            if (_activePet is PetA) return "/Assets/Sprites/petA_happy.gif";
            if (_activePet is PetB) return "/Assets/Sprites/charmander.png";
            if (_activePet is PetC) return "/Assets/Sprites/squirtle.png";
            return "/Assets/Sprites/bulbasaur.png";
        }

        public Image GetCurrentSprite() { throw new System.NotImplementedException(); }
        public void ApplyHungerDecay() 
        { 
            _activePet?.DecayHunger(2); // Moderate decay rate
        }
        public void ApplyOverduePenalty() 
        { 
            _activePet?.DecayHunger(5); // Penalty for overdue
        }
        
        public void CompleteTask(int taskId)
        {
            _activePet?.CompleteTask();
        }
        public void RegisterAsObserver(TaskController taskController) { }
    }
}