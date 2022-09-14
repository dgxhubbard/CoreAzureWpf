using System;
using System.Collections.Generic;
using System.Linq;


using Microsoft.EntityFrameworkCore;


namespace CoreModel
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    // NOTE: This context is only used to create or upgrade a postgre database
    //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////



    public partial class PostgreGtContext : GtContext
    {
        public PostgreGtContext ()
        {
            SupportsSchema = true;
        }

        public PostgreGtContext ( DbContextOptions<PostgreGtContext> options ) :
            base ( options )
        { }


        public PostgreGtContext ( DbContextOptions<PostgreGtContext> options, bool supportsSchema ) :
            base ( options )
        {
            SupportsSchema = supportsSchema;
        }

    }
}
