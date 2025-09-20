namespace OrderOperations.Application.Interfaces;

public interface IGenericRepository<T, TIdType>
{
    List<T> GetAll();
    Task<List<T>> GetAllAsync();
    T GetById(TIdType id);
    Task<T> GetByIdAsync(TIdType id);
    T Create(T entity);
    Task<T> CreateAsync(T entity);
    void DeleteById(TIdType id);
    void Delete(T entity);
    void Update(T entity);
}