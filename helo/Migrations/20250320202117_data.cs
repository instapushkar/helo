using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helo.Migrations
{
    /// <inheritdoc />
    public partial class data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDatas_UserAccounts_UserId",
                table: "ProductDatas");

            migrationBuilder.DropIndex(
                name: "IX_ProductDatas_UserId",
                table: "ProductDatas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductDatas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ProductDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDatas_UserId",
                table: "ProductDatas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDatas_UserAccounts_UserId",
                table: "ProductDatas",
                column: "UserId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
