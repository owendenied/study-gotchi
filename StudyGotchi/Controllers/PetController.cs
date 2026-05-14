using System.Windows.Controls;
using System.Windows.Media.Imaging;
using StudyGotchi.Models;

namespace StudyGotchi.Controllers
{
    public class PetController
    {
        public const int MaxPetNameLength = 24;
        private static readonly Dictionary<Type, string> SpriteFolders = new()
        {
            [typeof(PetA)] = nameof(PetA),
            [typeof(PetB)] = nameof(PetB),
            [typeof(PetC)] = nameof(PetC)
        };

        private TamagotchiPet? _activePet;

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
            _activePet.SetName(NormalizePetName(name));
        }

        public void RestoreActivePet(string petType, string name, int hungerLevel, int experience, int level)
        {
            _activePet = petType switch
            {
                nameof(PetB) => new PetB(),
                nameof(PetC) => new PetC(),
                _ => new PetA()
            };

            _activePet.SetName(NormalizePetName(name));
            _activePet.HungerLevel = hungerLevel;
            _activePet.Experience = experience;
            _activePet.Level = level;
        }

        public TamagotchiPet? GetActivePet() { return _activePet; }
        public void ClearActivePet() { _activePet = null; }
        public string GetActivePetType() { return _activePet?.GetType().Name ?? nameof(PetA); }
        public bool HasActivePet() { return _activePet != null; }
        public string GetPetName() { return _activePet?.Name ?? "Buddy"; }
        public int GetHungerLevel() { return _activePet?.HungerLevel ?? 100; }
        public int GetXp() { return _activePet?.Experience ?? 0; }
        public int GetLevel() { return _activePet?.Level ?? 1; }
        public event Action? PetLeveledUp;

        public string GetEvolutionStageName() { return _activePet?.GetEvolutionStageName() ?? "Baby Stage"; }
        
        private static string SpriteAbsPath(string relative)
        {
            return System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, relative);
        }

        public string GetSpritePath() 
        {
            string stage = _activePet?.GetEvolutionStageName() ?? "Baby";
            string mood = _activePet?.GetMoodName() ?? "Idle";
            string petFolder = _activePet != null && SpriteFolders.TryGetValue(_activePet.GetType(), out var folder)
                ? folder
                : nameof(PetA);
            
            return SpriteAbsPath($"Assets/Sprites/{petFolder}/{petFolder}_{stage}_{mood}.gif");
        }

        public Image GetCurrentSprite()
        {
            return new Image
            {
                Source = new BitmapImage(new Uri(GetSpritePath(), UriKind.Absolute)),
                Stretch = System.Windows.Media.Stretch.Uniform
            };
        }
        public void ApplyHungerDecay() 
        { 
            _activePet?.DecayHunger(2); // Moderate decay rate
        }
        public void ApplyOverduePenalty() 
        { 
            _activePet?.DecayHunger(5); // Penalty for overdue
        }
        
        public int CompleteTask(int taskId, bool finishedEarly = false, int baseXpReward = 150)
        {
            int oldLevel = _activePet?.Level ?? 1;
            int awardedXp = _activePet?.CompleteTask(finishedEarly, baseXpReward) ?? 0;
            if (_activePet?.Level > oldLevel)
            {
                PetLeveledUp?.Invoke();
            }
            return awardedXp;
        }

        public static string NormalizePetName(string? name)
        {
            var normalized = string.IsNullOrWhiteSpace(name) ? "Buddy" : name.Trim();
            return normalized.Length <= MaxPetNameLength
                ? normalized
                : normalized[..MaxPetNameLength];
        }
    }
}
