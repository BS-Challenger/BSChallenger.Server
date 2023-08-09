using BSChallenger.Server.API.Authentication.BeatLeader;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Views.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
    [ApiController]
    [Route("/test2")]
    public class TestController2 : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<BeatLeaderAuthenticateController>();
        private readonly Database _database;

        public TestController2(
            Database database)
        {
            _database = database;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ranking>>> Get()
        {
            Ranking testRanking = new Ranking(0, "Poodle Saber", "", "https://cdn.assets.beatleader.xyz/PAULclan.png");
            await _database.Rankings.AddAsync(testRanking);
            await _database.SaveChangesAsync();
            var colors = GenerateDissimilarColors(16);
            for (int i = 1; i < 16; i++)
            {
                var level = new Level(i, 1, "Default", colors[i]);
                testRanking.Levels.Add(level);
                await _database.SaveChangesAsync();
                switch (i)
                {
                    case 1:

                        level.AvailableForPass.Add(new Map("0ed5354bcfd2887e8d547f571fc9b85386e55fcb", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("6442e820a86814b9839d6b4ab5dff0be77431e49", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("9778b5bb8601b0ef95e7a0fbc39fd05febcfba26", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("1eb5af27d8b78f8a5081dc7599e761ce21a5f71f", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("4a8867a7782ec4301886fa43f157ff3c48379e09", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("1dd436bd5517f5df82f6f004fa0bf9693fdecf9a", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("9a7ef736492728b986bf0cf0dc4a153810249b0d", "Standard", "ExpertPlus"));
                        break;
                    case 2:
                        level.AvailableForPass.Add(new Map("34a13b3a11c49c551c1500a9295da38612d664c5", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("2fb45e54342f182583799364b923d1b679f27e69", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("24745826dc87ffa5c10d5f2c0a38aabff9a8d0c1", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("d1f0e5178a796f55c97455ab4f0eb7fb3e40deaa", "Standard", "Hard"));
                        level.AvailableForPass.Add(new Map("2a2bc59b7d27fa7c7ce0b2d70ec072fcd388b3d2", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("d091a2e6b02f7a282426cf81c81e6306c05cc595", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("9b611d3ccfa28bf893eb6ff6a6649f102301ad61", "Standard", "Normal"));
                        break;
                    case 3:
                        level.AvailableForPass.Add(new Map("bf5edc35bd8f436eaf90daa9cab7b79dcd696f43", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("06e2f7f9488d670bad3ad4b2ba7134600a3a3c38", "Standard", "Expert"));
                        level.AvailableForPass.Add(new Map("2a77c66d5f8d04a724df8b60d47fdc4b0c16d129", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("e79c820cb53b2e7ba2e9a96a28240adc24495e19", "Standard", "Expert"));
                        level.AvailableForPass.Add(new Map("0101f189955e7d023b48692cddd6a266ed107a03", "Standard", "ExpertPlus"));
                        break;
                    case 4:
                        level.AvailableForPass.Add(new Map("73116beae90cec5a07abec3d2ccd06e6b5e1a654", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("b5aff97c24cf4c0538f8abb34192514cfec739a9", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("272a2d5729257cbb6e1ae063396897d3fa05f9b3", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("9da824d551b47c816ad5113dd3a990c619709084", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("2b02d3233f1d3b0f3b4e055421c49d6c94d64fa7", "Standard", "Expert"));
                        level.AvailableForPass.Add(new Map("6ce73f570814e6f03fa4d18a053afba12b4a9179", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("78e039144b172e7cc2270392cccd6d1f2af3146b", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("6adb3f80c225dab0e68826a63d9cd87b32734bd7", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("3f93a478ea9adf743520818896462467365d02b9", "Standard", "Expert"));
                        level.AvailableForPass.Add(new Map("978899df025e8b1b0ca687d8044c03695677f485", "Standard", "ExpertPlus"));
                        break;
                    case 5:
                        level.AvailableForPass.Add(new Map("bd9cd14b4ea6b246e371e1aff55d47bfc2eb0b12", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("10fe7c7c73d5485e206fda400f9a558a751db057", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("d23eab19ded11bbb2c0451ed7c9a4a29358a4a06", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("64a5f7ba3245d21f1719d8f397acc690351cbe4c", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("d4cc204188654e33f1740bcdc25a30f2968b91b6", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("9dfe862161b935863c5866fb603b44f40d9f2fc7", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("574604b859562040fde5e5345c4868209a6f83ae", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("db3c4bdeaeffff5b7b753a643fd170b239bf7893", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("5ad792d6db5cf4bc97b93201d107cede2a03b227", "Standard", "ExpertPlus"));
                        break;
                    case 6:
                        level.AvailableForPass.Add(new Map("8e750b887741637ad2ad9f8d671a914b9b81bf1f", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("15d309a358aa255f63102a9f76ee5474fc00412e", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("27976a7a949e86f3a60c8b7fd808b730fa3851b4", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("34a1823bf167db7e58032451c84ebe590fcb92e5", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("a69634f70185a77ecb187eec567f92752314e8e3", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("8795fe60349cebd5bbe765bbca2f31c8a355170e", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("e1b8b6405b8acd5a54897ef10c1dfc7db449978d", "Standard", "ExpertPlus"));
                        break;
                    case 7:
                        level.AvailableForPass.Add(new Map("4ef489ddb3135892c9ca6f98b65fc0916c7b6b2f", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("97ed9514a5f3270d9ec2b09c0c0ab8be06c4f350", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("71752101cf7cf96500aff5c4b616d6bdbb92c92f", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("a553134602614261d03c3680dc8e54262090afe9", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("d9be886724111ce7a7c3ac50385fe47089ed11c0", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("df4cb863de11fa27ab9fd8e45b27dde6661bb26e", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("5e9a9e1d70fcd1daf382dc4412ce4598f68184ec", "Standard", "Expert"));
                        break;
                    case 8:
                        level.AvailableForPass.Add(new Map("75ea9becbf82384250273b1eb1e55ecf1ddf44f5", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("159773fa5e8fdece77bab2ccd6629a2d51939da1", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("a28e6ca26b76967beaadf5359f83adceb7f4a88e", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("9d7e69efeb4071c27f07d11905a1c38f767ade55", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("d8e904b20774294233c363ae7fc2bf776054680a", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("91883fc4f6378896c3cf436434c0e3fa199103c2", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("5a53c9a2ab10cd28534679d5243efa17248e9e48", "Standard", "ExpertPlus"));
                        break;
                    case 9:
                        level.AvailableForPass.Add(new Map("63e0092c19e435ce341f57334ea12c30e97a7938", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("a87e6005c418803025e94292351cdb66dec8f869", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("e9ad6e9a87f6cfb3880d2de814e49b564fb373dc", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("fa47b1a4366a1ab2c5c1782853dd5e93178302ec", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("82495bdc1d8cb4516e64846c41d8dd7435b01287", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("3ffadfc7b4b1306602554947a460e3cb457023eb", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("bb94f7468ba91d80d5bd2fc915aa87ad7d6fd803", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("2f0b05882aefa56b83e7fca0446c44475564f928", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("d1f0e5178a796f55c97455ab4f0eb7fb3e40deaa", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("acb67755a329e3828adf8d953cc6ca17d316dbe7", "Standard", "Expert"));
                        break;
                    case 10:
                        level.AvailableForPass.Add(new Map("f953abe882618c2009c1a9ee37b1ff79596bebbd", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("0461bcdaf6b325de15ff890ec0861eb588e431bc", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("17b6eb6075fd0cd41f5f8ec55c5d04e1f4ebe215", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("ab3ce3be4da76d001d5697f9b09c449c3b19854e", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("8c139e33019134f60abfcae35609861adbf37fce", "Standard", "Expert"));
                        level.AvailableForPass.Add(new Map("e3861f33aa0e8407233b8ab9d2269207d3ac02b9", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("706884ced29a601d1b468324691765a3c75ec879", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("676d65d87bee4f2f882c4fab33309a70b628d040", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("f9520b883997708b903bf95d84f60160f9614177", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("8eea849de71dc00a2549ac4d0a27e131eb55e979", "Standard", "Expert"));
                        break;
                    case 11:
                        level.AvailableForPass.Add(new Map("732f99abafe1dcf97d2d88cbe3e64e346e7f5d1f", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("bc8bf1e7c633bc25c2b696a1bca218de3fbccf67", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("d13c3571737db6bcbe31a7a9f2d94d2da1a8c0ea", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("3f94e0fddf9d557ff8c76c9935e9e6bd7b4de04a", "Standard", "Hard"));
                        level.AvailableForPass.Add(new Map("307cee3a7e40c8e97ca2d4691d23695c1349984e", "Standard", "Expert"));
                        level.AvailableForPass.Add(new Map("9b611d3ccfa28bf893eb6ff6a6649f102301ad61", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("c39106f01208bfceda16795880ba966e20f7ef68", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("33a8fdc1ec22c1b6cf791a3f47484997dbc1f7fa", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("29ad20a6f2c7f342a26e33082d4c340c944982c9", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("e4f9d865a8281ddedc3cee7d2b19294c21a6dc29", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("0a8aad6c94d58344031d8b74af6925247ca71a86", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("69992c4ef46311594e29b015b214dd689db73312", "Standard", "ExpertPlus"));
                        break;
                    case 12:
                        level.AvailableForPass.Add(new Map("fe3dfa44f89a09fdcbe2502d55ca314cb8e821c4", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("c0f36f3fd75f8a496d8e18b090fa68e58c9ed81b", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("680898c0246eee92a164cfe525f6597f2e0abf70", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("1b73ad1439f27d457b276a21c7df9350caa19dac", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("0868fe94a52cb6122b4cc51aac7dd18ebd6de6a1", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("fb3235a22813a9d40ca402d13c1c97fe95f6edd8", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("bcc1520144992e32b0175779b46302aa8223dc43", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("fb0eaac20831aee3922fd8985027dd1eb7ede511", "Standard", "ExpertPlus"));
                        break;
                    case 13:
                        level.AvailableForPass.Add(new Map("8a54a2b13f632834f66ffa30cd767cdafe32d351", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("0eb519ebf16f7ffced754df8410a7df4a95a12ad", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("7cbba1873b5f2ef1f37d60b490bb91306ec22877", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("77a550b529d80b0f0cf769b25ef21fa45ea7fa50", "Standard", "Hard"));
                        level.AvailableForPass.Add(new Map("68f00531e6963220216b24f405b7b392a4bce873", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("24a318ee999b08e46f7dc968deb5056bbf04cf10", "Standard", "ExpertPlus"));
                        level.AvailableForPass.Add(new Map("46f4ee7cd5521c3d31091444efc2216057815848", "Standard", "Normal"));
                        level.AvailableForPass.Add(new Map("9f9e4b05dcaf6d44332068109f36f4c9db5aa357", "Standard", "Expert"));
                        break;
                }
            }
            return _database.Rankings;
        }

        //Silly color funcs, Temporary ofc
        public static List<string> GenerateDissimilarColors(int count)
        {
            List<Color> colors = new List<Color>();
            for (int i = 0; i < count; i++)
            {
            Generate:
                var color = GenerateNewColor();
                if (colors.Any(x => isClose(x, color)))
                {
                    goto Generate;
                }
                else
                {
                    colors.Add(color);
                }
            }
            return colors.Select(x => x.R.ToString("X2") + x.G.ToString("X2") + x.B.ToString("X2")).ToList();
        }

        public static double map(double s, double a1, double a2, double b1, double b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        public static Color GenerateNewColor()
        {
            return ColorFromHSV(Random.Shared.NextDouble() * 360f, map(Random.Shared.NextDouble(), 0f, 1f, 0.6f, 1f), map(Random.Shared.NextDouble(), 0f, 1f, 0.6f, 1f));
        }

        public static bool isClose(Color a, Color z, int threshold = 10)
        {
            int r = a.R - z.R,
                g = a.G - z.G,
                b = a.B - z.B;
            return (r * r + g * g + b * b) <= threshold * threshold;
        }
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
