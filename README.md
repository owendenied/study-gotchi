<a id="top"></a>

<div align="center">
  <img src="StudyGotchi/Assets/Background/logo.png" alt="StudyGotchi logo" width="360" />

  <h1>StudyGotchi</h1>

  <p>
    <strong>StudyGotchi helps students finish tasks by turning study sessions into care, rewards, reminders, and evolution for a tiny desktop pet.</strong>
  </p>

  <p>
    <a href="#instructions-on-how-to-run-the-application"><img alt="Platform: Windows" src="https://img.shields.io/badge/platform-Windows-EAF9FF?style=for-the-badge&logo=windows&logoColor=1B4965&labelColor=80D8FF" /></a>
    <a href="#quality"><img alt="Build: local verified" src="https://img.shields.io/badge/build-local%20verified-EEFFFC?style=for-the-badge&logo=dotnet&logoColor=24443F&labelColor=7DDCCF" /></a>
    <a href="LICENSE"><img alt="License: MIT" src="https://img.shields.io/badge/license-MIT-FFF0F6?style=for-the-badge&logo=readthedocs&logoColor=5A315B&labelColor=FF8FB1" /></a>
    <a href="VERSION"><img alt="Version 0.1.0" src="https://img.shields.io/badge/version-0.1.0-F7F1FF?style=for-the-badge&logo=semver&logoColor=3F315B&labelColor=C7A8FF" /></a>
  </p>

  <p>
    <a href="#demo">Demo</a> .
    <a href="#instructions-on-how-to-run-the-application">Run</a> .
    <a href="#features-and-functionalities-of-the-system">Features</a> .
    <a href="#uml-diagram">UML</a> .
    <a href="#developers">Developers</a> .
    <a href="#quality">Quality</a>
  </p>
</div>

---

<div align="center">
  <img src="docs/brand/social-preview.png" alt="StudyGotchi social preview banner" width="900" />
</div>

## Demo

<div align="center">
  <img src="docs/demo/studygotchi-demo.gif" alt="StudyGotchi demo showing pet selection, task setup, study session flow, and pet progress" width="900" />
</div>

| Dashboard | Pet Selection | Floating Widget |
| --- | --- | --- |
| <img src="docs/screenshots/dashboard.png" alt="StudyGotchi dashboard with pet status, study timer, and task list" width="280" height="180" /> | <img src="docs/screenshots/pet-selection.png" alt="StudyGotchi pet selection screen" width="280" height="180" /> | <img src="docs/screenshots/study-widget.png" alt="StudyGotchi floating study widget" width="280" height="180" /> |

| Add Task | Session Summary | Settings |
| --- | --- | --- |
| <img src="docs/screenshots/add-task.png" alt="StudyGotchi add task screen with task type and deadline inputs" width="280" height="180" /> | <img src="docs/screenshots/session-summary.png" alt="StudyGotchi session summary screen with study progress" width="280" height="180" /> | <img src="docs/screenshots/settings.png" alt="StudyGotchi settings screen" width="280" height="180" /> |

## Instructions on How to Run the Application

### Requirements

- Windows 10 or later
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Git
- Visual Studio 2022 with the `.NET desktop development` workload, if you prefer running the app from the IDE

### Install From Source

```bash
git clone https://github.com/owendenied/study-gotchi.git
cd study-gotchi
dotnet restore StudyGotchi/StudyGotchi.slnx
dotnet build StudyGotchi/StudyGotchi.csproj
```

### Run From Visual Studio

1. Open `StudyGotchi/StudyGotchi.slnx`.
2. Restore NuGet packages when prompted.
3. Set `StudyGotchi` as the startup project.
4. Press `F5`.

## Quick Start

Copy and run this from a terminal:

```bash
git clone https://github.com/owendenied/study-gotchi.git
cd study-gotchi
dotnet restore StudyGotchi/StudyGotchi.slnx
dotnet run --project StudyGotchi/StudyGotchi.csproj
```

Once the app opens:

1. Choose a pet.
2. Name your pet and adjust settings.
3. Add one study task with a deadline.
4. Start a session.
5. Complete the task to feed your pet, earn XP, and progress toward evolution.

## Project Description and Purpose

StudyGotchi turns a study session into a small care ritual. Your task list becomes a set of quests, your deadlines become gentle reminders, and your progress keeps a virtual pet healthy and growing. Completing work rewards the pet with XP and hunger recovery; missing deadlines or studying too long without care makes the pet need attention.

