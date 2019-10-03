using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PSLovers2.Data.Migrations
{
    public partial class lastupdatefavorite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateTime",
                table: "FavoriteForUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateTime",
                table: "Favorites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateTime",
                table: "Favorites");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateTime",
                table: "FavoriteForUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
