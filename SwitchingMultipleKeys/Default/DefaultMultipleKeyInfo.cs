using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingMultipleKeys
{
    public class DefaultMultipleKeyInfo
    {
        public int Maximum { get; set; } = 100;

        public int CurrentLocation { get; set; } = 0;

        public int CurrentKeysIndex { get; set; } = 0;

        public DefaultMultipleKeyInfo(int maximum)
        {
            Maximum = maximum;
        }

        public List<IMultipleKeyEntity> Keys { get; set; }
    }
}
