using Lab05_Andrew_Alca.Models;

namespace Lab05_Andrew_Alca.Services;

public interface IMatriculasService
{
    Task<IEnumerable<Matriculas>> GetAllAsync();
    Task<Matriculas?> GetByIdAsync(int id);
    Task<(bool Success, string? Error, Matriculas? Matricula)> CreateAsync(Matriculas entity);
    Task<bool> UpdateAsync(int id, Matriculas entity);
    Task<bool> DeleteAsync(int id);
}
