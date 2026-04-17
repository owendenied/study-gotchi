// ITaskObserver.cs

namespace StudyGotchi.Interfaces
{
    // Observer pattern to connect tasks and pets. 
    // SessionManager registers the pet so it knows when tasks update.
    public interface ITaskObserver
    {
        // Task done -> pet gets XP.
        void OnTaskCompleted(StudyGotchi.Models.StudyTask task);

        // Task missed -> pet gets hungry faster.
        void OnTaskOverdue(StudyGotchi.Models.StudyTask task);
    }
}