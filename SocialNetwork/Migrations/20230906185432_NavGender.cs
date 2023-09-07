using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Migrations
{
    /// <inheritdoc />
    public partial class NavGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_IdGender",
                table: "Users",
                column: "IdGender");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Genders_IdGender",
                table: "Users",
                column: "IdGender",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Genders_IdGender",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdGender",
                table: "Users");
        }
    }
}
