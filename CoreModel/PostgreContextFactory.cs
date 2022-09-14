using System;
using System.Collections.Generic;
using System.Linq;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

using CoreBase.Enums;

namespace CoreModel
{
    public class PostgreContextFactory : IDesignTimeDbContextFactory<PostgreGtContext>
    {
        public PostgreGtContext CreateDbContext ( string [] args )
        {

            var optionsBuilder = new DbContextOptionsBuilder<PostgreGtContext> ();
            optionsBuilder.UseNpgsql ( GtContextFactory.MigratePostgreConnString, b => b.MigrationsAssembly ( "CoreModel" ) );

            return new PostgreGtContext ( optionsBuilder.Options );
        }


        public static PostgreGtContext Create ( string connStr )
        {

            if ( string.IsNullOrEmpty ( connStr ) )
                throw new InvalidOperationException ( "connection string is required" );

            var optionsBuilder = new DbContextOptionsBuilder<PostgreGtContext> ();
            if ( optionsBuilder == null )
                throw new NullReferenceException ( "Failed to create db context options builder" );


            var pgOptionsBuilder = new NpgsqlDbContextOptionsBuilder ( optionsBuilder );
            if ( pgOptionsBuilder == null )
                throw new NullReferenceException ( "Failed to create db context options builder" );


            var postgreBuilder = new NpgsqlConnectionStringBuilder ( connStr );

            //postgreBuilder.LicenseKey = GtContextFactory.PostgreKey;
            //postgreBuilder.ForceIPv4 = true;

            connStr = postgreBuilder.ToString ();

            optionsBuilder.UseNpgsql ( connStr, b => b.MigrationsAssembly ( "CoreModel" ) );

            var supportsSchema = true;
            var context = new PostgreGtContext ( optionsBuilder.Options, supportsSchema ) { DatabaseType = DatabaseType.Postgre };
            if ( context == null )
                throw new NullReferenceException ( "Failed to create context" );

            // disable change tracking
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;


            return context;
        }



    }


}
