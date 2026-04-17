// PetC.cs

using System.Drawing;

namespace StudyGotchi.Models
{
    // Third pet option. Works exactly like PetA and PetB.
    public class PetC : TamagotchiPet
    {
        // TODO Eume: Update these paths when the art is done!
        private static readonly string SpriteBaby = "Assets/PetC/baby.png";
        private static readonly string SpriteChild = "Assets/PetC/child.png";
        private static readonly string SpriteAdult = "Assets/PetC/adult.png";
        private static readonly string SpriteHungry = "Assets/PetC/hungry.png";
        private static readonly string SpriteHappy = "Assets/PetC/happy.png";

        private static readonly int[] _evolutionThresholds = { 2, 4 };

        public PetC(string name) : base(name) { }

        public override Image GetCurrentSprite()
        {
            string path = GetEvolutionStageName() switch
            {
                "Adult" => _hungerLevel < 30 ? SpriteHungry : SpriteAdult,
                "Child" => _hungerLevel < 30 ? SpriteHungry : SpriteChild,
                _ => _hungerLevel < 30 ? SpriteHungry : SpriteBaby,
            };
            return null!; // TODO Eume: Image.FromFile(path);
        }

        public override void OnLevelUp()
        {
            _evolutionStage = GetEvolutionStageName();
        }

        public override string GetEvolutionStageName()
        {
            if (_currentLevel >= _evolutionThresholds[1]) return "Adult";
            if (_currentLevel >= _evolutionThresholds[0]) return "Child";
            return "Baby";
        }
    }
}