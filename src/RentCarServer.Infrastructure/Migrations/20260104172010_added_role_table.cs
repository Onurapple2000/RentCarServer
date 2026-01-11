using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_role_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Branchs",
                table: "Branchs");

            migrationBuilder.RenameTable(
                name: "Branchs",
                newName: "Branches");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branches",
                table: "Branches",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name_Value = table.Column<string>(type: "varchar(MAX)", nullable: false),
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
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Branches",
                table: "Branches");

            migrationBuilder.RenameTable(
                name: "Branches",
                newName: "Branchs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branchs",
                table: "Branchs",
                column: "Id");
        }
    }
}
