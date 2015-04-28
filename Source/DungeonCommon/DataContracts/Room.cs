using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonCommon
{
    [DataContract]
    public class Room
    {
        public Room(string name, Position location, Dimension size)
        {
            Name = name;
            Location = location;
            Size = size;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Position Location { get; set; }

        [DataMember]
        public Dimension Size { get; set; }
    }
}
