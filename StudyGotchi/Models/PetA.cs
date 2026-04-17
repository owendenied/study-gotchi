// PetA.cs

using System.Drawing;

namespace StudyGotchi.Models
{
    // First pet option. Evolves to Child at lvl 2, Adult at lvl 4.
    public class PetA : TamagotchiPet
    {
        // TODO Eume: Update these paths when the art is done!
        private static readonly string SpriteBaby = "Assets/PetA/baby.png";
        private static readonly string SpriteChild = "Assets/PetA/child.png";
        private static readonly string SpriteAdult = "Assets/PetA/adult.png";
        private static readonly string SpriteHungry = "Assets/PetA/hungry.png";
        private static readonly string SpriteHappy = "Assets/PetA/happy.png";

        // Levels needed to evolve
        private static readonly int[] _evolutionThresholds = { 2, 4 };

        public PetA(string name) : base(name) { }

        // Figure out which image to show based on hunger and stage.
        public override Image GetCurrentSprite()
        {
            // TODO Eume: Fix this to actually load the image later
            string path = GetEvolutionStageName() switch
            {
                "Adult" => _hungerLevel < 30 ? SpriteHungry : SpriteAdult,
                "Child" => _hungerLevel < 30 ? SpriteHungry : SpriteChild,
                _ => _hungerLevel < 30 ? SpriteHungry : SpriteBaby,
            };

            return null!; // Image.FromFile(path);
        }

        // Update the stage name string when leveling up.
        public override void OnLevelUp()
        {
            _evolutionStage = GetEvolutionStageName();
        }

        // Check level and return Baby, Child, or Adult.
        public override string GetEvolutionStageName()
        {
            if (_currentLevel >= _evolutionThresholds[1]) return "Adult";
            if (_currentLevel >= _evolutionThresholds[0]) return "Child";
            return "Baby";
        }
    }
}