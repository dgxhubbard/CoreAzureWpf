using System;
using System.Collections.Generic;
using System.Linq;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

using CoreBase;

namespace CoreModel
{
    public partial class MsSqlGtContext : GtContext
    {
        public MsSqlGtContext ()
        {
            SupportsSchema = true;
        }

        public MsSqlGtContext ( DbContextOptions<MsSqlGtContext> options ) :
            base ( options )
        { }

        public MsSqlGtContext ( DbContextOptions<MsSqlGtContext> options, bool supportsSchema ) :
            base ( options )
        {
            SupportsSchema = supportsSchema;
        }







    }
}
