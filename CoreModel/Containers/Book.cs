using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Microsoft.EntityFrameworkCore;


using CoreModel.Config;

namespace CoreModel.Containers
{
    [Table ( "Books" )]
    [EntityTypeConfiguration ( typeof ( BookConfiguration ) )]
    public partial class Book
    {
        private int _book_RID;

        [Key]
        [DatabaseGenerated ( DatabaseGeneratedOption.Identity )]
        public virtual int Book_RID
        {
            get { return _book_RID; }
            set { SetField ( ref _book_RID, value, nameof ( Book_RID ) ); }

        }


        private string _title;

        public virtual string Title
        {
            get { return _title; }
            set { SetField ( ref _title, value, nameof ( Title ) ); }
        }



        int _author_RID_FK;
        [System.Runtime.Serialization.DataMember]

        public virtual int Author_RID_FK
        {
            get
            {
                return _author_RID_FK;
            }
            set
            {
                if ( _author_RID_FK != value )
                {
                    SetField ( ref _author_RID_FK, value, "Author_RID_FK" );
                }
            }
        }

        private Author _author;



        [System.Runtime.Serialization.DataMember]
        public virtual Author Author
        {
            get
            {
                return _author;
            }
            set
            {
                SetField ( ref _author, value, "Author" );
            }
        }




    }
}
