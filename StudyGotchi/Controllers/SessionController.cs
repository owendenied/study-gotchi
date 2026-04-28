using System;
using System.Threading;
using StudyGotchi.Models;

namespace StudyGotchi.Controllers
{
    public class SessionController
    {
        private PetController _petController;
        private TaskController _taskController;
        private bool _sessionActive;
        private Timer _decayTimer;
        private bool _isFocusMode;

        public SessionController(PetController petController, TaskController taskController)
        {
            _petController = petController;
            _taskController = taskController;
        }

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
    }
}