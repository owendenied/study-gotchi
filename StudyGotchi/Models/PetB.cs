using System.Windows.Controls;

namespace StudyGotchi.Models
{
    public class PetB : TamagotchiPet
    {
        public PetB()
        {
            HungerLevel = 100;
            Level = 1;
            Experience = 0;
        }

        public override Image GetCurrentSprite() { return new Image(); }
        public override void OnLevelUp() { }
    }
}