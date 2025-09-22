namespace OrderOperations.Application.Interfaces;

public interface IGenericRepository<T, TIdType>
{
    List<T> GetAll();
    Task<List<T>> GetAllAsync(CancellationToken ct = default);
    T GetById(TIdType id);
    Task<T> GetByIdAsync(TIdType id, CancellationToken ct = default);
    T Create(T entity);
    Task<T> CreateAsync(T entity, CancellationToken ct = default);
    void DeleteById(TIdType id);
    void Delete(T entity);
    void Update(T entity);
}