The app is designed for students who want task tracking to feel warm, visual, and motivating without becoming noisy. It keeps the experience focused: plan work, start the timer, keep your pet nearby, finish tasks, and review the session summary.

## UML Diagram

The diagram below reflects the current app wiring, including the shared service registry, view models, controllers, services, and model layer that keep the study loop and pet care system in sync.

```mermaid
classDiagram
    direction TB

    class MainWindow {
        <<Window>>
        +NavigateToPetSelection()
        +NavigateToTaskSetup()
        +NavigateToSettings()
        +NavigateToDashboard()
        +NavigateToSummary()
        +LaunchWidgetMode()
        +ToggleFullScreen()
    }

    class StudyWidgetWindow {
        <<Window>>
        +UpdatePetDisplay()
        +ReturnToDashboard()
    }

    class MainWindowViewModel {
        <<ViewModel>>
        +CurrentViewModel: object
    }

    class DashboardViewModel {
        <<ViewModel>>
        +PetName: string
        +PetStage: string
        +LevelText: string
        +Refresh()
    }

    class SessionViewModel {
        <<ViewModel>>
        +ClockText: string
        +StartCommand
        +PauseCommand
        +EndCommand
    }

    class TasksViewModel {
        <<ViewModel>>
        +Tasks: ObservableCollection
        +AddTask()
    }

    class PetSelectionViewModel {
        <<ViewModel>>
        +AvailablePets: ObservableCollection
        +ChoosePetCommand
        +SelectPetCommand
    }

    class ServiceRegistry {
        <<Service Hub>>
        +Initialize()
        +SaveState()
        +ResetApp()
        +ResetSaveData()
    }

    class SessionController {
        <<Controller>>
        +StartSession()
        +PauseSession()
        +ResumeSession()
        +EndSession()
        +ResetSession()
    }

    class TaskController {
        <<Controller>>
        +AddTask()
        +CompleteTask()
        +ClearTasks()
        +ReplaceTasks()
        +CheckAndNotifyOverdue()
    }

    class PetController {
        <<Controller>>
        +SetActivePet()
        +RestoreActivePet()
        +CompleteTask()
        +ApplyOverduePenalty()
        +GetSpritePath()
    }

    class ReminderService {
        <<Service>>
        +Start()
        +Pause()
        +Resume()
        +Stop()
    }

    class AudioService {
        <<Service>>
        +PlayBgm()
        +PlaySfx()
        +StopBgm()
    }

    class AppStatePersistenceService {
        <<Service>>
        +Load()
        +Save()
        +Reset()
    }

    class TaskManager {
        <<Model>>
        +AddTask()
        +CompleteTask()
        +CheckForOverdueTasks()
    }

    class StudyTask {
        <<Model>>
        +Name: string
        +Deadline: DateTime
        +Complete()
        +IsOverdue()
    }

    class TamagotchiPet {
        <<Abstract Model>>
        +Name: string
        +HungerLevel: int
        +Experience: int
        +Level: int
        +CompleteTask()
        +DecayHunger()
        +LevelUp()
    }

    class PetA {
        <<Pet Family>>
    }

    class PetB {
        <<Pet Family>>
    }

    class PetC {
        <<Pet Family>>
    }

    MainWindow --> MainWindowViewModel : binds
    MainWindow --> ServiceRegistry : initializes
    MainWindow --> StudyWidgetWindow : opens

    ServiceRegistry --> DashboardViewModel : creates
    ServiceRegistry --> SessionViewModel : creates
    ServiceRegistry --> TasksViewModel : creates
    ServiceRegistry --> PetSelectionViewModel : creates
    ServiceRegistry --> SessionController : owns
    ServiceRegistry --> TaskController : owns
    ServiceRegistry --> PetController : owns
    ServiceRegistry --> ReminderService : owns
    ServiceRegistry --> AudioService : owns
    ServiceRegistry --> AppStatePersistenceService : saves state

    DashboardViewModel --> PetController : pet display
    DashboardViewModel --> TasksViewModel : task list
    DashboardViewModel --> SessionViewModel : timer state
    SessionViewModel --> SessionController : session actions
    TasksViewModel --> TaskController : task actions
    PetSelectionViewModel --> PetController : selected pet

    SessionController --> PetController : hunger and level snapshot
    SessionController --> TaskController : session rewards
    ReminderService --> TaskController : deadline checks
    TaskController --> TaskManager : delegates
    TaskManager "1" o-- "*" StudyTask : stores
    PetController --> TamagotchiPet : active pet
    PetA --|> TamagotchiPet
    PetB --|> TamagotchiPet
    PetC --|> TamagotchiPet
    AppStatePersistenceService ..> StudyTask : restores tasks
    AppStatePersistenceService ..> TamagotchiPet : restores pet state

    classDef window fill:#FFF0F6,stroke:#FF8FB1,color:#5A315B,stroke-width:2px
    classDef viewModel fill:#EAF9FF,stroke:#80D8FF,color:#1B4965,stroke-width:2px
    classDef controller fill:#EEFFFC,stroke:#7DDCCF,color:#24443F,stroke-width:2px
    classDef service fill:#F7F1FF,stroke:#C7A8FF,color:#3F315B,stroke-width:2px
    classDef model fill:#FFF7D6,stroke:#F8C85A,color:#5B4818,stroke-width:2px
    classDef pet fill:#F1FFE8,stroke:#94D780,color:#2E5731,stroke-width:2px

    class MainWindow window
    class StudyWidgetWindow window
    class MainWindowViewModel viewModel
    class DashboardViewModel viewModel
    class SessionViewModel viewModel
    class TasksViewModel viewModel
    class PetSelectionViewModel viewModel
    class SessionController controller
    class TaskController controller
    class PetController controller
    class ServiceRegistry service
    class ReminderService service
    class AudioService service
    class AppStatePersistenceService service
    class TaskManager model
    class StudyTask model
    class TamagotchiPet model
    class PetA pet
    class PetB pet
    class PetC pet
```

