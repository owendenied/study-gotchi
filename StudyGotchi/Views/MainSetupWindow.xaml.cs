// MainSetupWindow.xaml.cs

using StudyGotchi.Managers;
using StudyGotchi.Models;
using System.Windows;
using System.Windows.Media;

namespace StudyGotchi.Views
{
    // Code-behind for the main setup screen. Talks to the SessionManager.
    public partial class MainSetupWindow : Window
    {
        private readonly SessionManager _manager;
        private int _selectedPetIndex = -1;  // -1 means nothing picked yet

        public MainSetupWindow()
        {
            InitializeComponent();
            _manager = new SessionManager();
        }

        // Pick a pet when the card is clicked
        private void PetACard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => SelectPet(0);
        private void PetBCard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => SelectPet(1);
        private void PetCCard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => SelectPet(2);

        private void SelectPet(int index)
        {
            _selectedPetIndex = index;

            // TODO Eume: Maybe add a bounce animation later? Just borders for now.
            PetACard.BorderThickness = new Thickness(index == 0 ? 3 : 0);
            PetBCard.BorderThickness = new Thickness(index == 1 ? 3 : 0);
            PetCCard.BorderThickness = new Thickness(index == 2 ? 3 : 0);

            string[] names = { "Pet A", "Pet B", "Pet C" };
            SelectedPetLabel.Text = $"{names[index]} selected ✓";
            SelectedPetLabel.Foreground = new SolidColorBrush(Color.FromRgb(0x6B, 0xC5, 0x88));
        }

        // Add a new task to the list
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string taskName = TaskNameInput.Text.Trim();

            if (string.IsNullOrEmpty(taskName))
            {
                ValidationMessage.Text = "Please enter a task name.";
                return;
            }

            DateTime deadline = DeadlinePicker.SelectedDate ?? DateTime.Now.AddHours(24);
            _manager.TaskManager.AddTask(taskName, deadline);

            // Refresh UI list
            TaskListBox.Items.Clear();
            foreach (var task in _manager.TaskManager.Tasks)
                TaskListBox.Items.Add($"• {task.TaskName}  ({task.Deadline:MMM d, h:mm tt})");

            TaskNameInput.Clear();
            ValidationMessage.Text = string.Empty;
        }

        // Open settings pop-up
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(_manager);
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        // Launch the widget
        private void StartSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPetIndex == -1)
            {
                ValidationMessage.Text = "Please select a pet first!";
                return;
            }

            if (_manager.TaskManager.Tasks.Count == 0)
            {
                ValidationMessage.Text = "Please add at least one task!";
                return;
            }

            // TODO: Grab pet name from settings later.
            string petName = "My Pet";
            TamagotchiPet selectedPet = _selectedPetIndex switch
            {
                0 => new PetA(petName),
                1 => new PetB(petName),
                2 => new PetC(petName),
                _ => new PetA(petName)
            };

            _manager.SetActivePet(selectedPet);
            _manager.StartSession();

            var widget = new StudyWidgetWindow(_manager);
            widget.Show();
            this.Hide();
        }
    }
}