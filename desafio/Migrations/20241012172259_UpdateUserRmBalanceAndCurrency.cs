using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace desafio.Migrations
{
    public partial class UpdateUserRmBalanceAndCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                table: "tb_user");

            migrationBuilder.DropColumn(
                name: "initialBalance",
                table: "tb_user");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "tb_user",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "initialBalance",
                table: "tb_user",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
