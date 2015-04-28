using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService
{
    internal class DungeonDepot: Dictionary<string, DungeonConfigEntity>, IDungeonDepot
    {
    }
}
