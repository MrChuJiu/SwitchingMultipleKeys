﻿using SwitchingMultipleKeys;

namespace Test_MultipleKets
{
    [MultipleKey(30)]
    public class DiMultipleKeyEntity: IMultipleKeyEntity
    {
        public string KeyId { get; set; }
         
        public string Password { get; set; }
    }
}
