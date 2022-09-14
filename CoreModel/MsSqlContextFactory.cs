using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using CoreBase.Enums;

namespace CoreModel
{


   

    public class MsSqlContextFactory : IDesignTimeDbContextFactory<MsSqlGtContext>
    {
        public MsSqlGtContext CreateDbContext ( string [] args )
        {
            var optionsBuilder = new DbContextOptionsBuilder<MsSqlGtContext> ();
            optionsBuilder.UseSqlServer ( GtContextFactory.MigrateMsSqlConnString, b => b.MigrationsAssembly ( "CoreModel" ) );

            return new MsSqlGtContext ( optionsBuilder.Options );
        }



        public static MsSqlGtContext Create ( string connStr )
        {

            if ( string.IsNullOrEmpty ( connStr ) )
                throw new InvalidOperationException ( "connection string is required" );

            var optionsBuilder = new DbContextOptionsBuilder<MsSqlGtContext> ();
            if ( optionsBuilder == null )
                throw new NullReferenceException ( "Failed to create db context options builder" );


            optionsBuilder.UseSqlServer (
                connStr,
                b =>
                {
                    b.MigrationsAssembly ( "CoreModel" );
                    b.EnableRetryOnFailure (
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds ( 30 ),
                        errorNumbersToAdd: null );
                } );

            /*
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure (
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds ( 30 ),
                errorNumbersToAdd: null );
            });
            */

            var supportsSchema = true;
            var context = new MsSqlGtContext ( optionsBuilder.Options, supportsSchema ) { DatabaseType = DatabaseType.MsSql };
            if ( context == null )
                throw new NullReferenceException ( "Failed to create context" );

            // disable change tracking
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;


            return context;
        }


    }
}
