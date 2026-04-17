// PetB.cs

using System.Drawing;

namespace StudyGotchi.Models
{
    // Second pet option. Works exactly like PetA.
    public class PetB : TamagotchiPet
    {
        // TODO Eume: Update these paths when the art is done!
        private static readonly string SpriteBaby = "Assets/PetB/baby.png";
        private static readonly string SpriteChild = "Assets/PetB/child.png";
        private static readonly string SpriteAdult = "Assets/PetB/adult.png";
        private static readonly string SpriteHungry = "Assets/PetB/hungry.png";
        private static readonly string SpriteHappy = "Assets/PetB/happy.png";

        private static readonly int[] _evolutionThresholds = { 2, 4 };

        public PetB(string name) : base(name) { }

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