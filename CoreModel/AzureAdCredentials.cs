using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;


using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Broker;


using CoreModel.Interfaces;


namespace CoreModel
{
    [Export ( typeof ( IAzureAdCredentials ) )]
    [PartCreationPolicy ( System.ComponentModel.Composition.CreationPolicy.Shared )]
    public class AzureAdCredentials : IAzureAdCredentials
    {

        #region 

        // Below are the clientId (Application Id) of your app registration and the tenant information. 
        // You have to replace:
        // - the content of ClientID with the Application Id for your app registration
        // - The content of Tenant by the information about the accounts allowed to sign-in in your application:
        //   - For Work or School account in your org, use your tenant ID, or domain
        //   - for any Work or School accounts, use organizations
        //   - for any Work or School accounts, or Microsoft personal account, use common
        //   - for Microsoft Personal account, use consumers

        // multi tenant
        //private static string ClientId = "7647757c-260c-4989-b472-68c30fe882aa";
        //private static string Tenant = "common";


        // single or mutil tenant
        // if multi tenant set Tenant to common
        private static string ClientId = "ddc27399-ecf0-4f86-a926-aeecf45391cf";
        private static string Tenant = "79f3db43-b504-4ffd-ae1c-8325ff5b2156";




        private static string Instance = "https://login.microsoftonline.com/";

        private const string RedirectUri = "http://localhost";

        #endregion

        #region Constructors

        public AzureAdCredentials ()
        {
            Initialize ();
        }


        #endregion

        #region Properties


        IPublicClientApplication _clientApp;

        public IPublicClientApplication PublicClientApp
        {
            get { return _clientApp; }
        }

        public string AccessToken
        { get; private set; }


        public Dictionary<string,string> TokenInfo
        { get; private set; }


        public DateTime IssuedAt
        { get; private set; }


        public DateTime NotBefore
        { get; private set; }

        public DateTime TokenExpires
        { get; private set; }


        private static readonly string [] _azureSqlScopes =
                 new []
                     {
                "https://database.windows.net//.default"
                     };


        #endregion

        #region Methods

        public void Initialize ()
        {
            var useWam = true;
            var handle = IntPtr.Zero;

            Window mainWindow = null;
            System.Windows.Application.Current.Dispatcher.Invoke ( 
                new Action ( () => 
                {
                    mainWindow = System.Windows.Application.Current.MainWindow;
                    if ( mainWindow != null) 
                        handle = new WindowInteropHelper ( ( Window ) mainWindow ).EnsureHandle ();

                } ) );


            if ( handle == IntPtr.Zero )
                throw new NullReferenceException ( "FAILED to get main window" );


            var builder = PublicClientApplicationBuilder.Create ( ClientId )
                .WithAuthority ( $"{Instance}{Tenant}" )
                .WithRedirectUri ( RedirectUri );
                //.WithDefaultRedirectUri ();

            if ( useWam )
            {
                builder.WithBrokerPreview ( true );  // Requires redirect URI "ms-appx-web://microsoft.aad.brokerplugin/{client_id}" in app registration
            }

            _clientApp = builder.Build ();
            if ( _clientApp == null )
                throw new NullReferenceException ( "FAILED to create client app builder" );


            AzureTokenCacheHelper.EnableSerialization ( _clientApp.UserTokenCache );

            IAccount firstAccount;

            var accountResult = PublicClientApp.GetAccountsAsync ();
            if ( accountResult == null )
                throw new NullReferenceException ( "FAILED to get accounts" );

            accountResult.Wait ();

            firstAccount = accountResult.Result.FirstOrDefault ();


            if ( firstAccount == null )
            {
                try
                {
                    var authResult = PublicClientApp.AcquireTokenInteractive ( _azureSqlScopes )
                        .WithAccount ( firstAccount )
                        .WithParentActivityOrWindow ( handle )
                        .WithPrompt ( Prompt.SelectAccount )
                        .ExecuteAsync ();

                    authResult.Wait ();


                    AccessToken = authResult.Result.AccessToken;
                }
                catch ( MsalException msalex )
                {
                    Debug.WriteLine ( msalex.Message );
                }

            }
            else
            {

                try
                {
                    var authSilentResult = PublicClientApp.AcquireTokenSilent ( _azureSqlScopes, firstAccount ).ExecuteAsync ();

                    authSilentResult.Wait ();

                    AccessToken = authSilentResult.Result.AccessToken;
                }
                catch ( MsalUiRequiredException ex )

                //try
                {
                    try
                    {
                        var authResult = PublicClientApp.AcquireTokenInteractive ( _azureSqlScopes )
                            .WithAccount ( firstAccount )
                            .WithParentActivityOrWindow ( handle )
                            .WithPrompt ( Prompt.SelectAccount )
                            .ExecuteAsync ();

                        authResult.Wait ();


                        AccessToken = authResult.Result.AccessToken;
                    }
                    catch ( MsalException msalex )
                    {
                        Debug.WriteLine ( msalex.Message );
                    }
                }
                catch ( Exception ex )
                {
                    Debug.WriteLine ( ex.Message );
                    return;
                }

                if ( AccessToken != null )
                {
                    TokenInfo = new Dictionary<string, string> ();

                    var handler = new JwtSecurityTokenHandler ();
                    var jwtSecurityToken = handler.ReadJwtToken ( AccessToken );

                    var claims = jwtSecurityToken.Claims.ToList ();

                    foreach ( var claim in claims )
                    {
                        if ( !TokenInfo.ContainsKey ( claim.Type) )
                            TokenInfo.Add ( claim.Type, claim.Value );
                    }

                    var tokenExp = claims.First ( claim => claim.Type.Equals ( "exp" ) ).Value;
                    var tokenTicks = long.Parse ( tokenExp );

                    TokenExpires = DateTimeOffset.FromUnixTimeSeconds ( tokenTicks ).UtcDateTime;


                    tokenExp = claims.First ( claim => claim.Type.Equals ( "nbf" ) ).Value;
                    tokenTicks = long.Parse ( tokenExp );

                    NotBefore = DateTimeOffset.FromUnixTimeSeconds ( tokenTicks ).UtcDateTime;


                    tokenExp = claims.First ( claim => claim.Type.Equals ( "iat" ) ).Value;
                    tokenTicks = long.Parse ( tokenExp );

                    IssuedAt = DateTimeOffset.FromUnixTimeSeconds ( tokenTicks ).UtcDateTime;

                }
            }
        }



    




        #endregion
    }
}
