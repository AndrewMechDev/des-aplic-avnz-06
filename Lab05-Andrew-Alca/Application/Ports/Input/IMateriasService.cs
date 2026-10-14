using Lab05_Andrew_Alca.Models;

namespace Lab05_Andrew_Alca.Services;

public interface IMateriasService
{
    Task<IEnumerable<Materias>> GetAllAsync();
    Task<Materias?> GetByIdAsync(int id);
    Task<Materias> CreateAsync(Materias entity);
    Task<bool> UpdateAsync(int id, Materias entity);
    Task<bool> DeleteAsync(int id);
}
