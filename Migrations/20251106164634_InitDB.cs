using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOA.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Confirmed = table.Column<bool>(type: "bit", nullable: false),
                    birth_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "provinces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    department_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_provinces_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token = table.Column<int>(type: "int", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    province_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    department_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_districts_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_districts_provinces_province_id",
                        column: x => x.province_id,
                        principalTable: "provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    department_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    province_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    district_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_locations_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_locations_districts_district_id",
                        column: x => x.district_id,
                        principalTable: "districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_locations_provinces_province_id",
                        column: x => x.province_id,
                        principalTable: "provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    property_type = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    property_category = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    year_built = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    extra_info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_properties_locations_location_id",
                        column: x => x.location_id,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_properties_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "commercial_properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    has_parking = table.Column<bool>(type: "bit", nullable: false),
                    parking_spaces = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commercial_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_commercial_properties_properties_Id",
                        column: x => x.Id,
                        principalTable: "properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    property_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    public_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_images_properties_property_id",
                        column: x => x.property_id,
                        principalTable: "properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "residential_properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: true),
                    Bathrooms = table.Column<int>(type: "int", nullable: true),
                    Area = table.Column<double>(type: "float", nullable: false),
                    Furnished = table.Column<bool>(type: "bit", nullable: false),
                    has_terrace = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_residential_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_residential_properties_properties_Id",
                        column: x => x.Id,
                        principalTable: "properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "services_to_properties",
                columns: table => new
                {
                    property_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    service_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services_to_properties", x => new { x.property_id, x.service_id });
                    table.ForeignKey(
                        name: "FK_services_to_properties_properties_property_id",
                        column: x => x.property_id,
                        principalTable: "properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_services_to_properties_services_service_id",
                        column: x => x.service_id,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departments_Slug",
                table: "departments",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_districts_department_id",
                table: "districts",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_districts_province_id",
                table: "districts",
                column: "province_id");

            migrationBuilder.CreateIndex(
                name: "IX_districts_Slug",
                table: "districts",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_images_property_id",
                table: "images",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_locations_department_id",
                table: "locations",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_locations_district_id",
                table: "locations",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "IX_locations_province_id",
                table: "locations",
                column: "province_id");

            migrationBuilder.CreateIndex(
                name: "IX_locations_Slug",
                table: "locations",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_properties_location_id",
                table: "properties",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_properties_user_id",
                table: "properties",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_provinces_department_id",
                table: "provinces",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_provinces_Slug",
                table: "provinces",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_services_to_properties_service_id",
                table: "services_to_properties",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_tokens_token",
                table: "tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tokens_user_id",
                table: "tokens",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Phone",
                table: "users",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commercial_properties");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "residential_properties");

            migrationBuilder.DropTable(
                name: "services_to_properties");

            migrationBuilder.DropTable(
                name: "TestEntities");

            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "properties");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "provinces");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
