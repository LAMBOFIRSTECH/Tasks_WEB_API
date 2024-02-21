using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Tasks_WEB_API;

/// <summary>
/// Représente un utilisateur dans le système
/// </summary>
public class Utilisateur
{
	/// <summary>
	/// Représente l'identifiant unique d'un utilisateur.
	/// </summary>
	//[Key]
	[Required]
	public int ID { get; set; }
	[Required]
	public string? Nom { get; set; }
	public string? Pass { get; set; }
	public enum Privilege
	{
		Admin,
		UserX

	}
	[EnumDataType(typeof(Privilege))]
	[Column(TypeName = "nvarchar(24)")]
	[Required]
	public Privilege Role { get; set; }
}
