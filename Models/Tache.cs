using System.ComponentModel.DataAnnotations;

namespace Tasks_WEB_API;
/// <summary>
/// Représente une tache dans le système
/// </summary>
public class Tache
{
    /// <summary>
    /// Représente l'identifiant unique d'une tache.
    /// </summary>
    [Key]
    public int? Matricule { get; set; }
    public string? Titre { get; set; }

    public string? Summary { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateH { get; set; }
}