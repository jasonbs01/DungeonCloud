using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonCommon
{

    [DataContract]
    public enum Direction
    {
        North,
        West,
        East,
        South
    }

    [DataContract]
    public class Door
    {
        public Door(string name, Direction wall, int position)
        {
            Name = name;
            Wall = wall;
            Position = position;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Direction Wall { get; set; }

        [DataMember]
        public int Position { get; set; }

        [DataMember]
        public string ConnectedHallName { get; set; }
    }
}
