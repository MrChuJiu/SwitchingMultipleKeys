using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingMultipleKeys
{
    public class MultipleKeysOptions {

        public List<MultipleKeyEntity> Keys { get; } 

        public MultipleKeysOptions()
        {
            Keys = new List<MultipleKeyEntity>();
        }
    }

    public enum LifeCycle
    {
        Day, 
        //Week, 
        Month, 
        Year,
        NotRepeat
    }

}
