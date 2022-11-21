using SwitchingMultipleKeys;

namespace Test_MultipleKets
{
    [MultipleKey(20)]
    public class SMZDMMultipleKeyEntity: IMultipleKeyEntity
    {
        public string KeyId { get; set; }
         
        public string HttpUrl { get; set; }
    }
}
