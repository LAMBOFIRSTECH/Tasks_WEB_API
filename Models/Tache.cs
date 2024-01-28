using System.ComponentModel.DataAnnotations;

namespace Tasks_WEB_API;
public class Tache
{
    public string? ID { get; set; }
    public string? Titre { get; set; }

    public string? Summary { get; set; }
    // public List<Tache>? Taches { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
}