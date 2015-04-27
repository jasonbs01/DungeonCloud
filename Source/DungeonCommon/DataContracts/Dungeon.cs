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

        public Dungeon(string id)
        {            
            Id = id;
        }

        #endregion

        #region Public Properties

        [DataMember]
        public string Id { get; set; }

        #endregion
    }
}
