namespace DotnetHeadStart;

public class BaseRepository<T>(DbContext context) where T : class, IBaseEntity
{
    private readonly DbContext _context = context;

    /// <summary>
    /// Gets all instances of <typeparamref name="T"/> from the database
    /// </summary>
    /// <param name="includeDeleted">Whether to include softdeleted instances of <typeparamref name="T"/></param>
    /// <returns>An IEnumerable of <typeparamref name="T"/></returns>
    public async Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false)
    {
        return await _context.Set<T>().Where(x => includeDeleted || x.DeletedAt == DateTime.MinValue).ToListAsync();
    }

    /// <summary>
    /// Gets all instances of <typeparamref name="T"/> from the database
    /// </summary>
    /// <returns>An IEnumerable of <typeparamref name="T"/></returns>
    public IEnumerable<T> GetAll(bool includeDeleted = false)
    {
        return [.. _context.Set<T>().Where(x => includeDeleted || x.DeletedAt == DateTime.MinValue)];
    }

    /// <summary>
    /// Gets a(n) <typeparamref name="T"/> from the database by its id
    /// </summary>
    /// <param name="id">Id of the object instance to get</param>
    /// <returns>the object instance, or null if none could be found</returns>
    public async Task<T?> GetByIdAsync(string id, bool includeDeleted = false)
    {
        return await _context.Set<T>().Where(x => includeDeleted || x.DeletedAt == DateTime.MinValue).FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Gets a(n) <typeparamref name="T"/> from the database by its id
    /// </summary>
    /// <param name="id">Id of the object instance to get</param>
    /// <returns>the object instance, or null if none could be found</returns>
    public T? GetById(string id, bool includeDeleted = false)
    {
        return _context.Set<T>().Where(x => includeDeleted || x.DeletedAt == DateTime.MinValue).FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Create a(n) <typeparamref name="T"/> in the database
    /// </summary>
    /// <param name="item">object of type <typeparamref name="T"/> to create</param>
    /// <returns></returns>
    public async Task<T> CreateAsync(T item)
    {
        _context.Set<T>().Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    /// <summary>
    /// Create a(n) <typeparamref name="T"/> in the database
    /// </summary>
    /// <param name="item">object of type <typeparamref name="T"/> to create</param>
    /// <returns></returns>
    public T Create(T item)
    {
        _context.Set<T>().Add(item);
        _context.SaveChanges();
        return item;
    }

    /// <summary>
    /// Update a(n) <typeparamref name="T"/> in the database
    /// </summary>
    /// <param name="id">Id of the object to update</param>
    /// <param name="item">object of type <typeparamref name="T"/> to update</param>
    /// <returns></returns>
    public async Task UpdateAsync(string id, T item)
    {
        if (id != item.Id)
        {
            throw new ArgumentException("Id does not match");
        }
        _context.Update(item);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update a(n) <typeparamref name="T"/> in the database
    /// </summary>
    /// <param name="id">Id of the object to update</param>
    /// <param name="item">object of type <typeparamref name="T"/> to update</param>
    /// <returns></returns>
    public void Update(string id, T item)
    {
        if (id != item.Id)
        {
            throw new ArgumentException("Id does not match");
        }
        _context.Update(item);
        _context.SaveChanges();
    }

    /// <summary>
    /// Delete a(n) <typeparamref name="T"/> from the database
    /// </summary>
    /// <param name="id">Id of the <typeparamref name="T"/> to delete</param>
    /// <returns></returns>
    public async Task DeleteAsync(string id, bool softDelete = true)
    {
        var item = await _context.Set<T>().FindAsync(id) ?? throw new ArgumentException("Item not found");
        _context.Set<T>().Remove(item);
        await _context.SaveChangesAsync(softDelete);
    }

    /// <summary>
    /// Delete a(n) <typeparamref name="T"/> from the database
    /// </summary>
    /// <param name="id">Id of the <typeparamref name="T"/> to delete</param>
    /// <returns></returns>
    public void Delete(string id)
    {
        var item = _context.Set<T>().Find(id) ?? throw new ArgumentException("Item not found");
        _context.Set<T>().Remove(item);
        _context.SaveChanges();
    }
}
