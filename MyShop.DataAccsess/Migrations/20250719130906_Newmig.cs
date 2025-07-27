using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop.Web.Migrations
{
    /// <inheritdoc />
    public partial class Newmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_AspNetUsers_applicationUserId",
                table: "orderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_orderHeaders_applicationUserId",
                table: "orderHeaders");

            migrationBuilder.DropColumn(
                name: "applicationUserId",
                table: "orderHeaders");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicatioUserId",
                table: "orderHeaders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_orderHeaders_ApplicatioUserId",
                table: "orderHeaders",
                column: "ApplicatioUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_AspNetUsers_ApplicatioUserId",
                table: "orderHeaders",
                column: "ApplicatioUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_AspNetUsers_ApplicatioUserId",
                table: "orderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_orderHeaders_ApplicatioUserId",
                table: "orderHeaders");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicatioUserId",
                table: "orderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "applicationUserId",
                table: "orderHeaders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orderHeaders_applicationUserId",
                table: "orderHeaders",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_AspNetUsers_applicationUserId",
                table: "orderHeaders",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
