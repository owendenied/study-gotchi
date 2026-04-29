# 🐾 Study-Gotchi
### A Tamagotchi-Style Study Tracker | Team 11 | CS 222 AOOP

Study-Gotchi is a desktop application built with **C# and WPF (.NET 10)** that gamifies your productivity. It combines a task manager with a virtual pet whose wellbeing is directly tied to your study habits.

---

## 🚀 Quick Setup Flow

Follow these steps to get the workspace running on your local machine:

### 1. Prerequisites
- **Visual Studio 2022** (v17.8+) with **.NET Desktop Development** workload installed.
- **.NET 10 SDK** (Ensure you have the latest preview/release).
- **Git** installed on your system.

### 2. Clone the Repository
```bash
git clone https://github.com/owendenied/study-gotchi.git
cd study-gotchi/StudyGotchi
```

### 3. Open & Build
- **Option A (Recommended):** Open `StudyGotchi.sln` in Visual Studio. Wait for dependencies to restore, then press `F5` to run.
- **Option B (CLI):** Run the following command in the project root:
  ```bash
  dotnet run
  ```

---

## 🏗️ Workspace & Architecture Flow

The project follows a strict **3-Layer Architecture** (Presentation, Application, and Domain) to ensure clean code and clear separation of responsibilities.

### Layer Breakdown
- **Presentation Layer (`/Views`)**: Handled via XAML and C# code-behind. Manages everything the user sees.
- **Application Layer (`/Controllers`)**: The "middle-man" (Pet, Task, and Session controllers) that coordinates between the UI and the underlying logic.
- **Domain Layer (`/Models`, `/Interfaces`)**: The core game logic (hunger decay, XP calculations, evolution stages).

### Core Navigation Flow
1. **Pet Selection**: Pick your starting companion.
2. **Task Setup**: Add your study goals and deadlines.
3. **Settings**: Name your pet and adjust preferences.
4. **Dashboard**: The main focus area where your pet lives and you track tasks.
5. **Study Widget**: An always-on-top floating pet to keep you company while you work in other apps.
6. **Session Summary**: See your XP gains and evolution progress at the end of a session.

---

## 🛠️ Tech Stack
- **Language:** C#
- **Framework:** WPF (.NET 10)
- **Design:** XAML with a custom pastel aesthetic.
- **Architecture:** 3-Tier MVC-inspired (Controllers/Models/Views).

---

## 👥 Team Contribution Workflow

To maintain a clean repository, we follow these golden rules:

1. **Branching**: Never push directly to `main`.
   - Use `dev` for shared integration.
   - Use `feature/<name>-<task>` for individual work.
2. **Daily Sync**: Always `git pull origin dev` before you start coding.
3. **Commits**: Use clear, descriptive commit messages:
   - `feat:` for new features.
   - `fix:` for bug fixes.
   - `style:` for UI/XAML changes.
4. **Pull Requests**: Open a PR to `dev` once your feature is complete. Review and merge are handled by the Lead Dev.

---

*For more detailed technical specifications, check the `Documents/` folder.*

