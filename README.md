<a id="top"></a>

<div align="center">
  <img src="StudyGotchi/Assets/Background/logo.png" alt="StudyGotchi logo" width="360" />

  <h1>StudyGotchi</h1>

  <p>
    <strong>StudyGotchi helps students finish tasks by turning study sessions into care, rewards, reminders, and evolution for a tiny desktop pet.</strong>
  </p>

  <p>
    <a href="#installation"><img alt="Platform: Windows" src="https://img.shields.io/badge/platform-Windows-EAF9FF?style=for-the-badge&logo=windows&logoColor=1B4965&labelColor=80D8FF" /></a>
    <a href="#quality"><img alt="Build: local verified" src="https://img.shields.io/badge/build-local%20verified-EEFFFC?style=for-the-badge&logo=dotnet&logoColor=24443F&labelColor=7DDCCF" /></a>
    <a href="LICENSE"><img alt="License: MIT" src="https://img.shields.io/badge/license-MIT-FFF0F6?style=for-the-badge&logo=readthedocs&logoColor=5A315B&labelColor=FF8FB1" /></a>
    <a href="VERSION"><img alt="Version 0.1.0" src="https://img.shields.io/badge/version-0.1.0-F7F1FF?style=for-the-badge&logo=semver&logoColor=3F315B&labelColor=C7A8FF" /></a>
  </p>

  <p>
    <a href="#demo">Demo</a> .
    <a href="#installation">Installation</a> .
    <a href="#quick-start">Quick Start</a> .
    <a href="#features">Features</a> .
    <a href="#repository-media">Repository Media</a>
  </p>
</div>

---

## Demo

<div align="center">
  <img src="docs/demo/studygotchi-demo.gif" alt="StudyGotchi demo showing pet selection, task setup, study session flow, and pet progress" width="900" />
</div>

| Dashboard | Pet Selection | Floating Widget |
| --- | --- | --- |
| <img src="docs/screenshots/dashboard.png" alt="StudyGotchi dashboard with pet status, study timer, and task list" /> | <img src="docs/screenshots/pet-selection.png" alt="StudyGotchi pet selection screen" /> | <img src="docs/screenshots/study-widget.png" alt="StudyGotchi floating study widget" /> |

| Add Task | Session Summary | Settings |
| --- | --- | --- |
| <img src="docs/screenshots/add-task.png" alt="StudyGotchi add task screen with task type and deadline inputs" /> | <img src="docs/screenshots/session-summary.png" alt="StudyGotchi session summary screen with study progress" /> | <img src="docs/screenshots/settings.png" alt="StudyGotchi settings screen" /> |

## Installation

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

## What StudyGotchi Does

StudyGotchi turns a study session into a small care ritual. Your task list becomes a set of quests, your deadlines become gentle reminders, and your progress keeps a virtual pet healthy and growing. Completing work rewards the pet with XP and hunger recovery; missing deadlines or studying too long without care makes the pet need attention.

The app is designed for students who want task tracking to feel warm, visual, and motivating without becoming noisy. It keeps the experience focused: plan work, start the timer, keep your pet nearby, finish tasks, and review the session summary.

## Features

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

## App Flow

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

## Repository Media

The README and repository branding use these checked-in media assets:

| Asset | Location | Purpose |
| --- | --- | --- |
| Demo GIF | `docs/demo/studygotchi-demo.gif` | Shows the main app flow at a glance. |
| Dashboard screenshot | `docs/screenshots/dashboard.png` | Shows the core study session experience. |
| Pet selection screenshot | `docs/screenshots/pet-selection.png` | Shows the available companions and visual style. |
| Floating widget screenshot | `docs/screenshots/study-widget.png` | Shows the always-on-top study companion mode. |
| Add task screenshot | `docs/screenshots/add-task.png` | Shows task creation, deadline input, and task categories. |
| Session summary screenshot | `docs/screenshots/session-summary.png` | Shows progress feedback after a session. |
| Settings screenshot | `docs/screenshots/settings.png` | Shows customization and preferences. |
| App icon | `docs/brand/app-icon.png` | Used for release, app, and repository branding. |
| Social preview image | `docs/brand/social-preview.png` | Used as the GitHub social preview image. |

## Release Metadata

| Item | Location | Purpose |
| --- | --- | --- |
| License | `LICENSE` | MIT license for professional reuse, modification, and distribution. |
| Version source | `VERSION` | Current project version used by the README badge. |
| Build verification | `dotnet build` and `dotnet test` | The README uses a local verification badge; no CI workflow is included in the repository. |

## Visual Identity

StudyGotchi should feel soft, cute, and polished: pastel pinks, light sky blues, soft teal accents, pixel-art pets, rounded panels, and warm copy. Keep screenshots bright and readable, with the pet visible as the emotional anchor.

<div align="center">
  <img src="docs/brand/social-preview.png" alt="StudyGotchi social preview banner" width="720" />
</div>

| Swatch | Hex | Use |
| --- | --- | --- |
| <img src="https://placehold.co/20x20/FF8FB1/FF8FB1.png" alt="#FF8FB1" /> | `#FF8FB1` | Pink accents, reward moments |
| <img src="https://placehold.co/20x20/80D8FF/80D8FF.png" alt="#80D8FF" /> | `#80D8FF` | Sky-blue panels and links |
| <img src="https://placehold.co/20x20/7DDCCF/7DDCCF.png" alt="#7DDCCF" /> | `#7DDCCF` | Teal status and balance accents |
| <img src="https://placehold.co/20x20/FFF0F6/FFF0F6.png" alt="#FFF0F6" /> | `#FFF0F6` | Light pink backgrounds |
| <img src="https://placehold.co/20x20/EAF9FF/EAF9FF.png" alt="#EAF9FF" /> | `#EAF9FF` | Light sky-blue backgrounds |

## Contributing

Contributions should preserve the cozy study loop and keep behavior easy to verify.

1. Create a focused branch from `main`.
2. Keep UI changes aligned with the pastel pixel style.
3. Add or update tests when changing task, session, pet, reminder, or persistence behavior.
4. Run the build-and-test commands before opening a pull request.
5. Include screenshots for user-facing UI changes.

## License

StudyGotchi is released under the [MIT License](LICENSE).

<div align="center">
  <sub>Made for focused study sessions, tiny wins, and one very cared-for desktop pet.</sub>
  <br />
  <a href="#top">Back to top</a>
</div>