## Features and Functionalities of the System

| Area | What It Does |
| --- | --- |
| Study sessions | Start, pause, resume, reset, and end timed focus sessions. |
| Task planning | Add study tasks with names, categories, and deadlines. |
| Task rewards | Completing tasks grants XP and restores hunger; early completion doubles XP. |
| Pet care | Hunger decays during sessions, overdue tasks reduce hunger, and low hunger reduces XP gain. |
| Progression | Pets level from 1 to 30 and evolve through Baby, Teen, and Adult stages. |
| Pet selection | Choose from three pet families with separate sprites and mood states. |
| Reminders | Deadline reminders fire at 10, 5, and 1 minute before a task is due. |
| Floating widget | Keep an always-on-top pet companion visible while working in other apps. |
| Audio feedback | Background music and event sounds support session starts, reminders, completion, and level-ups. |
| Persistence | Saves pet, tasks, settings, stats, and active session state locally. |
| Recovery | Backs up corrupted save files and starts cleanly instead of crashing. |

## Explanation of How the Program Works

StudyGotchi begins with pet selection. Once a pet is chosen, the app stores that choice, loads the current save state, and moves into the dashboard. From there, the player adds study tasks, starts a focus session, and keeps working while the pet's hunger, XP, and evolution state update in the background.

The session controller tracks elapsed time, task completions, pause and resume state, and the summary data that appears at the end of a run. The task controller manages study tasks and overdue checks, while the pet controller handles hunger, XP, evolution, and sprite selection. Reminder, audio, and persistence services keep the experience responsive, cozy, and safely saved locally.

The floating widget gives the pet a smaller home on the screen, and the session summary gives a tidy recap once the study block is done. It is meant to feel like a gentle loop: choose a pet, plan the work, focus, finish, and watch the pet grow with you.

```mermaid
flowchart LR
    A["Choose a pet"] --> B["Add study tasks"]
    B --> C["Start a session"]
    C --> D["Complete tasks"]
    D --> E["Earn XP and restore hunger"]
    E --> F["Level up and evolve"]
    C --> G["Deadline reminders"]
    G --> D
    C --> H["Open floating widget"]
    H --> D

    style A fill:#FFF0F6,stroke:#FF8FB1,color:#5A315B
    style B fill:#EAF9FF,stroke:#80D8FF,color:#1B4965
    style C fill:#EEFFFC,stroke:#7DDCCF,color:#24443F
    style D fill:#FFF7D6,stroke:#F8C85A,color:#5B4818
    style E fill:#FFF0F6,stroke:#FF8FB1,color:#5A315B
    style F fill:#EAF9FF,stroke:#80D8FF,color:#1B4965
    style G fill:#F7F1FF,stroke:#C7A8FF,color:#3F315B
    style H fill:#EEFFFC,stroke:#7DDCCF,color:#24443F
```

