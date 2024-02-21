using Microsoft.AspNetCore.Mvc;
using Tasks_WEB_API.Interfaces;
using Tasks_WEB_API.Models;

namespace Tasks_WEB_API.Repositories
{
	public class AuthenRepository : IAuthentificationRepository //,IUtilisateurRepository,ITacheRepository
	{
		private readonly DailyTasksMigrationsContext dataBaseMemoryContext;
		public AuthenRepository(DailyTasksMigrationsContext dataBaseMemoryContext)
		{
			this.dataBaseMemoryContext = dataBaseMemoryContext;
		}
		public Task BasicAuthentification(string username, string password)
		{
			var  users= dataBaseMemoryContext.Utilisateurs.ToList();
			foreach(var item in users)
			{
				if(item.Nom==username && item.Pass==password)
				{
					
				}
			}
			throw new NotImplementedException();
		}

		public Task TokenAuthentification()
		{
			throw new NotImplementedException();
		}
	}


}