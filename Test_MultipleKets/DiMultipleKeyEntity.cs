using SwitchingMultipleKeys;

namespace Test_MultipleKets
{
    [MultipleKey(40)]
    public class DiMultipleKeyEntity: IMultipleKeyEntity
    {
        public string KeyId { get; set; }
         
        public string PassWord { get; set; }
    }
}
