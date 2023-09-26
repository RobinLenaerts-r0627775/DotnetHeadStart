

namespace DotnetHeadStart;

/// <summary>
/// A base controller for all API controllers. Contains the basic CRUD operations for instances of the given type.
/// The type must be a subclass of BaseModel. 
/// The context must be a subclass of HeadStartContext.
/// </summary>
/// <typeparam name="T">The type of the type you want the crud operations to work on</typeparam>
public class BaseApiController<T> : Controller where T : BaseModel
{
    protected readonly HeadStartContext _context;
    public BaseApiController(HeadStartContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> Get()
    {
        return await _context.Set<T>().ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<T>> Get(int id)
    {
        var item = await _context.Set<T>().FindAsync(id);
        if (item == null)
        {
            return new NotFoundResult();
        }
        return item;
    }

    [HttpPost]
    public async Task<ActionResult<T>> Post(T item)
    {
        _context.Set<T>().Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, T item)
    {
        if (id != item.Id)
        {
            return new BadRequestResult();
        }
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Set<T>().FindAsync(id);
        if (item == null)
        {
            return new NotFoundResult();
        }
        _context.Set<T>().Remove(item);
        await _context.SaveChangesAsync();
        return new NoContentResult();
    }
}
