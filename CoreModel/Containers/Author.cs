using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

using CoreModel.Config;

namespace CoreModel.Containers
{
    [Table ( "Authors" )]
    [EntityTypeConfiguration ( typeof ( AuthorConfiguration ) )]
    public partial class Author
    {

        private int _author_RID;

        [Key]
        [DatabaseGenerated ( DatabaseGeneratedOption.Identity )]
        public virtual int Author_RID
        {
            get { return _author_RID; }
            set { SetField ( ref _author_RID, value, nameof ( Author_RID ) ); }

        }


        private string _firstName;

        public virtual string FirstName
        {
            get { return _firstName; }
            set { SetField ( ref _firstName, value, nameof ( FirstName ) ); }
        }


        private string _lastName;

        public virtual string LastName
        {
            get { return _lastName; }
            set { SetField ( ref _lastName, value, nameof ( LastName ) ); }

        }

        private List<Book> _books;
        [JsonIgnore]
        public virtual List<Book> Books
        {
            get { return _books; }
            set { SetField ( ref _books, value, nameof ( Books ) ); }

        }
    }
}
