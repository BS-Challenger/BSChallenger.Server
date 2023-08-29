using Microsoft.VisualBasic;
using Sqids;
using System.Linq;

namespace BSChallenger.Server.Providers
{
    public static class SqidProvider
    {
        private static SqidsEncoder encoder = new SqidsEncoder(new SqidsOptions() { MinLength = 5 });
        public static string GenerateID(IDType type, int id)
        {
            var sqid = encoder.Encode(id);
            switch (type)
            {
                case IDType.Ranking:
                    sqid = "rnk_" + sqid;
                    break;
                case IDType.Level:
                    sqid = "lvl_" + sqid;
                    break;
                case IDType.Map:
                    sqid = "map_" + sqid;
                    break;
                case IDType.User:
                    sqid = "usr_" + sqid;
                    break;
				case IDType.ScanHistory:
					sqid = "scn_" + sqid;
					break;
			}
            return sqid;
        }
    }
    public enum IDType
    {
        Ranking,
        Level,
        User,
        Map,
        ScanHistory
    }
}
