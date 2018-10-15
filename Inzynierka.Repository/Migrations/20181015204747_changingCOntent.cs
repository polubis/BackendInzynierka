using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inzynierka.Repository.Migrations
{
    public partial class changingCOntent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SecondsSpendOnQuiz",
                table: "Quizes",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "TimeForAnswerInSeconds",
                table: "Questions",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SecondsSpendOnQuiz",
                table: "Quizes",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "TimeForAnswerInSeconds",
                table: "Questions",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
