using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasks_WEB_API.Interfaces;
using Tasks_WEB_API.Models;

namespace Tasks_WEB_API.Repositories
{
	public class TacheRepository : ITacheRepository
	{
		private readonly DailyTasksMigrationsContext _dataBaseMemoryContext;
		public TacheRepository(DailyTasksMigrationsContext dailyTasksMigrationsContext)
		{
			_dataBaseMemoryContext=dailyTasksMigrationsContext;
		}

        public List<Tache> GetTaches()
        {
            throw new NotImplementedException();
        }
    }
}