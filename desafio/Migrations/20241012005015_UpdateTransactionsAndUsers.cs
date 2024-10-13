using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace desafio.Migrations
{
    public partial class UpdateTransactionsAndUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "tb_user",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "bonus_processed",
                table: "tb_transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                table: "tb_user");

            migrationBuilder.DropColumn(
                name: "bonus_processed",
                table: "tb_transaction");
        }
    }
}
