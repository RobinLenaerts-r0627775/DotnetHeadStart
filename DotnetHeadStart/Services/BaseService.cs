namespace DotnetHeadStart;

/// <summary>
/// A base service for all services. Contains the basic CRUD operations for instances of the given type.
/// The type must be a subclass of BaseModel.
/// The context must be a subclass of HeadStartContext.
/// </summary>
/// <typeparam name="T">Type that gets managed by this service</typeparam>
public class BaseService<T> where T : BaseModel
{
    private readonly HeadStartContext _context;
    public BaseService(HeadStartContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all instances of <typeparamref name="T"/> from the database
    /// </summary>
    /// <returns>An IEnumerable of <typeparamref name="T"/></returns>
    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Gets an instance of <typeparamref name="T"/> from the database by its id
    /// </summary>
    /// <param name="id">Id of the object instance to get</param>
    /// <returns>the object instance, or null if none could be found</returns>
    public async Task<T?> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Create a new instance of <typeparamref name="T"/> in the database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<T> Create(T item)
    {
        _context.Set<T>().Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">id of the </param>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">When gihen id does not match id of gihen item</exception>
    public async Task Update(int id, T item)
    {
        if (id != item.Id)
        {
            throw new ArgumentException("Id does not match");
        }
        _context.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var item = await _context.Set<T>().FindAsync(id);
        if (item == null)
        {
            throw new ArgumentException("Item not found");
        }
        _context.Set<T>().Remove(item);
        await _context.SaveChangesAsync();
    }
}
