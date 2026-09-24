using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Repositories;

namespace Lab05_Andrew_Alca.Services.Implements;

public class AsistenciasService : IAsistenciasService
{
    private readonly IUnitOfWork _unitOfWork;

    public AsistenciasService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Asistencias>> GetAllAsync()
    {
        return await _unitOfWork.Repository<Asistencias>().GetAllAsync();
    }

    public async Task<Asistencias?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Repository<Asistencias>().GetByIdAsync(id);
    }

    public async Task<Asistencias> CreateAsync(Asistencias entity)
    {
        await _unitOfWork.Repository<Asistencias>().AddAsync(entity);
        await _unitOfWork.Complete();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Asistencias entity)
    {
        var existing = await _unitOfWork.Repository<Asistencias>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        entity.IdAsistencia = id;
        _unitOfWork.Repository<Asistencias>().Update(entity);
        await _unitOfWork.Complete();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _unitOfWork.Repository<Asistencias>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        _unitOfWork.Repository<Asistencias>().Delete(existing);
        await _unitOfWork.Complete();
        return true;
    }
}
