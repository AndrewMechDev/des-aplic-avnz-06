using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Repositories;

namespace Lab05_Andrew_Alca.Services.Implements;

public class EvaluacionesService : IEvaluacionesService
{
    private readonly IUnitOfWork _unitOfWork;

    public EvaluacionesService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Evaluaciones>> GetAllAsync()
    {
        return await _unitOfWork.Repository<Evaluaciones>().GetAllAsync();
    }

    public async Task<Evaluaciones?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Repository<Evaluaciones>().GetByIdAsync(id);
    }

    public async Task<Evaluaciones> CreateAsync(Evaluaciones entity)
    {
        await _unitOfWork.Repository<Evaluaciones>().AddAsync(entity);
        await _unitOfWork.Complete();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Evaluaciones entity)
    {
        var existing = await _unitOfWork.Repository<Evaluaciones>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        entity.IdEvaluacion = id;
        _unitOfWork.Repository<Evaluaciones>().Update(entity);
        await _unitOfWork.Complete();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _unitOfWork.Repository<Evaluaciones>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        _unitOfWork.Repository<Evaluaciones>().Delete(existing);
        await _unitOfWork.Complete();
        return true;
    }
}
