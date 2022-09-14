using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;



namespace CoreBase.Enums
{
    [Serializable()]
    [DataContract]
    public enum DatabaseType : byte
    {
        [EnumMember]
        Unknown=0,
        [EnumMember]
        MsSql,
        [EnumMember]
        SQLite,
        [EnumMember]
        Postgre,
        [EnumMember]
        MsAccess,
    }
}
