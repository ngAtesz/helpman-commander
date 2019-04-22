using HelpmanCommander.Data.Entities;
using System.Threading.Tasks;
using Task = HelpmanCommander.Data.Entities.Task;

namespace HelpmanCommander.Data
{
    public interface ICompetitionRepository
    {
        // General 
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        //Competitions
        Task<Competition[]> GetAllCompetitionAsync(bool includeExercises = false);
        Task<Competition> GetCompetitionAsync(int id, bool includeExercises = false);
        
        //Stations
        Task<Station[]> GetAllStationByCompetitionAsync(int competitionId);
        //TODO: Investigate skip competitionId from repository QueryById methods
        Task<Station> GetStationByIdAsync(int competitionId, int stationId);

        //Tasks
        Task<Task[]> GetAllTasksAsync();
        Task<Task> GetTaskByIdAsync(int taskId);
        
        //Exercises
        Task<Exercise[]> GetAllExerciseByStationAsync(int competitionId, int stationId);
        Task<Exercise> GetExerciseByIdAsync(int exerciseId);
    }
}
