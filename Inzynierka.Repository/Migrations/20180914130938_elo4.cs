using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inzynierka.Repository.Migrations
{
    public partial class elo4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Motives_MotiveId",
                table: "UserSettings");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Motives_MotiveId",
                table: "UserSettings",
                column: "MotiveId",
                principalTable: "Motives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Motives_MotiveId",
                table: "UserSettings");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Motives_MotiveId",
                table: "UserSettings",
                column: "MotiveId",
                principalTable: "Motives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
