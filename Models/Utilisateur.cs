using System.ComponentModel.DataAnnotations;
namespace Tasks_WEB_API;
public class Utilisateur
{
    public string? Matricule { get; set; }
    [Required]
    public string? Nom { get; set; }
    //public Utilisateur() { }
}