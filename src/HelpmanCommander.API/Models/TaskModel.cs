namespace HelpmanCommander.API.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ushort DefaultScore { get; set; }
        public bool IsDefault { get; set; }
        public int? PrerequisiteTaskId { get; set; }
    }
}