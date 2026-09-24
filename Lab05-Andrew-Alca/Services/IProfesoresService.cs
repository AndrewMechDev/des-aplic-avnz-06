using Lab05_Andrew_Alca.Models;

namespace Lab05_Andrew_Alca.Services;

public interface IProfesoresService
{
    Task<IEnumerable<Profesores>> GetAllAsync();
    Task<Profesores?> GetByIdAsync(int id);
    Task<Profesores> CreateAsync(Profesores entity);
    Task<bool> UpdateAsync(int id, Profesores entity);
    Task<bool> DeleteAsync(int id);
}
