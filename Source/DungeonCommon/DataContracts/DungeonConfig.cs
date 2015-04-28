using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonCommon
{
    [DataContract]
    public class DungeonConfig
    {
        public DungeonConfig(string name, int length, int width)
        {
            Name = name;
            Size = new Dimension(length, width);
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Dimension Size { get; set; }

    }
}
