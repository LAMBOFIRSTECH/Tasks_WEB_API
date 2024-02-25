using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

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
	public string? Nom { get; set; }
	public enum Privilege
	{
		Admin,
		UserX
	}

	[EnumDataType(typeof(Privilege))]
	[Column(TypeName = "nvarchar(24)")]
	[Required]
	public Privilege Role { get; set; }
	//[NotMapped] // Ce champ ne sera pas mappé dans la base de données
	[Required]
	[XmlIgnore] // Ignorer dans la documentation XML
	[JsonIgnore] // Ignorer dans la sérialisation JSON
	private string? _pass { get; set; }
	
	public string DefinirMotDePasse(string password)
	{
		_pass = BCrypt.Net.BCrypt.HashPassword(password);
		return _pass;
	}
	public bool VerifierMotDePasse(string password)
	{
		return BCrypt.Net.BCrypt.Verify(password, _pass);
	}
	public string? Pass => _pass;
}
