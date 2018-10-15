using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inzynierka.Repository.Migrations
{
    public partial class changeSoundModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OctaveSymbol",
                table: "Sounds");

            migrationBuilder.AddColumn<int>(
                name: "GuitarString",
                table: "Sounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoundPosition",
                table: "Sounds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuitarString",
                table: "Sounds");

            migrationBuilder.DropColumn(
                name: "SoundPosition",
                table: "Sounds");

            migrationBuilder.AddColumn<string>(
                name: "OctaveSymbol",
                table: "Sounds",
                nullable: false,
                defaultValue: "");
        }
    }
}
