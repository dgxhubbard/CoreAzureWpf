using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using CommonServiceLocator;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite;
using Npgsql;

using CoreBase;
using CoreBase.Constants;
using CoreBase.Enums;
using CoreBase.Interfaces;
using CoreBase.Utilities;





namespace CoreModel.Utilities
{

	[Export ( typeof ( IDbUtils ) )]
	[PartCreationPolicy ( System.ComponentModel.Composition.CreationPolicy.Shared )]
	public class DbUtils : IDbUtils
	{
		#region Constants

		public const string DataDirectoryKey = "|DataDirectory|";
		public const string DataDirectory = "DataDirectory";

		public const bool TestIsCurrentVersion = true;
		public const bool DontTestIsCurrentVersion = false;

		public const string EFMigrationsTable = "__EFMigrationsHistory";


		public Version NullVersion
		{
			get { return new Version ( 0, 0, 0, 0 ); }
		}


		#endregion


		#region Constructors

		public DbUtils ()
		{
		}


		#endregion


		#region Properties



		object _syncLock = new object ();
		private object SyncLock
		{
			get
			{
				return _syncLock;
			}
		}

		#endregion

		#region Methods

		public DatabaseType GetDatabaseType ( string connStr )
		{
			if ( string.IsNullOrEmpty ( connStr ) )
				return DatabaseType.Unknown;

			if ( connStr.Contains ( "Jet OLEDB" ) )
				return DatabaseType.MsAccess;
			else if ( connStr.Contains ( "Initial Catalog" ) )
				return DatabaseType.MsSql;
			else if ( connStr.Contains ( "Data Source" ) )
				return DatabaseType.SQLite;
			else if ( connStr.Contains ( "Database" ) || connStr.Contains ( "Host" ) )
				return DatabaseType.Postgre;


			return DatabaseType.Unknown;
		}

		public DbConnectionStringBuilder GetConnectionStringBuilder ( string connectionStr )
		{
			DbConnectionStringBuilder bldr = null;

			if ( string.IsNullOrWhiteSpace ( connectionStr ) )
				throw new ArgumentNullException ( "Connection string is not valid" );

			var databaseType = GtContextFactory.GetDatabaseType ( connectionStr );

			try
			{
				if ( databaseType == DatabaseType.MsSql )
				{
					var sqlBldr = new SqlConnectionStringBuilder ( connectionStr );
					if ( sqlBldr == null )
						throw new NullReferenceException ( "Could not create connection string builder" );


					bldr = sqlBldr as DbConnectionStringBuilder;

				}
				else if ( databaseType == DatabaseType.SQLite )
				{
					var sqliteBldr = new SqliteConnectionStringBuilder ( connectionStr );
					if ( sqliteBldr == null )
						throw new NullReferenceException ( "Could not create connection string builder" );

					bldr = sqliteBldr as DbConnectionStringBuilder;

				}
				else if ( databaseType == DatabaseType.Postgre )
				{
					var pgBldr = new NpgsqlConnectionStringBuilder ( connectionStr );
					if ( pgBldr == null )
						throw new NullReferenceException ( "Could not create connection string builder" );

					bldr = pgBldr as DbConnectionStringBuilder;

				}


			}
			catch ( Exception ex )
			{
				LogProvider.Logger.LogException ( "Error loading connection string settings", ex );
			}

			if ( bldr == null )
				throw new NullReferenceException ( "Failted to get connection string builder" );

			return bldr;
		}


