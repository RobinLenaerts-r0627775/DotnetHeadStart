namespace DotnetHeadStart.Test;

public class BaseModelTests
{
    readonly HeadStartContext _dataBaseContext;
    public ProfessionalExperience ProfessionalExperience { get; set; }

    public BaseModelTests()
    {
        _dataBaseContext = new DataBaseContext(new DbContextOptionsBuilder<DataBaseContext>().UseInMemoryDatabase("test").Options, new Mock<ILogger>().Object);
        ProfessionalExperience = new ProfessionalExperience
        {
            Company = "Test Company",
            Title = "Test Title",
            StartDate = DateTime.Now.AddMonths(-30),
            EndDate = DateTime.Now,
            Description = "Test Description"
        };
    }

    [Fact]
    public void CreatedAdFilledInWhenNewBaseModelIsSaved()
    {
        _dataBaseContext.ProfessionalExperiences.Add(ProfessionalExperience);
        _dataBaseContext.SaveChanges();
        Assert.NotEqual(DateTime.MinValue, ProfessionalExperience.CreatedAt);
    }

    [Fact]
    public void ModifiedAtFilledInWhenBaseModelIsUpdated()
    {
        _dataBaseContext.ProfessionalExperiences.Add(ProfessionalExperience);
        _dataBaseContext.SaveChanges();
        ProfessionalExperience.Title = "Updated Title";
        _dataBaseContext.SaveChanges();
        Assert.NotEqual(DateTime.MinValue, ProfessionalExperience.ModifiedAt);
    }

    [Fact]
    public void DeletedAtFilledInWhenBaseModelIsDeleted()
    {
        _dataBaseContext.ProfessionalExperiences.Add(ProfessionalExperience);
        _dataBaseContext.SaveChanges();
        _dataBaseContext.ProfessionalExperiences.Remove(ProfessionalExperience);
        _dataBaseContext.SaveChanges();
        Assert.NotEqual(DateTime.MinValue, ProfessionalExperience.DeletedAt);
    }
}