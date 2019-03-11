namespace HelpmanCommander.Data.Entities
{
    public class ExerciseTask
    {
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }

        public ushort Score { get; set; }
    }
}
