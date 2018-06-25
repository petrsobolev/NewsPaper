using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NewsPaper.Data.Migrations
{
    public partial class model3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserPhoto",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserPhotoUrl",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserPhotoUrl",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "UserPhoto",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
