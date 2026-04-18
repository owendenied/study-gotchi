using System.Threading;

namespace StudyGotchi.Controllers
{
    public class SessionController
    {
        private PetController _petController;
        private TaskController _taskController;
        private bool _sessionActive;
        private Timer _decayTimer;

        public SessionController(PetController petController, TaskController taskController)
        {
            _petController = petController;
            _taskController = taskController;
        }

        public void StartSession() { throw new System.NotImplementedException(); }
        public void EndSession() { throw new System.NotImplementedException(); }
        public bool IsSessionActive() { return _sessionActive; }
        public string GetSessionSummary() { throw new System.NotImplementedException(); }

        private void OnDecayTick(object state) { throw new System.NotImplementedException(); }
    }
}