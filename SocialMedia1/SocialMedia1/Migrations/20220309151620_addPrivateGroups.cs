using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia1.Migrations
{
    public partial class addPrivateGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Groups",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Groups");
        }
    }
}
