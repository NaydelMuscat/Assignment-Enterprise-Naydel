using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acls_TextFiles_FileIdFk",
                table: "Acls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextFiles",
                table: "TextFiles");

            migrationBuilder.DropIndex(
                name: "IX_Acls_FileIdFk",
                table: "Acls");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TextFiles");

            migrationBuilder.DropColumn(
                name: "FileIdFk",
                table: "Acls");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextFiles",
                table: "TextFiles",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_Acls_FileName",
                table: "Acls",
                column: "FileName");

            migrationBuilder.AddForeignKey(
                name: "FK_Acls_TextFiles_FileName",
                table: "Acls",
                column: "FileName",
                principalTable: "TextFiles",
                principalColumn: "FileName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acls_TextFiles_FileName",
                table: "Acls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextFiles",
                table: "TextFiles");

            migrationBuilder.DropIndex(
                name: "IX_Acls_FileName",
                table: "Acls");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TextFiles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FileIdFk",
                table: "Acls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextFiles",
                table: "TextFiles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Acls_FileIdFk",
                table: "Acls",
                column: "FileIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_Acls_TextFiles_FileIdFk",
                table: "Acls",
                column: "FileIdFk",
                principalTable: "TextFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
