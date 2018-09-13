using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inzynierka.Repository.Migrations
{
    public partial class changingquizrating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WasAnswerCorrect",
                table: "Questions",
                newName: "AnsweredBeforeSugestion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnsweredBeforeSugestion",
                table: "Questions",
                newName: "WasAnswerCorrect");
        }
    }
}
