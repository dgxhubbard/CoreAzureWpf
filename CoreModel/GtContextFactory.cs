using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using CommonServiceLocator;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

using CoreBase.Enums;
using CoreBase.Interfaces;
using CoreBase.Utilities;

namespace CoreModel
{
    public static class GtContextFactory
    {
        #region Constants


        public const string DbPath = @"C:\Repository\CoreMigrationMs\bin\Debug\Blog.db3";

        public const string MsSqlConnString = @"Data Source=localhost;Initial Catalog=BookStore;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=cccccc";
        public const string SqliteConnString = @"Data Source=C:\Repository\CoreMigrationMs\bin\Debug\BookStore.db3";
        public const string PostgreConnString = @"User Id=postgres;Password=cccccc;Host=localhost;Port=5432;Database=BookStore";


        public const string MigrateMsSqlConnString = @"Data Source=localhost;Initial Catalog=;User ID=sa;Password=cccccc";
        public const string MigratePostgreConnString = @"User ID=postgres;Password=cccccc;Host=localhost;Port=5432;Database=";
        public const string MigrateSQLiteConnString = @"Data Source=;";



        public const string MachineConfigDefaultConnection = "LocalSqlServer";

        #endregion

        #region Constructors

        static GtContextFactory ()
        {
            Configuration = LoadConfiguration ();

            var connStringSettingsList = Configuration.ConnectionStrings.ConnectionStrings;


            // create connection string settings list but dont add 
            // connection string added by machine config so it is never shown
            var tmpList = new List<ConnectionStringSettings> ();
            for ( int i = 0; i < connStringSettingsList.Count; i++ )
            {
                var connStrSettings = connStringSettingsList [ i ];

                if ( connStrSettings.Name != MachineConfigDefaultConnection )
                {
                    var loadConnStrSettings = DbUtils.Load ( connStrSettings );
                    tmpList.Add ( loadConnStrSettings );
                }
            }

            var connStringSettings = tmpList;

            var connString = connStringSettings[0];

            ConnectionString = connString.ConnectionString;

        }


        #endregion

        #region Properties


        private static IDbUtils _dbUtils;
        public static IDbUtils DbUtils
        {
            get
            {
                if ( _dbUtils == null )
                {
                    _dbUtils = ServiceLocator.Current.GetInstance<IDbUtils> ();

                    if ( _dbUtils == null )
                        throw new NullReferenceException ( "Failed to get db utils" );

                }

                return _dbUtils;


            }
        }

        #endregion

        #region Configuration

        public static string ConnectionString
        { get; set; }

        public static Configuration Configuration
        { get; set; }

        public static string ConfigPath
        {
            get
            {
                var path = Path.Combine ( PathUtilities.BinPath, "GlobalAppSettings.config" );

                return path;
            }
        }

        public static Configuration LoadConfiguration ()
        {
            string path = null;

            try
            {
                var configPath = ConfigPath;
                if ( !string.IsNullOrWhiteSpace ( configPath ) && File.Exists ( configPath ) )
                    path = configPath;
                else
                {
                    var msg = "Could not find config path " + configPath;
                    LogProvider.Logger.LogError ( msg );
                    throw new ApplicationException ( msg );
                }

                var fileMap = new ExeConfigurationFileMap () { ExeConfigFilename = path };

                var config =
                    ConfigurationManager.OpenMappedExeConfiguration (
                        fileMap,
                        ConfigurationUserLevel.None );

                if ( config == null )
                    LogProvider.Logger.LogError ( "ConfigFileReadError" );

                return config;
            }
            catch ( Exception ex )
            {
                if ( ex.InnerException != null )
                    ex = ex.InnerException;

                LogProvider.Logger.LogException ( "Error loading app settings configuration file", ex );

                return null;
            }

        }



        #endregion

        #region Create Methods

        public static DatabaseType GetDatabaseType ( string connStr )
        {
            if ( string.IsNullOrEmpty ( connStr ) )
                return DatabaseType.Unknown;

            if ( connStr.Contains ( "Initial Catalog" ) )
                return DatabaseType.MsSql;
            else if ( connStr.Contains ( "Data Source" ) )
                return DatabaseType.SQLite;

            return DatabaseType.Unknown;
        }


        public static GtContext Create ( TargetDatabase target )
        {

            return Create ( ConnectionString );

        }

        public static GtContext Create ( string connStr )
        {
            if ( string.IsNullOrEmpty ( connStr ) )
                throw new InvalidOperationException ( "connection string is required" );

            var optionsBuilder = new DbContextOptionsBuilder<GtContext> ();
            if ( optionsBuilder == null )
                throw new NullReferenceException ( "Failed to create db context options builder" );

            var supportsSchema = true;
            var databaseType = GetDatabaseType ( connStr );

            switch ( databaseType )
            {

                case DatabaseType.MsSql:
                    {
                        optionsBuilder.UseSqlServer ( connStr, b => b.MigrationsAssembly ( "CoreModel" ) );
                        break;

                    }

                case DatabaseType.SQLite:
                    {
                        supportsSchema = false;
                        optionsBuilder.UseSqlite ( connStr, b => b.MigrationsAssembly ( "CoreModel" ) );

                        break;

                    }

                case DatabaseType.Postgre:
                    {
                        supportsSchema = false;
                        optionsBuilder.UseNpgsql ( connStr, b => b.MigrationsAssembly ( "CoreModel" ) );

                        break;

                    }


            }

            var context = new GtContext ( optionsBuilder.Options, supportsSchema ) { DatabaseType = databaseType };
            if ( context == null )
                throw new NullReferenceException ( "Failed to create context" );


            return context;
        }





        #endregion

    }
}
