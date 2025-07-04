using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GylleneDroppen.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeRegionAndTypeOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Whiskies_Regions_RegionId",
                table: "Whiskies");

            migrationBuilder.DropForeignKey(
                name: "FK_Whiskies_WhiskyTypes_WhiskyTypeId",
                table: "Whiskies");

            migrationBuilder.AlterColumn<Guid>(
                name: "WhiskyTypeId",
                table: "Whiskies",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                table: "Whiskies",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Whiskies_Regions_RegionId",
                table: "Whiskies",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Whiskies_WhiskyTypes_WhiskyTypeId",
                table: "Whiskies",
                column: "WhiskyTypeId",
                principalTable: "WhiskyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Whiskies_Regions_RegionId",
                table: "Whiskies");

            migrationBuilder.DropForeignKey(
                name: "FK_Whiskies_WhiskyTypes_WhiskyTypeId",
                table: "Whiskies");

            migrationBuilder.AlterColumn<Guid>(
                name: "WhiskyTypeId",
                table: "Whiskies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                table: "Whiskies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Whiskies_Regions_RegionId",
                table: "Whiskies",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Whiskies_WhiskyTypes_WhiskyTypeId",
                table: "Whiskies",
                column: "WhiskyTypeId",
                principalTable: "WhiskyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
