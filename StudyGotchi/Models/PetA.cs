using System.Windows.Controls;

namespace StudyGotchi.Models
{
    public class PetA : TamagotchiPet
    {
        public PetA()
        {
            HungerLevel = 100;
            Level = 1;
            Experience = 0;
        }

        public override Image GetCurrentSprite() { return new Image(); }
        public override void OnLevelUp() { }
    }
}