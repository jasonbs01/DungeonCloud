using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonCommon
{
    [DataContract]
    public class Dimension
    {
        public Dimension(int length, int width)
        {
            Length = length;
            Width = width;
        }
        
        [DataMember]
        public int Length { get; set; }

        [DataMember]
        public int Width { get; set; }
    }
}
