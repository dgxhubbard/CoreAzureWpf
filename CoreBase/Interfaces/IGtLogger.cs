using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CoreBase.Interfaces
{
    public interface IGtLogger
    {
        #region Initialize Methods

        void Initialize ( string categoryName );

        #endregion

        #region Logging Methods

        void LogDebug ( string txt );
        void LogDebug ( string message, Exception loggedException );
        void LogDebug ( string txt, params object [] args );

        void LogCritical ( string txt );
        void LogCritical ( string txt, params object [] args );

        void LogError ( string txt );
        void LogError ( string txt, params object [] args );

        void LogException ( string txt );
        void LogException ( Exception ex );
        void LogException ( string txt, Exception ex );
        void LogException ( string txt, params object[] args );

        void LogInfo ( string txt );
        void LogInfo ( string txt, params object [] args );

        void LogWarning ( string txt );
        void LogWarning ( string txt, params object [] args );


        #endregion

    }

    public interface ICommonLogger : IGtLogger
    {}

}
