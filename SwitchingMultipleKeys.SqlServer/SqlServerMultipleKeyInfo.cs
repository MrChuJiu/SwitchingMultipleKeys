using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingMultipleKeys.SqlServer
{
    public class SqlServerMultipleKeyInfo
    {
        public int Id { get; set; }

        public string KeyName { get; set; }

        public int Maximum { get; set; }

        public int ResidueDegree { get; set; }

        public MultipleKeyEntity Data  { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
