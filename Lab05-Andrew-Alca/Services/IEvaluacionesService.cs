using Lab05_Andrew_Alca.Models;

namespace Lab05_Andrew_Alca.Services;

public interface IEvaluacionesService
{
    Task<IEnumerable<Evaluaciones>> GetAllAsync();
    Task<Evaluaciones?> GetByIdAsync(int id);
    Task<Evaluaciones> CreateAsync(Evaluaciones entity);
    Task<bool> UpdateAsync(int id, Evaluaciones entity);
    Task<bool> DeleteAsync(int id);
}
