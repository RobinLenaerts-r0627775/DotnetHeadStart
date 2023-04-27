using DotnetHeadStart.MVC.Data;

namespace DotnetHeadStart.Controllers;

public class HomeController : Controller
{
    private readonly ILogger _logger;
    private readonly DataBaseContext _databaseContext;

    public HomeController(ILogger logger, DataBaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
