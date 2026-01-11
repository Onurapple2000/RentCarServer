using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations;

/// <inheritdoc />
public partial class value_added : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
        name: "ForgotPasswordCode",
        table: "Users",
        newName: "ForgotPasswordCode_Value");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
        name: "ForgotPasswordCode_Value",
        table: "Users",
        newName: "ForgotPasswordCode");
    }
}
