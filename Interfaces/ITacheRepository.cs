using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tasks_WEB_API.Interfaces
{
	public interface ITacheRepository
	{
		Task<List<Tache>> GetTaches();
		Task<Tache> GetTaskById(int? matricule);
		Task<Tache> CreateTaskById(Tache Tache);
		Task<Tache> UpdateTask(Tache Tache);
		Task DeleteTaskById(int id);
   
    }
}