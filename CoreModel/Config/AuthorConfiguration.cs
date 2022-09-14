using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


using CoreModel.Containers;


namespace CoreModel.Config
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
	{
		public void Configure ( EntityTypeBuilder<Author> builder )
		{
			builder.HasKey ( x => x.Author_RID );

			builder.HasMany ( b => b.Books ).WithOne ( p => p.Author );

		}

	}
}
