using BeatSaberPlaylistsLib.Legacy;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Providers
{
    //Used for quick importing bplist for levels
    public class BPListParserProvider
    {
        private readonly Database _database;
        private readonly LegacyPlaylistHandler playlistHandler = new LegacyPlaylistHandler();

        public BPListParserProvider(
            Database database)
        {
            _database = database;
        }

        public async Task Parse(Level level, Stream readStream)
        {
            var x = playlistHandler.Deserialize<LegacyPlaylist>(readStream);
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
