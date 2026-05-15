namespace StudyGotchi.Models
{
    public enum StudyTaskType
    {
        WrittenWork,
        Activity,
        Project
    }

    public static class StudyTaskTypeExtensions
    {
        public static string GetDisplayName(this StudyTaskType taskType)
        {
            return taskType switch
            {
                StudyTaskType.Project => "Project",
                StudyTaskType.Activity => "Activity",
                StudyTaskType.WrittenWork => "Written Work",
                _ => "Activity"
            };
        }

        public static int GetBaseXpReward(this StudyTaskType taskType)
        {
            return taskType switch
            {
                StudyTaskType.Project => 250,
                StudyTaskType.Activity => 150,
                StudyTaskType.WrittenWork => 100,
                _ => 150
            };
        }

        public static string GetChipColor(this StudyTaskType taskType)
        {
            return taskType switch
            {
                StudyTaskType.Project => "#D9ECFF",
                StudyTaskType.Activity => "#DFF7E8",
                StudyTaskType.WrittenWork => "#FFF1D8",
                _ => "#DFF7E8"
            };
        }
    }
}
