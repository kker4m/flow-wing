using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlowWing.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReleationshipUpdate : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledEmails_RepeatingMails_RepeatingMailId",
                table: "ScheduledEmails");

            migrationBuilder.DropTable(
                name: "RepeatingMails");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledEmails_RepeatingMailId",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "EmailSubject",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "RecipientsEmail",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "RepeatingMailId",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "SenderEmail",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "SentDateTime",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "SentEmailBody",
                table: "ScheduledEmails");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ScheduledEmails",
                newName: "IsRepeating");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSendingDate",
                table: "ScheduledEmails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextSendingDate",
                table: "ScheduledEmails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepeatEndDate",
                table: "ScheduledEmails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepeatInterval",
                table: "ScheduledEmails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "EmailLogs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentDateTime",
                table: "EmailLogs",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduledEmailId",
                table: "EmailLogs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_ScheduledEmails_ScheduledEmailId",
                table: "EmailLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_Users_UserId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "LastSendingDate",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "NextSendingDate",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "RepeatEndDate",
                table: "ScheduledEmails");

            migrationBuilder.DropColumn(
                name: "RepeatInterval",
                table: "ScheduledEmails");

            migrationBuilder.RenameColumn(
                name: "IsRepeating",
                table: "ScheduledEmails",
                newName: "Status");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "ScheduledEmails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmailSubject",
                table: "ScheduledEmails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientsEmail",
                table: "ScheduledEmails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RepeatingMailId",
                table: "ScheduledEmails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SenderEmail",
                table: "ScheduledEmails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDateTime",
                table: "ScheduledEmails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SentEmailBody",
                table: "ScheduledEmails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "EmailLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentDateTime",
                table: "EmailLogs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ScheduledEmailId",
                table: "EmailLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RepeatingMails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmailSubject = table.Column<string>(type: "text", nullable: false),
                    EndingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NextScheduledDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RecipientsEmail = table.Column<string>(type: "text", nullable: false),
                    RepeatInterval = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SenderEmail = table.Column<string>(type: "text", nullable: false),
                    SentEmailBody = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepeatingMails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledEmails_RepeatingMailId",
                table: "ScheduledEmails",
                column: "RepeatingMailId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_ScheduledEmails_ScheduledEmailId",
                table: "EmailLogs",
                column: "ScheduledEmailId",
                principalTable: "ScheduledEmails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_Users_UserId",
                table: "EmailLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledEmails_RepeatingMails_RepeatingMailId",
                table: "ScheduledEmails",
                column: "RepeatingMailId",
                principalTable: "RepeatingMails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
