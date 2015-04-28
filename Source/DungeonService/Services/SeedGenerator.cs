using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService
{
    internal class SeedGenerator: ISeedGenerator
    {
        long _prevSeed;

        public SeedGenerator()
        {
            _prevSeed = DateTime.Now.Ticks;
        }

        public long GetNextSeed()
        {
            long currentSeed = DateTime.Now.Ticks;

            //if requests come in faster then ticks we could get duplicate seeds which is not nice so sleep so it is not duplicated
            if(currentSeed == _prevSeed)
            {
                Thread.Sleep(1);
            }
            _prevSeed = currentSeed;

            return currentSeed;
        }
    }
}
