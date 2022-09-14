using System;
using System.Collections.Generic;
using System.Linq;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.SqlServer;

using CoreBase;

namespace CoreModel
{
    public partial class SqliteGtContext : GtContext
    {
        public SqliteGtContext ()
        {}

        public SqliteGtContext ( DbContextOptions<SqliteGtContext> options ) :
            base ( options )
        { }


        public SqliteGtContext ( DbContextOptions<SqliteGtContext> options, bool supportsSchema ) :
            base ( options )
        {
            SupportsSchema = supportsSchema;
        }



    }
}
