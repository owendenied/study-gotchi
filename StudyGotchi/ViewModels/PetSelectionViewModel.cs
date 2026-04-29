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
        public string BackgroundColor { get; set; }

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
            var baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            AvailablePets = new ObservableCollection<PetOption>
            {
                new PetOption { Name = "Glow", Description = "A bright and playful spirit", SpritePath = System.IO.Path.Combine(baseDir, "Assets/Sprites/yellow_baby.gif"), BackgroundColor = "#FFFDF0" },
                new PetOption { Name = "Sunny", Description = "Warm and full of cheer", SpritePath = System.IO.Path.Combine(baseDir, "Assets/Sprites/charmander.png"), BackgroundColor = "#FFF8F8" },
                new PetOption { Name = "Starry", Description = "Dreamy and curious", SpritePath = System.IO.Path.Combine(baseDir, "Assets/Sprites/squirtle.png"), BackgroundColor = "#FBFBFF" }
            };

            SelectPetCommand = new RelayCommand(p => SelectedPet = p as PetOption);
            
            ChoosePetCommand = new RelayCommand(_ => 
            {
                if (SelectedPet != null)
                {
                    // Set active pet in controller
                    int index = AvailablePets.IndexOf(SelectedPet);
                    ServiceRegistry.PetController.SetActivePet(index, SelectedPet.Name);

                    // Navigate to Settings via the MainWindow
                    var mainWindow = System.Windows.Application.Current.MainWindow as Views.MainWindow;
                    mainWindow?.NavigateToSettings();
                }
            }, _ => SelectedPet != null);

            // Default selection
            SelectedPet = AvailablePets[0];
        }
    }
}
