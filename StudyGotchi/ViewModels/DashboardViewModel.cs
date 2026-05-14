using StudyGotchi.Controllers;

namespace StudyGotchi.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly PetController _petController;

        public string PetName => _petController.GetPetName();
        public string PetStage => _petController.GetEvolutionStageName();
        public string PetSpritePath => _petController.GetSpritePath();
        public int HungerLevel => _petController.GetHungerLevel();
        public int XpLevel => _petController.GetXp();
        public int Level => _petController.GetLevel();
        public string LevelText => $"Level {Level} - {XpLevel} / 100 XP";
        public string PetMoodText
        {
            get
            {
                return HungerLevel switch
                {
                    >= 80 => "Feeling great!",
                    >= 40 => "Hanging in there.",
                    > 20 => "Getting hungry.",
                    _ => "Needs a break soon."
                };
            }
        }

        public TasksViewModel Tasks { get; }
        public SessionViewModel Session { get; }

        public DashboardViewModel(PetController petController, TasksViewModel tasks, SessionViewModel session)
        {
            _petController = petController;
            Tasks = tasks;
            Session = session;
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(PetName));
            RaisePropertyChanged(nameof(PetStage));
            RaisePropertyChanged(nameof(PetSpritePath));
            RaisePropertyChanged(nameof(HungerLevel));
            RaisePropertyChanged(nameof(XpLevel));
            RaisePropertyChanged(nameof(Level));
            RaisePropertyChanged(nameof(LevelText));
            RaisePropertyChanged(nameof(PetMoodText));
        }
    }
}
