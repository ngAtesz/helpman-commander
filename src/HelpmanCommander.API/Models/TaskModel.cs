namespace HelpmanCommander.API.Models
{
    public class TaskModel
    {
        public string Name { get; set; }
        public ushort DefaultScore { get; set; }
        public bool IsDefault { get; set; }
        public int? PrerequisiteTaskId { get; set; }
    }
}