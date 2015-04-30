using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonCommon
{
    [DataContract]
    public class Hall
    {
        public Hall(string name)
        {
            Name = name;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string StartRoomName { get; set; }

        [DataMember]
        public string EndRoomName { get; set; }

        [DataMember]
        public Position[] Segments { get; set; }
    }
}
