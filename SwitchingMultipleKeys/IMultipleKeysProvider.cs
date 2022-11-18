using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingMultipleKeys
{
    public interface IMultipleKeysProvider<T> where T : IMultipleKeyEntity
    {
        T GetMultipleKeys();
    }
}
