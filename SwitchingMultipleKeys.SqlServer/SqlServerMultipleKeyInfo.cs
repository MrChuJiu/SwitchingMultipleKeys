using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingMultipleKeys.SqlServer
{
    public class SqlServerMultipleKeyInfo: MultipleKeyEntity
    {
        public SqlServerMultipleKeyInfo(): base(LifeCycle.Day, 42)
        {

        }
        public SqlServerMultipleKeyInfo(LifeCycle lifeCycle, int maximum = 40) : base(lifeCycle, maximum)
        {

        }

        public int Id { get; set; }

        public string KeyName { get; set; }

        public MultipleKeyEntity Data  { get; set; }
    }
}
