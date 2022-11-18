using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingMultipleKeys
{
    //[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class MultipleKeyAttribute: Attribute
    {
        public int Maximum { get; set; } = 100;

        public MultipleKeyAttribute()
        {

        }

        public MultipleKeyAttribute(int maximum)
        {
            Maximum = maximum;
        }

    }
}
