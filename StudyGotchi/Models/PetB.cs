using System.Windows.Controls;

namespace StudyGotchi.Models
{
    public class PetB : TamagotchiPet // Rename to PetB and PetC for the other files
    {
        private object[] _sprites; // Placeholder for SpriteSet[]
        private int[] _evolutionThresholds;

        public override Image GetCurrentSprite() { throw new System.NotImplementedException(); }
        public override void OnLevelUp() { throw new System.NotImplementedException(); }
        public override string GetEvolutionStageName() { throw new System.NotImplementedException(); }
    }
}