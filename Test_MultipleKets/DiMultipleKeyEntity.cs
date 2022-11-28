using SwitchingMultipleKeys;

namespace Test_MultipleKets
{
    public class DiMultipleKeyEntity: MultipleKeyEntity
    {


        public string KeyId { get; set; }
         
        public string Password { get; set; }

        public DiMultipleKeyEntity(LifeCycle lifeCycle, int maximum = 40) : base(lifeCycle, maximum)
        {

        }
    }
}
