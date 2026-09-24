using Lab05_Andrew_Alca.Models;

namespace Lab05_Andrew_Alca.Services;

public interface IAsistenciasService
{
    Task<IEnumerable<Asistencias>> GetAllAsync();
    Task<Asistencias?> GetByIdAsync(int id);
    Task<Asistencias> CreateAsync(Asistencias entity);
    Task<bool> UpdateAsync(int id, Asistencias entity);
    Task<bool> DeleteAsync(int id);
}
