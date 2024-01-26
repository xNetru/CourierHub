using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierHub.Shared.Migrations {
    /// <inheritdoc />
    public partial class MainMigration : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Number = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    Flat = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    Postal_code = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Datetime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Review", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rule",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Depth_max = table.Column<int>(type: "int", nullable: true),
                    Width_max = table.Column<int>(type: "int", nullable: true),
                    Length_max = table.Column<int>(type: "int", nullable: true),
                    Mass_max = table.Column<int>(type: "int", nullable: true),
                    Velocity_max = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Rule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scaler",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Depth = table.Column<decimal>(type: "money", nullable: true),
                    Width = table.Column<decimal>(type: "money", nullable: true),
                    Length = table.Column<decimal>(type: "money", nullable: true),
                    Mass = table.Column<decimal>(type: "money", nullable: true),
                    Distance = table.Column<decimal>(type: "money", nullable: true),
                    Time = table.Column<decimal>(type: "money", nullable: true),
                    Company = table.Column<decimal>(type: "money", nullable: true),
                    Weekend = table.Column<decimal>(type: "money", nullable: true),
                    Priority = table.Column<decimal>(type: "money", nullable: true),
                    Fee = table.Column<decimal>(type: "money", nullable: true),
                    Tax = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Scaler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Api_key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Statute = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BaseAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsIntegrated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsCancelable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client_data",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Photo = table.Column<byte[]>(type: "image", nullable: true),
                    Phone = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_Id = table.Column<int>(type: "int", nullable: false),
                    Source_address_Id = table.Column<int>(type: "int", nullable: false),
                    Client_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Client_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_data_Address1",
                        column: x => x.Address_Id,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Client_data_Address2",
                        column: x => x.Source_address_Id,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Client_data_User",
                        column: x => x.Client_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evaluation",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Rejection_reason = table.Column<string>(type: "ntext", nullable: true),
                    Worker_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Evaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evaluation_User",
                        column: x => x.Worker_Id,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Inquire",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Client_Id = table.Column<int>(type: "int", nullable: true),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Mass = table.Column<int>(type: "int", nullable: false),
                    Source_Id = table.Column<int>(type: "int", nullable: false),
                    Destination_Id = table.Column<int>(type: "int", nullable: false),
                    Source_date = table.Column<DateTime>(type: "date", nullable: false),
                    Destination_date = table.Column<DateTime>(type: "date", nullable: false),
                    Datetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsCompany = table.Column<bool>(type: "bit", nullable: false),
                    IsWeekend = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Inquire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inquire_Address",
                        column: x => x.Source_Id,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inquire_Address1",
                        column: x => x.Destination_Id,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inquire_User",
                        column: x => x.Client_Id,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Parcel",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pickup_datetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Delivery_datetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Undelivered_reason = table.Column<string>(type: "ntext", nullable: true),
                    Courier_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Parcel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcel_User",
                        column: x => x.Courier_Id,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Inquire_Id = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Status_Id = table.Column<int>(type: "int", nullable: false),
                    Service_Id = table.Column<int>(type: "int", nullable: false),
                    Evaluation_Id = table.Column<int>(type: "int", nullable: true),
                    Parcel_Id = table.Column<int>(type: "int", nullable: true),
                    Review_Id = table.Column<int>(type: "int", nullable: true),
                    Client_Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Client_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Client_Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Client_Phone = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    Client_Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Client_Address_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Address",
                        column: x => x.Client_Address_Id,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Evaluation",
                        column: x => x.Evaluation_Id,
                        principalTable: "Evaluation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Inquire",
                        column: x => x.Inquire_Id,
                        principalTable: "Inquire",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Parcel",
                        column: x => x.Parcel_Id,
                        principalTable: "Parcel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Review",
                        column: x => x.Review_Id,
                        principalTable: "Review",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Service",
                        column: x => x.Service_Id,
                        principalTable: "Service",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Status",
                        column: x => x.Status_Id,
                        principalTable: "Status",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_data_Address_Id",
                table: "Client_data",
                column: "Address_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Client_data_Client_Id",
                table: "Client_data",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Client_data_Source_address_Id",
                table: "Client_data",
                column: "Source_address_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluation_Worker_Id",
                table: "Evaluation",
                column: "Worker_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inquire_Client_Id",
                table: "Inquire",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inquire_Destination_Id",
                table: "Inquire",
                column: "Destination_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inquire_Source_Id",
                table: "Inquire",
                column: "Source_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Client_Address_Id",
                table: "Order",
                column: "Client_Address_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Evaluation_Id",
                table: "Order",
                column: "Evaluation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Inquire_Id",
                table: "Order",
                column: "Inquire_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Parcel_Id",
                table: "Order",
                column: "Parcel_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Review_Id",
                table: "Order",
                column: "Review_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Service_Id",
                table: "Order",
                column: "Service_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Status_Id",
                table: "Order",
                column: "Status_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Parcel_Courier_Id",
                table: "Parcel",
                column: "Courier_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "Client_data");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Rule");

            migrationBuilder.DropTable(
                name: "Scaler");

            migrationBuilder.DropTable(
                name: "Evaluation");

            migrationBuilder.DropTable(
                name: "Inquire");

            migrationBuilder.DropTable(
                name: "Parcel");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
