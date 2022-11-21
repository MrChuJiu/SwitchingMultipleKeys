using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingMultipleKeys
{
    public class MultipleKeysOptions {

        public List<IMultipleKeyEntity> Keys { get; }

        public MultipleKeysOptions()
        {
            Keys = new List<IMultipleKeyEntity>();
        }
    }


    public enum LifeCycle
    {
        Day, 
        Week, 
        Month, 
        Year
    }

}
