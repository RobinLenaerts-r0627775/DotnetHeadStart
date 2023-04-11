using DotnetHeadStart.Data;

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
        var test = new ProfessionalExperience
        {
            Company = "Test Company",
            Title = "Test Title",
            StartDate = DateTime.Now.AddMonths(-30),
            EndDate = DateTime.Now,
            Description = "Test Description"
        };
        _databaseContext.ProfessionalExperiences.Add(test);
        _databaseContext.SaveChanges();

        _databaseContext.ProfessionalExperiences.Remove(test);
        _databaseContext.SaveChanges();
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
