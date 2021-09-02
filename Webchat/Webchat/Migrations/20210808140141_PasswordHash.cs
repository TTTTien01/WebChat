using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webchat.Migrations
{
    public partial class PasswordHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "AppUser",
                type: "varbinary(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "AppUser",
                type: "varbinary(200)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary()",
                oldMaxLength: 200);
        }
    }
}
