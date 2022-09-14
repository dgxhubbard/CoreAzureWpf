using System;
using System.Collections.Generic;
using System.Linq;


using Microsoft.Identity.Client;


namespace CoreModel.Interfaces
{
    public interface IAzureAdCredentials
    {
        #region Properties


        IPublicClientApplication PublicClientApp
        { get; }

        public string AccessToken
        { get;  }


        #endregion

        #region Methods


        #endregion

    }
}
