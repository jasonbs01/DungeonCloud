using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonCommon
{
    [DataContract]
    public class Dungeon
    {
        #region Constructors

        public Dungeon(string name, int length, int width)
        {            
            Name = name;
            Size = new Dimension(length, width);
        }

        #endregion

        #region Public Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Dimension Size { get; set; }

        [DataMember]
        public Room[] Rooms { get; set; }

        [DataMember]
        public Hall[] Halls { get; set; }
                
        #endregion
    }
}
