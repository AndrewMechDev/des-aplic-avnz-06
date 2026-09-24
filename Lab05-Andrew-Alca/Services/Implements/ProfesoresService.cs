using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Repositories;

namespace Lab05_Andrew_Alca.Services.Implements;

public class ProfesoresService : IProfesoresService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProfesoresService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Profesores>> GetAllAsync()
    {
        return await _unitOfWork.Repository<Profesores>().GetAllAsync();
    }

    public async Task<Profesores?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Repository<Profesores>().GetByIdAsync(id);
    }

    public async Task<Profesores> CreateAsync(Profesores entity)
    {
        await _unitOfWork.Repository<Profesores>().AddAsync(entity);
        await _unitOfWork.Complete();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Profesores entity)
    {
        var existing = await _unitOfWork.Repository<Profesores>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        entity.IdProfesor = id;
        _unitOfWork.Repository<Profesores>().Update(entity);
        await _unitOfWork.Complete();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _unitOfWork.Repository<Profesores>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        _unitOfWork.Repository<Profesores>().Delete(existing);
        await _unitOfWork.Complete();
        return true;
    }
}
