using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Repositories;

namespace Lab05_Andrew_Alca.Services.Implements;

public class CursosService : ICursosService
{
    private readonly IUnitOfWork _unitOfWork;

    public CursosService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Cursos>> GetAllAsync()
    {
        return await _unitOfWork.Repository<Cursos>().GetAllAsync();
    }

    public async Task<Cursos?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Repository<Cursos>().GetByIdAsync(id);
    }

    public async Task<Cursos> CreateAsync(Cursos entity)
    {
        await _unitOfWork.Repository<Cursos>().AddAsync(entity);
        await _unitOfWork.Complete();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Cursos entity)
    {
        var existing = await _unitOfWork.Repository<Cursos>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        entity.IdCurso = id;
        _unitOfWork.Repository<Cursos>().Update(entity);
        await _unitOfWork.Complete();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _unitOfWork.Repository<Cursos>().GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        _unitOfWork.Repository<Cursos>().Delete(existing);
        await _unitOfWork.Complete();
        return true;
    }
}
