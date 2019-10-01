using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PSLovers2.Data.Migrations
{
    public partial class ffu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "LastUpdateTime",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "Owned",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Favorites");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FavoriteForUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FavoriteId = table.Column<int>(nullable: false),
                    Owned = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteForUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteForUsers_Favorites_FavoriteId",
                        column: x => x.FavoriteId,
                        principalTable: "Favorites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteForUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FavoriteId",
                table: "AspNetUsers",
                column: "FavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteForUsers_FavoriteId",
                table: "FavoriteForUsers",
                column: "FavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteForUsers_UserId",
                table: "FavoriteForUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Favorites_FavoriteId",
                table: "AspNetUsers",
                column: "FavoriteId",
                principalTable: "Favorites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Favorites_FavoriteId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "FavoriteForUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FavoriteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateTime",
                table: "Favorites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Owned",
                table: "Favorites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Favorites",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
