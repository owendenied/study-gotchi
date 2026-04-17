// StudyWidgetWindow.xaml.cs

using StudyGotchi.Managers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StudyGotchi.Views
{
    // The logic behind the floating study widget.
    // Gets data from SessionManager and updates the UI live.
    public partial class StudyWidgetWindow : Window
    {
        private readonly SessionManager _manager;

        // UI timer just to update the screen, doesn't run the actual game logic
        private readonly System.Windows.Threading.DispatcherTimer _uiTimer;

        public StudyWidgetWindow(SessionManager manager)
        {
            InitializeComponent();
            _manager = manager;

            LoadTaskList();
            UpdatePetDisplay();

            // Refresh the screen every 5 seconds so we see hunger drop
            _uiTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _uiTimer.Tick += (s, e) => UpdatePetDisplay();
            _uiTimer.Start();
        }

        // Pulls latest stats from the manager and puts them on screen
        public void UpdatePetDisplay()
        {
            if (_manager.ActivePet == null) return;

            var pet = _manager.ActivePet;

            // Name & stage
            PetNameLabel.Text = pet.Name;
            EvolutionStageLabel.Text = $"{pet.GetEvolutionStageName()} 🥚";

            // TODO: Uncomment when image files are ready
            // var sprite = pet.GetCurrentSprite();
            // if (sprite != null) PetSpriteImage.Source = ConvertToBitmapImage(sprite);

            // Update hunger bar and change colors depending on how low it is
            HungerBar.Value = pet.HungerLevel;
            HungerValueLabel.Text = $"{pet.HungerLevel}%";
            HungerBar.Foreground = pet.HungerLevel switch
            {
                > 60 => new SolidColorBrush(Color.FromRgb(0xB5, 0xEA, 0xD7)),  // Green
                > 30 => new SolidColorBrush(Color.FromRgb(0xFF, 0xEA, 0xA7)),  // Yellow
                _ => new SolidColorBrush(Color.FromRgb(0xFF, 0x6B, 0x6B))   // Red
            };

            // Update XP bar
            XpBar.Value = pet.Xp;
            XpValueLabel.Text = $"{pet.Xp} / 100 XP";
            LevelLabel.Text = $"⭐ Level {pet.CurrentLevel}";
        }

        // Load all tasks from the manager
        private void LoadTaskList()
        {
            TaskCheckList.Items.Clear();
            foreach (var task in _manager.TaskManager.Tasks)
            {
                // TODO: Swap out for custom CheckBox styling later
                TaskCheckList.Items.Add(new TaskListItem
                {
                    TaskId = task.Id,
                    DisplayText = task.TaskName,
                    IsChecked = task.IsCompleted
                });
            }
        }

        // When a task gets clicked in the list
        private void TaskCheckList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // TODO: Fix this up when we make custom checkboxes
            if (TaskCheckList.SelectedItem is TaskListItem item && !item.IsChecked)
            {
                _manager.TaskManager.CompleteTask(item.TaskId);
                item.IsChecked = true;
                UpdatePetDisplay();
            }
        }

        // Close widget and show final summary
        private void EndSessionButton_Click(object sender, RoutedEventArgs e)
        {
            _uiTimer.Stop();
            _manager.EndSession();

            var summaryWindow = new SessionSummaryWindow(_manager);
            summaryWindow.Show();
            this.Close();
        }

        // Little helper to convert regular images into WPF images
        private BitmapImage ConvertToBitmapImage(System.Drawing.Image image)
        {
            using var ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            return bitmap;
        }
    }

    // Dummy class just to hold task data for the listbox
    // TODO: Switch to a real ViewModel later
    public class TaskListItem
    {
        public int TaskId { get; set; }
        public string DisplayText { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public override string ToString() => IsChecked ? $"✅ {DisplayText}" : $"⬜ {DisplayText}";
    }
}