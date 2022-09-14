using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


using CommonServiceLocator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;



using CoreModel.Interfaces;



namespace CoreModel
{
    public class AzureAdDbConnectorInterceptor : DbConnectionInterceptor
    {

        static AzureAdDbConnectorInterceptor ()
        {
            /*
            var options = new DefaultAzureCredentialOptions
            {
                ExcludeEnvironmentCredential = false,
                ExcludeManagedIdentityCredential = false,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeVisualStudioCredential = true,
                ExcludeVisualStudioCodeCredential = true,
                ExcludeAzureCliCredential = true,
                ExcludeInteractiveBrowserCredential = false,
                ExcludeAzurePowerShellCredential = true
            };

            var credential = new DefaultAzureCredential ( options );

            if ( credential == null )
                throw new NullReferenceException ( "FAILED to create credential" );

            _credential = credential;
            */
        }

        /*
        // Refer:
        // https://docs.microsoft.com/azure/active-directory/managed-identities-azure-resources/services-support-managed-identities#azure-sql
        // https://devblogs.microsoft.com/azure-sdk/azure-identity-with-sql-graph-ef/

        private static readonly string [] _azureSqlScopes =
            new []
                {
                "https://database.windows.net//.default"
                };


        private static readonly DefaultAzureCredential _credential;
        */

        public override InterceptionResult ConnectionOpening (
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result )
        {

            var sqlConnection = ( Microsoft.Data.SqlClient.SqlConnection ) connection;
            if ( DoesConnectionNeedAccessToken ( sqlConnection ) )
            {
                /*
                var tokenRequestContext = new TokenRequestContext ( _azureSqlScopes );
                var token = _credential.GetToken ( tokenRequestContext, default );
                */

                var credentials = ServiceLocator.Current.GetInstance<IAzureAdCredentials> ();
                if ( credentials == null )
                    throw new NullReferenceException ( "FAILED to get credentials" );

                sqlConnection.AccessToken = credentials.AccessToken;
            }

            return base.ConnectionOpening ( connection, eventData, result );
        }

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync (
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default )
        {
            var sqlConnection = ( Microsoft.Data.SqlClient.SqlConnection ) connection;
            if ( DoesConnectionNeedAccessToken ( sqlConnection ) )
            {
                /*
                var tokenRequestContext = new TokenRequestContext ( _azureSqlScopes );
                var token = await _credential.GetTokenAsync ( tokenRequestContext, cancellationToken );
                */

                var credentials = ServiceLocator.Current.GetInstance<IAzureAdCredentials> ();
                if ( credentials == null )
                    throw new NullReferenceException ( "FAILED to get credentials" );

                sqlConnection.AccessToken = credentials.AccessToken;
            }

            return await base.ConnectionOpeningAsync ( connection, eventData, result, cancellationToken );
        }

        private static bool DoesConnectionNeedAccessToken ( Microsoft.Data.SqlClient.SqlConnection connection )
        {
            var connectionStringBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder ( connection.ConnectionString );

            var dataSource = connectionStringBuilder.DataSource;

            var contains = false;

            if ( dataSource.Contains ( MsSqlConstants.AzureUrl, StringComparison.OrdinalIgnoreCase ) )
                contains = true;

            return contains;
        }


    }
}
