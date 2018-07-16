using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inzynierka.Repository.Migrations
{
    public partial class elo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAcceptedRegister",
                table: "Users",
                newName: "IsAcceptedRegister");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAcceptedRegister",
                table: "Users",
                newName: "isAcceptedRegister");
        }
    }
}
