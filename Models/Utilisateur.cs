using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
namespace Tasks_WEB_API;
using Microsoft.OpenApi.Models;

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
	public string? Nom { get; set; }
	//[NotMapped] // Ce champ ne sera pas mappé dans la base de données
	[Required]
	[XmlIgnore] // Ignorer dans la documentation XML
	[JsonIgnore] // Ignorer dans la sérialisation JSON
	public string? Pass { get; set; }//protected et hash 
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
