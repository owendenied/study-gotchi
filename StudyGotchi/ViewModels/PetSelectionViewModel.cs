using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StudyGotchi.Models;
using StudyGotchi.Services;

namespace StudyGotchi.ViewModels
{
    public class PetOption : BaseViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string SpritePath { get; set; } = string.Empty;
        public string TintColor { get; set; } = string.Empty;
        public string SelectedSpritePath { get; set; } = "pack://application:,,,/Assets/Background/cracked_egg.png";
        public string DisplaySpritePath => IsCracked ? SelectedSpritePath : SpritePath;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
            }
        }

        private bool _isCracked;
        public bool IsCracked
        {
            get => _isCracked;
            set
            {
                _isCracked = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DisplaySpritePath));
            }
        }
    }

    public class PetSelectionViewModel : BaseViewModel
    {
        private ObservableCollection<PetOption> _availablePets = new();
        public ObservableCollection<PetOption> AvailablePets
        {
            get => _availablePets;
            set { _availablePets = value; RaisePropertyChanged(); }
        }

        private PetOption? _selectedPet;
        public PetOption? SelectedPet
        {
            get => _selectedPet;
            set
            {
                if (_selectedPet != null) _selectedPet.IsSelected = false;
                _selectedPet = value;
                if (_selectedPet != null) _selectedPet.IsSelected = true;
                RaisePropertyChanged();
                if (ChoosePetCommand is RelayCommand chooseCommand)
                {
                    chooseCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand ChoosePetCommand { get; }
        public ICommand SelectPetCommand { get; }

        public PetSelectionViewModel()
        {
            AvailablePets = new ObservableCollection<PetOption>
            {
                new PetOption {
                    Name = "Glow",
                    SpritePath = "pack://application:,,,/Assets/Background/egg.png",
                    TintColor = "#A0FFB0" // Soft Green
                },
                new PetOption {
                    Name = "Sunny",
                    SpritePath = "pack://application:,,,/Assets/Background/egg.png",
                    TintColor = "#FFE0A0" // Soft Orange/Yellow
                },
                new PetOption {
                    Name = "Starry",
                    SpritePath = "pack://application:,,,/Assets/Background/egg.png",
                    TintColor = "#A0B0FF" // Soft Blue/Purple
                }
            };

            var activePet = ServiceRegistry.PetController.GetActivePet();
            if (activePet != null)
            {
                for (var i = 0; i < AvailablePets.Count; i++)
                {
                    var pet = AvailablePets[i];
                    pet.IsCracked = false;

                    if ((i == 0 && activePet is PetA) ||
                        (i == 1 && activePet is PetB) ||
                        (i == 2 && activePet is PetC))
                    {
                        SelectedPet = pet;
                        pet.IsCracked = true;
                    }
                }
            }
            else
            {
                SelectedPet = AvailablePets[0];
            }

            SelectPetCommand = new RelayCommand(p =>
            {
                if (p is PetOption petOption) SelectedPet = petOption;
            });

            ChoosePetCommand = new RelayCommand(_ =>
            {
                if (SelectedPet != null)
                {
                    foreach (var pet in AvailablePets)
                    {
                        pet.IsCracked = false;
                    }

                    SelectedPet.IsCracked = true;

                    int index = AvailablePets.IndexOf(SelectedPet);
                    var activePet = ServiceRegistry.PetController.GetActivePet();
                    
                    bool isNewPet = activePet == null;
                    if (!isNewPet)
                    {
                        if (index == 0 && !(activePet is PetA)) isNewPet = true;
                        if (index == 1 && !(activePet is PetB)) isNewPet = true;
                        if (index == 2 && !(activePet is PetC)) isNewPet = true;
                    }

                    if (isNewPet)
                    {
                        ServiceRegistry.SessionController.ResetSession();
                        ServiceRegistry.TaskController.ClearTasks();
                        ServiceRegistry.PetController.SetActivePet(index, SelectedPet.Name);
                    }

                    ServiceRegistry.SaveState();

                    // Navigate to Settings via the MainWindow
                    var mainWindow = System.Windows.Application.Current.MainWindow as Views.MainWindow;
                    mainWindow?.NavigateToSettings();
                }
            }, _ => SelectedPet != null);
        }
    }
}
