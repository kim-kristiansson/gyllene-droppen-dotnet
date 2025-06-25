using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GylleneDroppen.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMembeshipSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MembershipPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DurationInMonths = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "text", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipPeriods_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipPeriods_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTrialUsages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    HasUsedTrial = table.Column<bool>(type: "boolean", nullable: false),
                    TrialUsedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrialUsedForEventId = table.Column<Guid>(type: "uuid", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrialUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTrialUsages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrialUsages_TastingEvents_TrialUsedForEventId",
                        column: x => x.TrialUsedForEventId,
                        principalTable: "TastingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    MembershipPeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMemberships_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMemberships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMemberships_MembershipPeriods_MembershipPeriodId",
                        column: x => x.MembershipPeriodId,
                        principalTable: "MembershipPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPeriod_DurationInMonths",
                table: "MembershipPeriods",
                column: "DurationInMonths");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPeriod_IsActive",
                table: "MembershipPeriods",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPeriods_CreatedByUserId",
                table: "MembershipPeriods",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPeriods_UpdatedByUserId",
                table: "MembershipPeriods",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMembership_EndDate",
                table: "UserMemberships",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserMembership_IsActive",
                table: "UserMemberships",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserMembership_StartDate",
                table: "UserMemberships",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserMembership_UserId",
                table: "UserMemberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMembership_UserId_EndDate",
                table: "UserMemberships",
                columns: new[] { "UserId", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMemberships_CreatedByUserId",
                table: "UserMemberships",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMemberships_MembershipPeriodId",
                table: "UserMemberships",
                column: "MembershipPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrialUsage_Email",
                table: "UserTrialUsages",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrialUsage_HasUsedTrial",
                table: "UserTrialUsages",
                column: "HasUsedTrial");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrialUsage_UserId",
                table: "UserTrialUsages",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTrialUsages_TrialUsedForEventId",
                table: "UserTrialUsages",
                column: "TrialUsedForEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMemberships");

            migrationBuilder.DropTable(
                name: "UserTrialUsages");

            migrationBuilder.DropTable(
                name: "MembershipPeriods");
        }
    }
}
