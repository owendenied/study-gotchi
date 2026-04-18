using System.Windows.Controls;

namespace StudyGotchi.Interfaces
{
    public interface IPet
    {
        Image GetCurrentSprite();
        void OnLevelUp();
        string GetEvolutionStageName();
    }
}