using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourierHub.Shared.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rule",
                columns: new[] { "Id", "Depth_max", "Length_max", "Mass_max", "Velocity_max", "Width_max" },
                values: new object[] { 1, 8000, 8000, 20000, 20f, 8000 });

            migrationBuilder.InsertData(
                table: "Scaler",
                columns: new[] { "Id", "Company", "Depth", "Distance", "Fee", "Length", "Mass", "Name", "Priority", "Tax", "Time", "Weekend", "Width" },
                values: new object[] { 1, 2.3m, 1.2m, 3m, 1.2m, 1.2m, 2m, "SusScaler", 1.2m, 1.18f, 1.3m, 1.2m, 1.2m });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "Id", "Api_key", "BaseAddress", "IsIntegrated", "Name", "Statute" },
                values: new object[,]
                {
                    { 1, "a2cf116b-854a-4b96-99f0-c88fdadb6de6", "https://courierhub-bck-api-new.azurewebsites.net/", true, "CourierHub", "TBD" },
                    { 2, "team2d;EAAA50B8-90CB-436E-9864-4BC75B56F3BE", "https://mini.currier.api.snet.com.pl/", true, "MiNI.Courier.API", "TBD" },
                    { 3, "ApiKey.1", "https://couriercompanyapi.azurewebsites.net/", false, "WeraHubApi", "TBD" },
                    { 4, "79a31940-2209-4422-93bd-f0ce9067a3c8", "https://courierhub-bck-api-new.azurewebsites.net/", false, "CourierHub-Wera", "TBD" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "IsCancelable", "Name" },
                values: new object[,]
                {
                    { 1, true, "NotConfirmed" },
                    { 2, true, "Confirmed" },
                    { 3, false, "Cancelled" },
                    { 4, false, "Denied" },
                    { 5, false, "PickedUp" },
                    { 6, false, "Delivered" },
                    { 7, false, "CouldNotDeliver" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Scaler",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
