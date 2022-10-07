using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce0.Migrations
{
    public partial class AddAvatarToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Phải sửa hết các vị trí có sử dụng IdentityUser -> MyIdentityUser
            migrationBuilder.AddColumn<string>(
                name: "AvatarPath",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarPath",
                table: "Users");
        }
    }
}
