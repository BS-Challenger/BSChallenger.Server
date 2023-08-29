using BSChallenger.Server.Models;
using Quartz;
using System;
using System.Threading.Tasks;

namespace BSChallenger.Server.Jobs
{
	public class WeeklyScanHistoryJob : IJob
	{
		private Database _database;
		public WeeklyScanHistoryJob(Database db)
		{
			_database = db;
		}
		public async Task Execute(IJobExecutionContext context)
		{
			foreach(var ranking in _database.Rankings)
			{
				ranking.History.Add(new Models.API.ScanHistory(DateTime.UtcNow));
				await _database.SaveChangesAsync();
			}
		}
	}
}
