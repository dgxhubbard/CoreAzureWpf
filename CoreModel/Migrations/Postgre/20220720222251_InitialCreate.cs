using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoreModel.Migrations.Postgre
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "FOO");

            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "FOO",
                columns: table => new
                {
                    Author_RID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Author_RID);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "FOO",
                columns: table => new
                {
                    Book_RID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Author_RID_FK = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Book_RID);
                    table.ForeignKey(
                        name: "FK_Books_Authors_Author_RID_FK",
                        column: x => x.Author_RID_FK,
                        principalSchema: "FOO",
                        principalTable: "Authors",
                        principalColumn: "Author_RID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "FOO",
                table: "Authors",
                columns: new[] { "Author_RID", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "William", "Shakespere" },
                    { 2, "Issac", "Asimov" },
                    { 3, "Jules", "Verne" },
                    { 4, "Wilson", "Rawls" }
                });

            migrationBuilder.InsertData(
                schema: "FOO",
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[,]
                {
                    { 1, 1, "A Midsummer Nights Dread" },
                    { 2, 1, "The Merchant of Venice" },
                    { 3, 1, "King Lear" },
                    { 4, 2, "I Robot" },
                    { 5, 2, "Foundation" },
                    { 6, 3, "Journey to the Center of the Earth" },
                    { 7, 3, "Around the World in 80 Days" },
                    { 8, 3, "The Mysterious Island" },
                    { 9, 4, "Where the Red Fern Grows" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Author_RID_FK",
                schema: "FOO",
                table: "Books",
                column: "Author_RID_FK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books",
                schema: "FOO");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "FOO");
        }
    }
}
