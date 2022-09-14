using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBase.Interfaces
{
    public interface IGtLoggerFactory
    {

        IGtLogger CreateLogger ( string categoryName );

    }
}
