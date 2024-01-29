using System.ComponentModel.DataAnnotations;
namespace Tasks_WEB_API;
/// <summary>
/// Représente un utilisateur dans le système
/// </summary>
public class Utilisateur
{   
    /// <summary>
    /// Représente l'identifiant unique d'un utilisateur.
    /// </summary>
    [Key]
    public int ID { get; set; }
    [Required]
    public string? Nom { get; set; }
}
