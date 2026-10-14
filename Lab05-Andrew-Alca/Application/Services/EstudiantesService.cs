using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Repositories;

namespace Lab05_Andrew_Alca.Services.Implements;

public class EstudiantesService : IEstudiantesService
{
    private readonly IUnitOfWork _unitOfWork;

    public EstudiantesService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Estudiantes>> GetAllAsync()
    {
        return await _unitOfWork.Repository<Estudiantes>().GetAllAsync();
    }

    public async Task<Estudiantes?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Repository<Estudiantes>().GetByIdAsync(id);
    }

    public async Task<Estudiantes> CreateAsync(Estudiantes entity)
    {
        await _unitOfWork.Repository<Estudiantes>().AddAsync(entity);
        await _unitOfWork.Complete();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Estudiantes entity)
    {
        var existing = await _unitOfWork.Repository<Estudiantes>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        entity.IdEstudiante = id;
        _unitOfWork.Repository<Estudiantes>().Update(entity);
        await _unitOfWork.Complete();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _unitOfWork.Repository<Estudiantes>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        _unitOfWork.Repository<Estudiantes>().Delete(existing);
        await _unitOfWork.Complete();
        return true;
    }
}
