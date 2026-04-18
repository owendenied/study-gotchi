using System.Windows.Controls;
using StudyGotchi.Models;

namespace StudyGotchi.Controllers
{
    public class PetController
    {
        private TamagotchiPet _activePet;

        public void SetActivePet(int petIndex, string name) { throw new System.NotImplementedException(); }
        public TamagotchiPet GetActivePet() { return _activePet; }
        public bool HasActivePet() { throw new System.NotImplementedException(); }
        public string GetPetName() { throw new System.NotImplementedException(); }
        public int GetHungerLevel() { throw new System.NotImplementedException(); }
        public int GetXp() { throw new System.NotImplementedException(); }
        public int GetLevel() { throw new System.NotImplementedException(); }
        public string GetEvolutionStageName() { throw new System.NotImplementedException(); }
        public Image GetCurrentSprite() { throw new System.NotImplementedException(); }
        public void ApplyHungerDecay() { throw new System.NotImplementedException(); }
        public void ApplyOverduePenalty() { throw new System.NotImplementedException(); }
        public void RegisterAsObserver(TaskController taskController) { throw new System.NotImplementedException(); }
    }
}