using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

using CommonServiceLocator;

using CoreBase;
using CoreBase.Interfaces;

namespace CoreModel
{
    internal class LogProvider : LogProviderBase
    {
        static LogProvider ()
        {
            try
            {
                Logger = LoggerFactory.CreateLogger( "GtBase" );
            }
            catch ( Exception ex )
            {
                Debug.WriteLine( "LogProvider(GtBase) exception" + System.Environment.NewLine + ex.Message );
            }
        }

        public static IGtLogger Logger
        { get; private set; }

    }
}
