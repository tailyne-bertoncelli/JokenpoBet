using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace desafio.Migrations
{
    public partial class AddBettingInTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "betting",
                table: "tb_transaction",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "betting",
                table: "tb_transaction");
        }
    }
}
