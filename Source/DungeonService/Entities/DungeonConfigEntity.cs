using DungeonCloud.DungeonCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService
{
    internal class DungeonConfigEntity : DungeonConfig
    {       
        public DungeonConfigEntity(DungeonConfig baseConfig, ISeedGenerator seedGenerator)
            : base(baseConfig.Name, baseConfig.Size.Length, baseConfig.Size.Width)
        {
            GenerationSeed = seedGenerator.GetNextSeed();
        }

        public long GenerationSeed { get; set; }
    }
}
