using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;



namespace CoreBase.Enums
{
    [Serializable ()]
    [DataContract]
    
    public enum LogVerbosity
    {
        [EnumMember]
        Info,
        [EnumMember]
        Warning,
        [EnumMember]
        Error
    }
}
