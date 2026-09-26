using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionOverview.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProviderIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Providers_ServiceName",
                table: "Providers");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ServiceName",
                table: "Providers",
                column: "ServiceName",
                unique: true,
                filter: "[UserId] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ServiceName_UserId",
                table: "Providers",
                columns: new[] { "ServiceName", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Providers_ServiceName",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_ServiceName_UserId",
                table: "Providers");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ServiceName",
                table: "Providers",
                column: "ServiceName");
        }
    }
}
