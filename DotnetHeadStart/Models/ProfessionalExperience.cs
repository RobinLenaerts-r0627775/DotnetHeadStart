namespace DotnetHeadStart.Models;
public class ProfessionalExperience : BaseModel
{
    public string Company { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public List<string> TechStack { get; set; } = new List<string>();
    public ProfessionalExperience()
    {
    }
}