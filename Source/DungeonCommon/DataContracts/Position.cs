using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonCommon
{
    [DataContract]
    public class Position
    {
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        [DataMember]
        public int Row { get; set; }
        
        [DataMember]
        public int Column { get; set; }
    }
}
