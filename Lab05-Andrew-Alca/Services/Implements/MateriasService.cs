using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Repositories;

namespace Lab05_Andrew_Alca.Services.Implements;

public class MateriasService : IMateriasService
{
    private readonly IUnitOfWork _unitOfWork;

    public MateriasService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Materias>> GetAllAsync()
    {
        return await _unitOfWork.Repository<Materias>().GetAllAsync();
    }

    public async Task<Materias?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Repository<Materias>().GetByIdAsync(id);
    }

    public async Task<Materias> CreateAsync(Materias entity)
    {
        await _unitOfWork.Repository<Materias>().AddAsync(entity);
        await _unitOfWork.Complete();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Materias entity)
    {
        var existing = await _unitOfWork.Repository<Materias>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        entity.IdMateria = id;
        _unitOfWork.Repository<Materias>().Update(entity);
        await _unitOfWork.Complete();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _unitOfWork.Repository<Materias>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        _unitOfWork.Repository<Materias>().Delete(existing);
        await _unitOfWork.Complete();
        return true;
    }
}
