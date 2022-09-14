using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


using CoreModel.Containers;


namespace CoreModel.Config
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
	{
		public void Configure ( EntityTypeBuilder<Book> builder )
		{
			builder.HasKey ( x => x.Book_RID );

			builder.HasOne ( c => c.Author ).WithMany ( c => c.Books ).HasForeignKey ( c => c.Author_RID_FK );


		}

	}
}
