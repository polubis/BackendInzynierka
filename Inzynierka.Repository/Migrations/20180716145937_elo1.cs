using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inzynierka.Repository.Migrations
{
    public partial class elo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Sex",
                table: "Users",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Sex",
                table: "Users",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
