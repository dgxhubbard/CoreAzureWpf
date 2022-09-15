using System;
using System.Collections.Generic;
using System.Linq;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

using CoreBase;
using CoreBase.Enums;
using CoreModel.Containers;

namespace CoreModel
{
    public partial class GtContext : DbContext
    {

        public GtContext ()
        {}

        public GtContext ( DbContextOptions<GtContext> options ) :
            base ( options )
        {
            OnCreated ();
        }


        public GtContext ( DbContextOptions<GtContext> options, bool supportsSchema ) :
            base ( options )
        {
            SupportsSchema = supportsSchema;
            OnCreated ();
        }

        protected GtContext ( DbContextOptions options ) : 
            base ( options )
        {
        }


        public bool SupportsSchema
        { get; set; }


        public DatabaseType DatabaseType
        { get; set; }


        partial void OnCreated ();


        protected override void OnConfiguring ( DbContextOptionsBuilder optionsBuilder )
        {
            CustomizeConfiguration ( ref optionsBuilder );

            // Refer to date time handling for postgres
            // https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty
            if ( DatabaseType == DatabaseType.Postgre )
                AppContext.SetSwitch ( "Npgsql.EnableLegacyTimestampBehavior", true );
            else if ( DatabaseType == DatabaseType.MsSql )
            {
                optionsBuilder.AddInterceptors ( new AzureAdDbConnectorInterceptor () );
            }



            base.OnConfiguring ( optionsBuilder );
        }

        partial void CustomizeConfiguration ( ref DbContextOptionsBuilder optionsBuilder );

        partial void OnConfiguring ( ref DbContextOptionsBuilder optionsBuilder );

        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            if ( SupportsSchema )
                modelBuilder.HasDefaultSchema ( "FOO" );


            modelBuilder.Ignore<BaseModel> ();

        
            
            modelBuilder.Entity<Author> ()
                        .HasData 
                        (
                             new Author { Author_RID = 1, FirstName = "William", LastName = "Shakespere" },
                             new Author { Author_RID = 2, FirstName = "Issac", LastName = "Asimov" },
                             new Author { Author_RID = 3, FirstName = "Jules", LastName = "Verne" },
                             new Author { Author_RID = 4, FirstName = "Wilson", LastName = "Rawls" }
                         );




            modelBuilder.Entity<Book> ()
                        .HasData
                        (
                             new Book { Book_RID = 1, Title = "A Midsummer Nights Dread", Author_RID_FK = 1 },
                             new Book { Book_RID = 2, Title = "The Merchant of Venice", Author_RID_FK = 1 },
                             new Book { Book_RID = 3, Title = "King Lear", Author_RID_FK = 1 },

                             new Book { Book_RID = 4, Title = "I Robot", Author_RID_FK = 2 },
                             new Book { Book_RID = 5, Title = "Foundation", Author_RID_FK = 2 },

                             new Book { Book_RID = 6, Title = "Journey to the Center of the Earth", Author_RID_FK = 3 },
                             new Book { Book_RID = 7, Title = "Around the World in 80 Days", Author_RID_FK = 3 },
                             new Book { Book_RID = 8, Title = "The Mysterious Island", Author_RID_FK = 3 },

                             new Book { Book_RID = 9, Title = "Where the Red Fern Grows", Author_RID_FK = 4 }
                         );







        }

        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Books { get; set; }




    }
}
