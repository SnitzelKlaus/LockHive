using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Password.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToPasswordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Operations",
                table: "Operations");

            migrationBuilder.RenameTable(
                name: "Operations",
                newName: "PasswordOperations");

            migrationBuilder.RenameIndex(
                name: "IX_Operations_RequestId",
                table: "PasswordOperations",
                newName: "IX_PasswordOperations_RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Operations_PasswordId",
                table: "PasswordOperations",
                newName: "IX_PasswordOperations_PasswordId");

            migrationBuilder.RenameIndex(
                name: "IX_Operations_ClusterId",
                table: "PasswordOperations",
                newName: "IX_PasswordOperations_ClusterId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Passwords",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PasswordOperations",
                table: "PasswordOperations",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PasswordOperations",
                table: "PasswordOperations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Passwords");

            migrationBuilder.RenameTable(
                name: "PasswordOperations",
                newName: "Operations");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordOperations_RequestId",
                table: "Operations",
                newName: "IX_Operations_RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordOperations_PasswordId",
                table: "Operations",
                newName: "IX_Operations_PasswordId");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordOperations_ClusterId",
                table: "Operations",
                newName: "IX_Operations_ClusterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Operations",
                table: "Operations",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
