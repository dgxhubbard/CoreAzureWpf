
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Common;

using CoreBase.Enums;

namespace CoreBase.Interfaces
{
    public interface IDbUtils
    {
        ConnectionStringSettings Load ( ConnectionStringSettings connStrSettings );
        ConnectionStringSettings Save ( ConnectionStringSettings connStrSettings );
    }
}