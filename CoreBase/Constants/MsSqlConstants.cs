using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBase.Constants
{
    public static class MsSqlConstants
    {
        #region Constants

        public const string DefaultDatabaseName = "GAGETRAK";
        public const string DefaultSchemaName = "GAGETRAK";
        public const string DefaultLogin = "GTLOGIN";
        public const string DefaultPassword = "PASSword1";
        public const string DefaultUsername = "GTUSER";
        public const string SystemSchemaName = "dbo";

        public const string SqlMasterDatabase = "master";
        public const string SqlLoginNameQuery = @"select count ( name ) from master..syslogins where name = @loginname";
        public const string SqlLoginNameParameter = "loginname";

        public const int BadCredentials = 18456;

        public const string SqlValidNamePattern = @"^[a-zA-Z_@#][a-zA-Z0-9@#$_]+$";

        #endregion

        #region Constructors

        static MsSqlConstants ()
        {
        }

        #endregion

        #region Properties

        public static string DefaultServerName
        {

            get
            {
                string serverName = Environment.MachineName;

                if ( string.IsNullOrWhiteSpace( serverName ) )
                    serverName = "localhost";

                return serverName;
            }
        }

        #endregion
    }
}