## Tech Stack

| Layer | Technology |
| --- | --- |
| App | C# desktop application |
| Runtime | .NET 10 |
| UI | WPF |
| Animation | WpfAnimatedGif |
| Persistence | Local JSON in `%APPDATA%\StudyGotchi\state.json` |
| Testing | xUnit |
| Assets | Pixel sprites, PNG backgrounds, Pixelify Sans fonts, WAV/MP3 audio |

## Architecture

StudyGotchi keeps the codebase split into clear responsibilities: screens render the interface, view models expose bindable state, controllers coordinate user actions, models own the pet/task rules, and services handle persistence, audio, reminders, and shared app wiring.

```mermaid
flowchart TD
    Views["Views<br/>screens, controls, widget"]
    ViewModels["ViewModels<br/>bindable UI state"]
    Controllers["Controllers<br/>pet, task, session coordination"]
    Models["Models<br/>pet rules, tasks, rewards"]
    Services["Services<br/>save data, audio, reminders"]
    Assets["Assets<br/>sprites, backgrounds, fonts, sounds"]
    Tests["Tests<br/>xUnit behavior coverage"]

    Views --> ViewModels
    ViewModels --> Controllers
    Controllers --> Models
    Controllers --> Services
    Views --> Assets
    Tests --> Controllers
    Tests --> Models
    Tests --> Services

    style Views fill:#FFF0F6,stroke:#FF8FB1,color:#5A315B
    style ViewModels fill:#EAF9FF,stroke:#80D8FF,color:#1B4965
    style Controllers fill:#EEFFFC,stroke:#7DDCCF,color:#24443F
    style Models fill:#FFF7D6,stroke:#F8C85A,color:#5B4818
    style Services fill:#F7F1FF,stroke:#C7A8FF,color:#3F315B
    style Assets fill:#FFF9FB,stroke:#FFB7CE,color:#5A315B
    style Tests fill:#F5FCFF,stroke:#B9E6FF,color:#24445C
```

## Project Structure

```text
study-gotchi/
|-- StudyGotchi/                 Desktop app
|   |-- Assets/                   Pixel backgrounds, pet sprites, fonts, and audio
|   |-- Controllers/              Session, task, and pet coordination
|   |-- Converters/               WPF value converters
|   |-- Interfaces/               Pet and task observer contracts
|   |-- Models/                   Core task and pet rules
|   |-- Services/                 Persistence, reminders, audio, and service registry
|   |-- Styles/                   Global WPF resources and button styles
|   |-- ViewModels/               Bindable state for screens and controls
|   `-- Views/                    Windows and user controls
|-- StudyGotchi.Tests/           xUnit tests
|-- README.md
`-- .gitignore
```

## Quality

Run the test suite:

```bash
dotnet test StudyGotchi.Tests/StudyGotchi.Tests.csproj
```

Run a full local verification pass:

```bash
dotnet restore StudyGotchi/StudyGotchi.slnx
dotnet build StudyGotchi/StudyGotchi.csproj --no-restore
dotnet test StudyGotchi.Tests/StudyGotchi.Tests.csproj --no-restore
```

The tests cover task rewards, early-completion bonuses, hunger penalties, session state, reminders, corrupted-save recovery, persisted stats, input normalization, and pet sprite selection.

## Developers

- Eume C. Derez - GUI Designer and Artist
- Coleen B. Dichoso - Logic Developer
- Goldwyn Daine Kierzene D. Mendoza - Project Manager

## Contributing

Contributions are welcome! Whether it is a bug fix, a new pet sprite, or a quality-of-life tweak, if it fits the cozy study loop, it belongs here.

1. Branch from `main` and keep changes focused.
2. Match the pastel pixel style for any UI changes.
3. Add or update tests for anything touching tasks, sessions, pets, reminders, or persistence.
4. Run the build-and-test commands before opening a pull request.
5. Include screenshots for visual changes.

## License

StudyGotchi is released under the [MIT License](LICENSE).

<div align="center">
  <sub>Made for focused study sessions, tiny wins, and one very cared-for desktop pet.</sub>
  <br />
  <a href="#top">Back to top</a>
</div>
