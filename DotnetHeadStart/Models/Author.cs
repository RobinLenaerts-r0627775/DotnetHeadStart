namespace DotnetHeadStart.Models;

public class Author
{
    [Key]
    public long ID { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Firstname { get; set; }

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    public override string ToString()
    {
        string res = "[ ID: " + this.ID + ", Name: " + this.Name + ", Firstname: " + this.Firstname + ", Birthdate: " + this.BirthDate + "]";
        return res;
    }
}