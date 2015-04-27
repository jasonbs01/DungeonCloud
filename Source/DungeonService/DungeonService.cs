using DungeonCloud.DungeonCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService
{
    public class DungeonService : IDungeonService
    {
        [WebGet(UriTemplate = "/dungeons/{id}", ResponseFormat = WebMessageFormat.Json)]
        public Dungeon GetDungeon(string Id)
        {
            return new Dungeon(Id);
        }
    }
}
