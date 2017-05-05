using System;

namespace Shinobytes.XzaarScript
{
    [Serializable, Flags]
    public enum XzaarMemberTypes
    {
        EntryPoint    = 0x001,
        Constructor   = 0x002,        
        Event         = 0x004,        
        Field         = 0x008,
        Property      = 0x010,
        TypeInfo      = 0x020,
        Method        = 0x040,
        Custom        = 0x080,        
        NestedType    = 0x100,
        All           =  Constructor | Event | Field | Method | Property | TypeInfo | NestedType
    }
}