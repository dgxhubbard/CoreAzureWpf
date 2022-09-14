using System;
using System.Collections.Generic;
using System.Linq;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


using CoreBase.Enums;

namespace CoreModel
{
    public class SQLiteContextFactory : IDesignTimeDbContextFactory<SqliteGtContext>
    {
        public SqliteGtContext CreateDbContext ( string [] args )
        {

            var optionsBuilder = new DbContextOptionsBuilder<SqliteGtContext> ();
            optionsBuilder.UseSqlite ( GtContextFactory.MigrateSQLiteConnString, b => b.MigrationsAssembly ( "CoreModel" ) );

            return new SqliteGtContext ( optionsBuilder.Options );
        }



        public static SqliteGtContext Create ( string connStr )
        {

            if ( string.IsNullOrEmpty ( connStr ) )
                throw new InvalidOperationException ( "connection string is required" );

            var optionsBuilder = new DbContextOptionsBuilder<SqliteGtContext> ();
            if ( optionsBuilder == null )
                throw new NullReferenceException ( "Failed to create db context options builder" );


            optionsBuilder.UseSqlite ( connStr, b => b.MigrationsAssembly ( "CoreModel" ) );

            var supportsSchema = true;
            var context = new SqliteGtContext ( optionsBuilder.Options, supportsSchema ) { DatabaseType = DatabaseType.SQLite };
            if ( context == null )
                throw new NullReferenceException ( "Failed to create context" );

            // disable change tracking
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;


            return context;
        }


    }


}
