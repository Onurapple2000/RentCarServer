using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class login_token_and_deneme_table_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deneme",
                columns: table => new
                {
                    DenemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DenemeStr1 = table.Column<string>(type: "varchar(MAX)", maxLength: 100, nullable: false),
                    DenemeStr2 = table.Column<string>(type: "varchar(MAX)", maxLength: 50, nullable: false),
                    DenemeInt1 = table.Column<int>(type: "int", nullable: false),
                    DenemeInt2 = table.Column<int>(type: "int", nullable: false),
                    DenemeBool1 = table.Column<bool>(type: "bit", nullable: false),
                    DenemeBool2 = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deneme", x => x.DenemeId);
                });

            migrationBuilder.CreateTable(
                name: "LoginTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token_Value = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive_Value = table.Column<bool>(type: "bit", nullable: false),
                    ExpiresDate_Value = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginTokens", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deneme");

            migrationBuilder.DropTable(
                name: "LoginTokens");
        }
    }
}
