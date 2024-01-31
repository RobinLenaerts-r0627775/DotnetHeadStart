namespace DotnetHeadStart;

public class BaseRepository<T>(BaseContext context) where T : BaseModel
{
    private readonly BaseContext _context = context;

    /// <summary>
    /// Gets all instances of <typeparamref name="T"/> from the database
    /// </summary>
    /// <returns>An IEnumerable of <typeparamref name="T"/></returns>
    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Gets a(n) <typeparamref name="T"/> from the database by its id
    /// </summary>
    /// <param name="id">Id of the object instance to get</param>
    /// <returns>the object instance, or null if none could be found</returns>
    public async Task<T?> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Create a(n) <typeparamref name="T"/> in the database
    /// </summary>
    /// <param name="item">object of type <typeparamref name="T"/> to create</param>
    /// <returns></returns>
    public async Task<T> Create(T item)
    {
        _context.Set<T>().Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    /// <summary>
    /// Update a(n) <typeparamref name="T"/> in the database
    /// </summary>
    /// <param name="id">Id of the object to update</param>
    /// <param name="item">object of type <typeparamref name="T"/> to update</param>
    /// <returns></returns>
    public async Task Update(int id, T item)
    {
        if (id != item.Id)
        {
            throw new ArgumentException("Id does not match");
        }
        _context.Update(item);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete a(n) <typeparamref name="T"/> from the database
    /// </summary>
    /// <param name="id">Id of the <typeparamref name="T"/> to delete</param>
    /// <returns></returns>
    public async Task Delete(int id)
    {
        var item = await _context.Set<T>().FindAsync(id) ?? throw new ArgumentException("Item not found");
        _context.Set<T>().Remove(item);
        await _context.SaveChangesAsync();
    }
}
