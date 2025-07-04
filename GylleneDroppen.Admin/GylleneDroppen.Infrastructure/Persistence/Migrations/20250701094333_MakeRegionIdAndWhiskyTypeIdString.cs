using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GylleneDroppen.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeRegionIdAndWhiskyTypeIdString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Countries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Countries",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
