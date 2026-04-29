using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StudyGotchi.Models;
using StudyGotchi.Services;

namespace StudyGotchi.ViewModels
{
    public class PetOption : BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpritePath { get; set; }
        public string TintColor { get; set; } // New property for the egg's glow/tint

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; RaisePropertyChanged(); }
        }
    }

    public class PetSelectionViewModel : BaseViewModel
    {
        private ObservableCollection<PetOption> _availablePets;
        public ObservableCollection<PetOption> AvailablePets
        {
            get => _availablePets;
            set { _availablePets = value; RaisePropertyChanged(); }
        }

        private PetOption _selectedPet;
        public PetOption SelectedPet
        {
            get => _selectedPet;
            set
            {
                if (_selectedPet != null) _selectedPet.IsSelected = false;
                _selectedPet = value;
                if (_selectedPet != null) _selectedPet.IsSelected = true;
                RaisePropertyChanged();
                ((RelayCommand)ChoosePetCommand).RaiseCanExecuteChanged();
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

            SelectPetCommand = new RelayCommand(p => SelectedPet = p as PetOption);

            ChoosePetCommand = new RelayCommand(_ =>
            {
                if (SelectedPet != null)
                {
                    int index = AvailablePets.IndexOf(SelectedPet);
                    ServiceRegistry.PetController.SetActivePet(index, SelectedPet.Name);

                    // Navigate to Settings via the MainWindow
                    var mainWindow = System.Windows.Application.Current.MainWindow as Views.MainWindow;
                    mainWindow?.NavigateToSettings();
                }
            }, _ => SelectedPet != null);


            SelectedPet = AvailablePets[0];
        }
    }
}