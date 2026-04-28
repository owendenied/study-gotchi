<<<<<<< HEAD
﻿using System;
using System.Threading;
using StudyGotchi.Models;
=======
using System;
using System.Windows.Threading;
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977

namespace StudyGotchi.Controllers
{
    public class SessionController
    {
        private PetController _petController;
        private TaskController _taskController;
        private bool _sessionActive;
<<<<<<< HEAD
        private Timer _decayTimer;
        private bool _isFocusMode;
=======
        private DispatcherTimer _sessionTimer;
        private DateTime _startTime;
        private TimeSpan _elapsedTime;
        private int _tasksCompletedThisSession;
        private int _xpEarnedThisSession;

        public event Action<string>? TimeUpdated;
        public event Action? StatsUpdated;

        public int TasksCompletedThisSession => _tasksCompletedThisSession;
        public int XpEarnedThisSession => _xpEarnedThisSession;
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977

        public SessionController(PetController petController, TaskController taskController)
        {
            _petController = petController;
            _taskController = taskController;
            _sessionTimer = new DispatcherTimer();
            _sessionTimer.Interval = TimeSpan.FromSeconds(1);
            _sessionTimer.Tick += OnTimerTick;

            _taskController.TaskCompleted += (t) => 
            {
                if (_sessionActive)
                {
                    _tasksCompletedThisSession++;
                    _xpEarnedThisSession += 25; // Matching the reward in TamagotchiPet
                }
            };
        }

<<<<<<< HEAD
        public void StartSession(bool focusMode)
        {
            _sessionActive = true;
            _isFocusMode = focusMode;
            _decayTimer = new Timer(OnDecayTick, null, 0, 600000);
        }

        public void EndSession()
        {
            _sessionActive = false;
            _decayTimer?.Dispose();
        }

        public bool IsSessionActive()
        {
            return _sessionActive;
        }

        private void OnDecayTick(object state)
        {
            if (_sessionActive)
            {
                _petController.GetCurrentPet().ApplyHungerDecay(_isFocusMode);
            }
        }

        public string GetSessionSummary()
        {
            return _sessionActive ? "Session is currently running..." : "No active session.";
        }
=======
        public void StartSession() 
        { 
            _sessionActive = true;
            _startTime = DateTime.Now;
            _elapsedTime = TimeSpan.Zero;
            _tasksCompletedThisSession = 0;
            _xpEarnedThisSession = 0;
            _sessionTimer.Start();
            TimeUpdated?.Invoke("00:00:00");
        }

        public void EndSession() 
        { 
            _sessionActive = false;
            _sessionTimer.Stop();
        }

        public bool IsSessionActive() { return _sessionActive; }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            _elapsedTime = DateTime.Now - _startTime;
            TimeUpdated?.Invoke(_elapsedTime.ToString(@"hh\:mm\:ss"));
            
            // Moderate hunger decay: decay every 15 seconds
            if ((int)_elapsedTime.TotalSeconds % 15 == 0 && (int)_elapsedTime.TotalSeconds > 0)
            {
                _petController.ApplyHungerDecay();
                StatsUpdated?.Invoke();
            }
        }
>>>>>>> 005cd88206b7db80b57d63b8208cce7c2b3ad977
    }
}