using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBase.Constants
{
    public static class SQLiteConstants
    {

        #region Constants


        public const string ConnectionStrDefaults = "; FailIfMissing=False; Version=3; Pooling=True; Max Pool Size=100; BinaryGUID=False; Journal Mode=Off; Synchronous=Full;";

        public const string BaseDatabaseName = "GtData";
        public const string DatabaseFileExt = ".db3";
        public const string Separator = "_";
        public const string DateFunction = "Date";
        public const string BlankTime = "00:00:00.000000";


        public const string DefaultPassword = "we092uRc1SLKr";
        public const string DefaultUsername = "Admin";


        #endregion


    }
}
