using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inzynierka.Repository.Migrations
{
    public partial class changerates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PointsForAllGames",
                table: "Rates",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PointsForGame",
                table: "Quizes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PointsForQuestion",
                table: "Questions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsForAllGames",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "PointsForGame",
                table: "Quizes");

            migrationBuilder.DropColumn(
                name: "PointsForQuestion",
                table: "Questions");
        }
    }
}
