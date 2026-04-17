// IPet.cs

using System.Drawing;

namespace StudyGotchi.Interfaces
{
    // Interface for all pets. Makes it easy to add new pets later without breaking stuff.
    public interface IPet
    {
        // Gets the right image depending on if the pet is happy, hungry, evolving, etc.
        Image GetCurrentSprite();

        // Called when XP is high enough to level up.
        void OnLevelUp();

        // Returns a string like "Baby", "Child", or "Adult".
        string GetEvolutionStageName();
    }
}