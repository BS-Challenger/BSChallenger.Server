using BeatSaberPlaylistsLib.Legacy;
using BSChallenger.Server.Models.API;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Models
{
	//Used for quick importing bplist for levels
	public class BPListParser
	{
		private readonly Database _database;
		private readonly LegacyPlaylistHandler playlistHandler = new LegacyPlaylistHandler();
		private readonly ILogger _logger = Log.ForContext<BPListParser>();

		public BPListParser(
			Database database)
		{
			_database = database;
		}

		public async Task Parse(Level level, Stream readStream)
		{
			var x = playlistHandler.Deserialize<LegacyPlaylist>(readStream);
			_logger.Information(x.Title);
			_logger.Information(x.Count.ToString());
			foreach (var item in x)
			{
				level.AvailableForPass.Add(new Map(item.Hash, item.Difficulties[0].Characteristic, ((BeatmapDifficulty)item.Difficulties[0].DifficultyValue).ToString()));
			}
			await _database.SaveChangesAsync();
		}

		public enum BeatmapDifficulty
		{
			Easy,
			Normal,
			Hard,
			Expert,
			ExpertPlus
		}
	}
}
