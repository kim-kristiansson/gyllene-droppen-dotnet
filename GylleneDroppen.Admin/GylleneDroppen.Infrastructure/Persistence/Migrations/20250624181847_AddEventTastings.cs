using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GylleneDroppen.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTastings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TastingEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MaxParticipants = table.Column<int>(type: "integer", nullable: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    OrganizedByUserId = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TastingEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TastingEvents_AspNetUsers_OrganizedByUserId",
                        column: x => x.OrganizedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TastingEventParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TastingEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RegisteredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Attended = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TastingEventParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TastingEventParticipants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TastingEventParticipants_TastingEvents_TastingEventId",
                        column: x => x.TastingEventId,
                        principalTable: "TastingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TastingEventWhiskies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TastingEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    WhiskyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AddedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AddedByUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TastingEventWhiskies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TastingEventWhiskies_AspNetUsers_AddedByUserId",
                        column: x => x.AddedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TastingEventWhiskies_TastingEvents_TastingEventId",
                        column: x => x.TastingEventId,
                        principalTable: "TastingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TastingEventWhiskies_Whiskies_WhiskyId",
                        column: x => x.WhiskyId,
                        principalTable: "Whiskies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TastingEventParticipant_TastingEventId",
                table: "TastingEventParticipants",
                column: "TastingEventId");

            migrationBuilder.CreateIndex(
                name: "IX_TastingEventParticipant_TastingEventId_UserId",
                table: "TastingEventParticipants",
                columns: new[] { "TastingEventId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TastingEventParticipant_UserId",
                table: "TastingEventParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TastingEvent_EventDate",
                table: "TastingEvents",
                column: "EventDate");

            migrationBuilder.CreateIndex(
                name: "IX_TastingEvent_IsPublic",
                table: "TastingEvents",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_TastingEvent_OrganizedByUserId",
                table: "TastingEvents",
                column: "OrganizedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TastingEventWhiskies_AddedByUserId",
                table: "TastingEventWhiskies",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TastingEventWhisky_Order",
                table: "TastingEventWhiskies",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_TastingEventWhisky_TastingEventId",
                table: "TastingEventWhiskies",
                column: "TastingEventId");

            migrationBuilder.CreateIndex(
                name: "IX_TastingEventWhisky_TastingEventId_WhiskyId",
                table: "TastingEventWhiskies",
                columns: new[] { "TastingEventId", "WhiskyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TastingEventWhisky_WhiskyId",
                table: "TastingEventWhiskies",
                column: "WhiskyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TastingEventParticipants");

            migrationBuilder.DropTable(
                name: "TastingEventWhiskies");

            migrationBuilder.DropTable(
                name: "TastingEvents");
        }
    }
}
