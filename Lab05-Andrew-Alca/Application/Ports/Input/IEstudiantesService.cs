using Lab05_Andrew_Alca.Models;

namespace Lab05_Andrew_Alca.Services;

public interface IEstudiantesService
{
    Task<IEnumerable<Estudiantes>> GetAllAsync();
    Task<Estudiantes?> GetByIdAsync(int id);
    Task<Estudiantes> CreateAsync(Estudiantes entity);
    Task<bool> UpdateAsync(int id, Estudiantes entity);
    Task<bool> DeleteAsync(int id);
}
