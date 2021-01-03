using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class photoEntityChangeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackGroundImage",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "friendsCount",
                table: "AspNetUsers",
                newName: "FriendsCount");

            migrationBuilder.AddColumn<string>(
                name: "BackGroundPhoto",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackGroundPhoto",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FriendsCount",
                table: "AspNetUsers",
                newName: "friendsCount");

            migrationBuilder.AddColumn<string>(
                name: "BackGroundImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
