using Microsoft.EntityFrameworkCore;
using Tasks_WEB_API.Interfaces;
using Tasks_WEB_API.Models;
using System.Threading.Tasks;

namespace Tasks_WEB_API.Repositories
{
	public class UtilisateurRepository : IUtilisateurRepository
	{
		private readonly DailyTasksMigrationsContext _dataBaseMemoryContext;
		public UtilisateurRepository(DailyTasksMigrationsContext dataBaseMemoryContext)
		{

			_dataBaseMemoryContext = dataBaseMemoryContext;

		}
		public async Task<List<Utilisateur>> GetUsers()
		{
			var listUtilisateur = await _dataBaseMemoryContext.Utilisateur.ToListAsync();
			return listUtilisateur;
		}
		public async Task<Utilisateur> GetUserById(int id)
		{
			var utilisateur = await _dataBaseMemoryContext.Utilisateur.FirstOrDefaultAsync(u => u.ID == id);
			return utilisateur;
		}

		public async Task<Utilisateur> CreateUserById(Utilisateur utilisateur)
		{
			await _dataBaseMemoryContext.Utilisateur.AddAsync(utilisateur);
			await _dataBaseMemoryContext.SaveChangesAsync();
			return utilisateur;
		}


		public async Task<List<Utilisateur>> DeleteUserById(int id)
		{
			var listUtilisateur = await _dataBaseMemoryContext.Utilisateur.ToListAsync();
			var utilisateur = await _dataBaseMemoryContext.Utilisateur.FindAsync(id);
			listUtilisateur.Remove(utilisateur);
			await _dataBaseMemoryContext.SaveChangesAsync();
			return listUtilisateur;
		}


		public Utilisateur UpdateUser(Utilisateur utilisateur)
		{
			throw new NotImplementedException();
		}
	}

}