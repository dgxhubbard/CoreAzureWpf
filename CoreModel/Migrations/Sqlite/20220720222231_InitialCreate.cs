using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreModel.Migrations.Sqlite
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Author_RID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Author_RID);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Book_RID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Author_RID_FK = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Book_RID);
                    table.ForeignKey(
                        name: "FK_Books_Authors_Author_RID_FK",
                        column: x => x.Author_RID_FK,
                        principalTable: "Authors",
                        principalColumn: "Author_RID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Author_RID", "FirstName", "LastName" },
                values: new object[] { 1, "William", "Shakespere" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Author_RID", "FirstName", "LastName" },
                values: new object[] { 2, "Issac", "Asimov" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Author_RID", "FirstName", "LastName" },
                values: new object[] { 3, "Jules", "Verne" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Author_RID", "FirstName", "LastName" },
                values: new object[] { 4, "Wilson", "Rawls" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[] { 1, 1, "A Midsummer Nights Dread" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[] { 2, 1, "The Merchant of Venice" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[] { 3, 1, "King Lear" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[] { 4, 2, "I Robot" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[] { 5, 2, "Foundation" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[] { 6, 3, "Journey to the Center of the Earth" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[] { 7, 3, "Around the World in 80 Days" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[] { 8, 3, "The Mysterious Island" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_RID", "Author_RID_FK", "Title" },
                values: new object[] { 9, 4, "Where the Red Fern Grows" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Author_RID_FK",
                table: "Books",
                column: "Author_RID_FK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
