using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService
{
    internal interface IIdGenerator
    {
        string GetNextId(string name);
    }
}
