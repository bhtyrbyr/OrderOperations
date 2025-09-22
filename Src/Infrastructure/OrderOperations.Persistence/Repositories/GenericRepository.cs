using Microsoft.EntityFrameworkCore;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.Persistence.Context;

namespace OrderOperations.Persistence.Repositories;

public class GenericRepository<T, TIdType> : IGenericRepository<T, TIdType> where T : class
{
    private readonly AppDbContext context;

    public GenericRepository(AppDbContext context)
    {
        this.context = context;
    }

    public T Create(T entity)
    {
        context.Add(entity);
        context.SaveChanges();
        return entity;
    }

    public async Task<T> CreateAsync(T entity, CancellationToken ct = default)
    {
        await context.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public void CreateRange(List<T> entities)
    {
        context.AddRange(entities);
        context.SaveChanges();
    }

    public async Task CreateRangeAsync(List<T> entities, CancellationToken ct = default)
    {
        await context.AddRangeAsync(entities, ct);
        await context.SaveChangesAsync(ct);
    }

    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
        context.SaveChanges();
    }

    public void DeleteById(TIdType id)
    {
        var record = context.Set<T>().Find(id);
        if (record is null)
            return;
        context.Set<T>().Remove(record);
        context.SaveChanges();
    }

    public void DeleteRange(List<T> entities)
    {
        context.Set<T>().RemoveRange(entities);
        context.SaveChanges();
    }

    public List<T> GetAll()
    {
        return context.Set<T>().ToList();
    }

    public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Set<T>().ToListAsync(ct);
    }

    public T GetById(TIdType id)
    {
#pragma warning disable CS8603 // Olası null başvuru dönüşü.
        return context.Set<T>().Find(id);
#pragma warning restore CS8603 // Olası null başvuru dönüşü.
    }

    public async Task<T> GetByIdAsync(TIdType id, CancellationToken ct = default)
    {
#pragma warning disable CS8603 // Olası null başvuru dönüşü.
        return await context.Set<T>().FindAsync(id, ct);
#pragma warning restore CS8603 // Olası null başvuru dönüşü.
    }

    public void Update(T entity)
    {
        context.Set<T>().Update(entity);
        context.SaveChanges();
    }

    public void UpdateRange(List<T> entities)
    {
        context.Set<T>().UpdateRange(entities);
        context.SaveChanges();
    }
}
