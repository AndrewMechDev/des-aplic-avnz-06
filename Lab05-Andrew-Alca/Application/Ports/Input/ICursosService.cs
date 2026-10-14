using Lab05_Andrew_Alca.Models;

namespace Lab05_Andrew_Alca.Services;

public interface ICursosService
{
    Task<IEnumerable<Cursos>> GetAllAsync();
    Task<Cursos?> GetByIdAsync(int id);
    Task<Cursos> CreateAsync(Cursos entity);
    Task<bool> UpdateAsync(int id, Cursos entity);
    Task<bool> DeleteAsync(int id);
}
