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
        public Position()
        {
            Row = -1;
            Column = -1;

        }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Position(Position otherPosition)
        {
            Row = otherPosition.Row;
            Column = otherPosition.Column;
        }

        [DataMember]
        public int Row { get; set; }
        
        [DataMember]
        public int Column { get; set; }
    }
}
