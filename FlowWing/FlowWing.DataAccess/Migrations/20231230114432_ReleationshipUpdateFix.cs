using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowWing.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReleationshipUpdateFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_ScheduledEmails_ScheduledEmailId",
                table: "EmailLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_Users_UserId",
                table: "EmailLogs");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_ScheduledEmailId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "ScheduledEmailId",
                table: "EmailLogs");

            migrationBuilder.AddColumn<int>(
                name: "EmailLogId",
                table: "ScheduledEmails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "EmailLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledEmails_EmailLogId",
                table: "ScheduledEmails",
                column: "EmailLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_Users_UserId",
                table: "EmailLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledEmails_EmailLogs_EmailLogId",
                table: "ScheduledEmails",
                column: "EmailLogId",
                principalTable: "EmailLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_Users_UserId",
                table: "EmailLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledEmails_EmailLogs_EmailLogId",
                table: "ScheduledEmails");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledEmails_EmailLogId",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "EmailLogId",
                table: "ScheduledEmails");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "EmailLogs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ScheduledEmailId",
                table: "EmailLogs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_ScheduledEmailId",
                table: "EmailLogs",
                column: "ScheduledEmailId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_ScheduledEmails_ScheduledEmailId",
                table: "EmailLogs",
                column: "ScheduledEmailId",
                principalTable: "ScheduledEmails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_Users_UserId",
                table: "EmailLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
