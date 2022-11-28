using SwitchingMultipleKeys;

namespace Test_MultipleKets
{
    public class SMZDMMultipleKeyEntity: MultipleKeyEntity
    {
        public string KeyId { get; set; }
         
        public string HttpUrl { get; set; }

        public SMZDMMultipleKeyEntity(LifeCycle lifeCycle, int maximum = 40) : base(lifeCycle, maximum)
        {
        }
    }
}
