using DungeonCloud.DungeonCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService
{
    [ServiceContract]
    public interface IDungeonService
    {
        [OperationContract]
        Dungeon GetDungeon(string Id);

        [OperationContract]
        string CreateDungeon(DungeonConfig configuration);

        //[OperationContract]
        //string CreateDungeon(string config);


    }
}
