using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schiavello.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedDescriptionAndChangeThePhotoNameProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoName",
                table: "Photos",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Photos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Photos",
                newName: "PhotoName");
        }
    }
}
