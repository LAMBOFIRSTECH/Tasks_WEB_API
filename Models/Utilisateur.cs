using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
namespace Tasks_WEB_API;
/// <summary>
/// Représente un utilisateur dans le système.
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
	[Required(ErrorMessage="Le format d'adresse doit etre comme l'exemple suivant : <adress_name>@<mailing_server>.<domain>")]
	[DataType(DataType.EmailAddress)]
	[EmailAddress]
	public string Email { get; set; }
	public enum Privilege
	{
		Admin,
		UserX
	}

	[EnumDataType(typeof(Privilege))]
	[Column(TypeName = "nvarchar(24)")]
	[Required]
	public Privilege Role { get; set; }
	[Required]
	[XmlIgnore] // Ignorer dans la documentation XML
	[JsonIgnore] // Ignorer dans la sérialisation JSON		
	public string? Pass { get; set; }
	public bool ChechHashPassword(string? password)
	{
		return BCrypt.Net.BCrypt.Verify(password, Pass);
	}

	public string SetHashPassword(string? password)
	{

		if (!string.IsNullOrEmpty(password))
		{
			Pass = BCrypt.Net.BCrypt.HashPassword($"{password}" + "");
		}
		return Pass;
	}

}