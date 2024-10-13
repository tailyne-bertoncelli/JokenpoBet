using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace desafio.Migrations
{
    public partial class AddWalletInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_wallet_user_id",
                table: "tb_wallet");

            migrationBuilder.CreateIndex(
                name: "IX_tb_wallet_user_id",
                table: "tb_wallet",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_wallet_user_id",
                table: "tb_wallet");

            migrationBuilder.CreateIndex(
                name: "IX_tb_wallet_user_id",
                table: "tb_wallet",
                column: "user_id");
        }
    }
}
