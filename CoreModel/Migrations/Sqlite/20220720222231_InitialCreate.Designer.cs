﻿// <auto-generated />
using CoreModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CoreModel.Migrations.Sqlite
{
    [DbContext(typeof(SqliteGtContext))]
    [Migration("20220720222231_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("CoreModel.Containers.Author", b =>
                {
                    b.Property<int>("Author_RID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Author_RID");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Author_RID = 1,
                            FirstName = "William",
                            LastName = "Shakespere"
                        },
                        new
                        {
                            Author_RID = 2,
                            FirstName = "Issac",
                            LastName = "Asimov"
                        },
                        new
                        {
                            Author_RID = 3,
                            FirstName = "Jules",
                            LastName = "Verne"
                        },
                        new
                        {
                            Author_RID = 4,
                            FirstName = "Wilson",
                            LastName = "Rawls"
                        });
                });

            modelBuilder.Entity("CoreModel.Containers.Book", b =>
                {
                    b.Property<int>("Book_RID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Author_RID_FK")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Book_RID");

                    b.HasIndex("Author_RID_FK");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Book_RID = 1,
                            Author_RID_FK = 1,
                            Title = "A Midsummer Nights Dread"
                        },
                        new
                        {
                            Book_RID = 2,
                            Author_RID_FK = 1,
                            Title = "The Merchant of Venice"
                        },
                        new
                        {
                            Book_RID = 3,
                            Author_RID_FK = 1,
                            Title = "King Lear"
                        },
                        new
                        {
                            Book_RID = 4,
                            Author_RID_FK = 2,
                            Title = "I Robot"
                        },
                        new
                        {
                            Book_RID = 5,
                            Author_RID_FK = 2,
                            Title = "Foundation"
                        },
                        new
                        {
                            Book_RID = 6,
                            Author_RID_FK = 3,
                            Title = "Journey to the Center of the Earth"
                        },
                        new
                        {
                            Book_RID = 7,
                            Author_RID_FK = 3,
                            Title = "Around the World in 80 Days"
                        },
                        new
                        {
                            Book_RID = 8,
                            Author_RID_FK = 3,
                            Title = "The Mysterious Island"
                        },
                        new
                        {
                            Book_RID = 9,
                            Author_RID_FK = 4,
                            Title = "Where the Red Fern Grows"
                        });
                });

            modelBuilder.Entity("CoreModel.Containers.Book", b =>
                {
                    b.HasOne("CoreModel.Containers.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("Author_RID_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("CoreModel.Containers.Author", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
