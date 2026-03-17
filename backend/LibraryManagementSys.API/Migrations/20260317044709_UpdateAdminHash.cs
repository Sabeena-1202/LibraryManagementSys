using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSys.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$fWsSOgldFQZh/wbv2zMuZOX4c0gSKCY0F/qopN8tPb2igbbQDI7Pa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$9WCfQRHGDI8tDDVMOb6xsOhMJFAFCBXeHRPRVjW3QFwqDhqxWIXz");
        }
    }
}
