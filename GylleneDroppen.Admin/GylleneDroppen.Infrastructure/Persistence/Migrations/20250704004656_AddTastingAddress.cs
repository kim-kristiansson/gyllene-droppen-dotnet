using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GylleneDroppen.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTastingAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Department_Value",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Departments");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "TastingEvents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StreetAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "text", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TastingEvents_AddressId",
                table: "TastingEvents",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_City",
                table: "Addresses",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Address_IsActive",
                table: "Addresses",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Address_Name",
                table: "Addresses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CreatedByUserId",
                table: "Addresses",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UpdatedByUserId",
                table: "Addresses",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TastingEvents_Addresses_AddressId",
                table: "TastingEvents",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TastingEvents_Addresses_AddressId",
                table: "TastingEvents");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_TastingEvents_AddressId",
                table: "TastingEvents");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "TastingEvents");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Departments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Department_Value",
                table: "Departments",
                column: "Value",
                unique: true);
        }
    }
}