		public ConnectionStringSettings Save ( ConnectionStringSettings connStrSettings )
		{
			ConnectionStringSettings newConnStrSettings = null;

			if ( connStrSettings == null )
				throw new ArgumentNullException ( "Connection settings is not valid" );

			var connectionStr = connStrSettings.ConnectionString;
			var databaseType = GetDatabaseType ( connectionStr );
			var bldr = GetConnectionStringBuilder ( connectionStr );

			try
			{
				// copy settings
				newConnStrSettings = new ConnectionStringSettings ();
				if ( newConnStrSettings == null )
					throw new NullReferenceException ( "Failed to create connection string settings" );

				newConnStrSettings.Name = connStrSettings.Name;
				newConnStrSettings.ConnectionString = connStrSettings.ConnectionString;
				newConnStrSettings.ProviderName = connStrSettings.ProviderName;

				if ( databaseType == DatabaseType.MsSql )
				{
					var sqlBldr = bldr as SqlConnectionStringBuilder;
					if ( sqlBldr == null )
						throw new NullReferenceException ( "Could not create connection string builder" );

					var password = sqlBldr.Password;
					if ( !string.IsNullOrEmpty ( password ) )
					{
						var encodedPassword = CryptoUtility.Encrypt ( password );
						sqlBldr.Password = encodedPassword;
					}

				}
				else if ( databaseType == DatabaseType.SQLite )
				{
					var sqliteBldr = bldr as SqliteConnectionStringBuilder;
					if ( sqliteBldr == null )
						throw new NullReferenceException ( "Could not create connection string builder" );

					var password = sqliteBldr.Password;
					if ( !string.IsNullOrEmpty ( password ) )
					{
						var encodedPassword = CryptoUtility.Encrypt ( password );
						sqliteBldr.Password = encodedPassword;
					}
				}
				else if ( databaseType == DatabaseType.Postgre )
				{
					var npgBldr = bldr as NpgsqlConnectionStringBuilder;
					if ( npgBldr == null )
						throw new NullReferenceException ( "Could not create connection string builder" );

					var password = npgBldr.Password;
					if ( !string.IsNullOrEmpty ( password ) )
					{
						var encodedPassword = CryptoUtility.Encrypt ( password );
						npgBldr.Password = encodedPassword;
					}
				}

				if ( bldr != null )
					newConnStrSettings.ConnectionString = bldr.ConnectionString;

			}
			catch ( Exception ex )
			{
				LogProvider.Logger.LogException ( "Error loading connection string settings", ex );
			}


			return newConnStrSettings;
		}


		public ConnectionStringSettings Load ( ConnectionStringSettings connStrSettings )
		{
			ConnectionStringSettings newConnStrSettings = null;

			if ( connStrSettings == null )
				throw new ArgumentNullException ( "Connection settings is not valid" );

			var connectionStr = connStrSettings.ConnectionString;
			var databaseType = GetDatabaseType ( connectionStr );
			var bldr = GetConnectionStringBuilder ( connectionStr );

			try
			{
				// copy settings
				newConnStrSettings = new ConnectionStringSettings ();
				if ( newConnStrSettings == null )
					throw new NullReferenceException ( "Failed to create connection string settings" );

				newConnStrSettings.Name = connStrSettings.Name;
				newConnStrSettings.ConnectionString = connStrSettings.ConnectionString;
				newConnStrSettings.ProviderName = connStrSettings.ProviderName;



				if ( databaseType == DatabaseType.MsSql )
				{
					var sqlBldr = bldr as SqlConnectionStringBuilder;
					if ( sqlBldr == null )
						throw new NullReferenceException ( "Could not create connection string builder" );

					var password = sqlBldr.Password;
					if ( !string.IsNullOrEmpty ( password ) )
					{
						var decodedPassword = CryptoUtility.Decrypt ( password );
						sqlBldr.Password = decodedPassword;
					}
				}
				else if ( databaseType == DatabaseType.SQLite )
				{
					var sqliteBldr = bldr as SqliteConnectionStringBuilder;
					if ( sqliteBldr == null )
						throw new NullReferenceException ( "Could not create connection string builder" );


					var password = sqliteBldr.Password;
					if ( !string.IsNullOrEmpty ( password ) )
					{
						var decodedPassword = CryptoUtility.Decrypt ( password );
						sqliteBldr.Password = decodedPassword;
					}
				}
				else if ( databaseType == DatabaseType.Postgre )
				{
					var npgBldr = bldr as NpgsqlConnectionStringBuilder;
					if ( npgBldr == null )
						throw new NullReferenceException ( "Could not create connection string builder" );


					var password = npgBldr.Password;
					if ( !string.IsNullOrEmpty ( password ) )
					{
						var decodedPassword = CryptoUtility.Decrypt ( password );
						npgBldr.Password = decodedPassword;
					}
				}

				if ( bldr != null )
					newConnStrSettings.ConnectionString = bldr.ConnectionString;

			}
			catch ( Exception ex )
			{
				LogProvider.Logger.LogException ( "Error loading connection string settings", ex );
				throw;
			}

			return newConnStrSettings;
		}


		#endregion



	}
}
