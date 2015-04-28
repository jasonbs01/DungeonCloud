using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService
{
    internal class IdGenerator : IIdGenerator
    {
        Dictionary<string, int> _usedIds;
        
        public IdGenerator()
        {
            _usedIds = new Dictionary<string,int>();
        }

        public string GetNextId(string name)
        {
            string newId;
            int usages; 
            if(_usedIds.ContainsKey(name) == true)
            {
                usages = ++_usedIds[name];                
            }
            else
            {
                usages = 1;
                _usedIds.Add(name, usages);
            }
            newId = string.Format("{0}_{1}", name, usages);

            return newId;
        }
    }
}
