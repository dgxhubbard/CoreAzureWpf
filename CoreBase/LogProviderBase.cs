using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using CommonServiceLocator;

using CoreBase.Interfaces;

namespace CoreBase
{
    public class LogProviderBase
    {
        #region Constructors

        static LogProviderBase ()
        {

            LoggerFactory = ServiceLocator.Current.GetInstance<IGtLoggerFactory> ();
            if ( LoggerFactory == null )
                throw new NullReferenceException ( "FAILED to get logger factory" );

        }



        #endregion


        #region Properties

        protected static IGtLoggerFactory LoggerFactory
        { get; private set; }

        #endregion

        #region Methods


        #endregion
    }
}
