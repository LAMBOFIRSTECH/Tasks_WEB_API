using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tasks_WEB_API.Interfaces
{
	public interface IUtilisateurRepository
	{
		Task<List<Utilisateur>> GetUsers();
		Task<Utilisateur> GetUserById(int id);
		Task<Utilisateur> CreateUserById(Utilisateur utilisateur);
		Task<Utilisateur>  DeleteUserById(int id);
		Utilisateur UpdateUser(Utilisateur utilisateur);
	}
}