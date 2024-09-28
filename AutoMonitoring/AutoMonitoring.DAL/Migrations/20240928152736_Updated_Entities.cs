using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoMonitoring.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a011990-bce1-481d-a5a5-60d0cba79c61"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: new Guid("3d980679-71bc-4894-b866-3d9c022e0cd2"));

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserSessions",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "UserSessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "UserSessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BlockedUntil",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { new Guid("d4e04492-84d2-4489-9fa6-b3c4263f67c7"), false, "Resident" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "IsDeleted", "RoleId", "UserId" },
                values: new object[] { new Guid("a9f348a7-6362-4b9c-8126-2d9f8d702272"), false, new Guid("583e1840-ba88-418d-ae9e-4ce7571f0946"), new Guid("bd65e7bd-e25a-4935-81d1-05093b5f48c0") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bd65e7bd-e25a-4935-81d1-05093b5f48c0"),
                columns: new[] { "BlockedUntil", "IsBlocked", "Password" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Admin14689" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d4e04492-84d2-4489-9fa6-b3c4263f67c7"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: new Guid("a9f348a7-6362-4b9c-8126-2d9f8d702272"));

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "BlockedUntil",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { new Guid("2a011990-bce1-481d-a5a5-60d0cba79c61"), false, "Resident" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "IsDeleted", "RoleId", "UserId" },
                values: new object[] { new Guid("3d980679-71bc-4894-b866-3d9c022e0cd2"), false, new Guid("583e1840-ba88-418d-ae9e-4ce7571f0946"), new Guid("bd65e7bd-e25a-4935-81d1-05093b5f48c0") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bd65e7bd-e25a-4935-81d1-05093b5f48c0"),
                column: "PasswordHash",
                value: "$2b$10$7mDInwM.f1sBgp.DDlng.OZsRYvvc6K4xmtmsslOemakouYGKqwYK");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId",
                unique: true);
        }
    }
}
