using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inzynierka.Repository.Migrations
{
    public partial class elo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedMotives_Motives_MotiveId",
                table: "SharedMotives");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedMotives_Motives_MotiveId",
                table: "SharedMotives",
                column: "MotiveId",
                principalTable: "Motives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedMotives_Motives_MotiveId",
                table: "SharedMotives");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedMotives_Motives_MotiveId",
                table: "SharedMotives",
                column: "MotiveId",
                principalTable: "Motives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
