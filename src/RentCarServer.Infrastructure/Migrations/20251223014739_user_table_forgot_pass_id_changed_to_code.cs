using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations;

/// <inheritdoc />
public partial class user_table_forgot_pass_id_changed_to_code : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
        name: "ForgotPasswordId_Value",
        table: "Users",
        newName: "ForgotPasswordCode_Value");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
        name: "ForgotPasswordId_Value",
        table: "Users",
        newName: "ForgotPasswordCode_Value");
    }
}
