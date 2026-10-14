using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Repositories;

namespace Lab05_Andrew_Alca.Services.Implements;

public class MatriculasService : IMatriculasService
{
    private readonly IUnitOfWork _unitOfWork;

    public MatriculasService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Matriculas>> GetAllAsync()
    {
        return await _unitOfWork.Repository<Matriculas>().GetAllAsync();
    }

    public async Task<Matriculas?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Repository<Matriculas>().GetByIdAsync(id);
    }

    public async Task<(bool Success, string? Error, Matriculas? Matricula)> CreateAsync(Matriculas entity)
    {
        if (entity.IdEstudiante == null || await _unitOfWork.Repository<Estudiantes>().GetByIdAsync(entity.IdEstudiante.Value) == null)
        {
            return (false, "El estudiante indicado no existe.", null);
        }

        if (entity.IdCurso == null || await _unitOfWork.Repository<Cursos>().GetByIdAsync(entity.IdCurso.Value) == null)
        {
            return (false, "El curso indicado no existe.", null);
        }

        await _unitOfWork.Repository<Matriculas>().AddAsync(entity);
        await _unitOfWork.Complete();
        return (true, null, entity);
    }

    public async Task<bool> UpdateAsync(int id, Matriculas entity)
    {
        var existing = await _unitOfWork.Repository<Matriculas>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        entity.IdMatricula = id;
        _unitOfWork.Repository<Matriculas>().Update(entity);
        await _unitOfWork.Complete();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _unitOfWork.Repository<Matriculas>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        _unitOfWork.Repository<Matriculas>().Delete(existing);
        await _unitOfWork.Complete();
        return true;
    }
}
