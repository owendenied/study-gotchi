using StudyGotchi.Models;

namespace StudyGotchi.Interfaces
{
    public interface ITaskObserver
    {
        void OnTaskCompleted(StudyTask task);
        void OnTaskOverdue(StudyTask task);
    }
}