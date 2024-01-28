using System.ComponentModel.DataAnnotations;
namespace Tasks_WEB_API;
public class Utilisateur
{
    [Key]
    public int ID { get; set; }
    [Required]
    public string Nom { get; set; }
    //public Utilisateur() { }
}
