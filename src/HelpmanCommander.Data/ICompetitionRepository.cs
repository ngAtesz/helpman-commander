using HelpmanCommander.Data.Entities;
using System.Threading.Tasks;

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
    }
}
