using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AptechVisionPetZilla.Migrations
{
    /// <inheritdoc />
    public partial class AddPetImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    PetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ImagePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pets__48E538621267AA6E", x => x.PetId);
                });

            migrationBuilder.CreateTable(
                name: "PetsStray",
                columns: table => new
                {
                    PetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ImagePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PetsStra__48E5386234DF1744", x => x.PetId);
                });

            migrationBuilder.CreateTable(
                name: "USER_REGISTRATION",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FIRST_NAME = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    LAST_NAME = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    USER_NAME = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    USER_EMAIL = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    USER_PASSWORD = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    USER_ROLE = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USER_REG__F3BEEBFF781C6630", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "AdoptionRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    RequesterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "pending"),
                    RequestedOn = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Adoption__33A8517A47003281", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK__AdoptionR__PetId__4E88ABD4",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId");
                });

            migrationBuilder.CreateTable(
                name: "PetImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PetsStrayPetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PetImage__ImageId", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_PetImages_PetsStray_PetsStrayPetId",
                        column: x => x.PetsStrayPetId,
                        principalTable: "PetsStray",
                        principalColumn: "PetId");
                    table.ForeignKey(
                        name: "FK_PetImages_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionRequests_PetId",
                table: "AdoptionRequests",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_PetImages_PetId",
                table: "PetImages",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_PetImages_PetsStrayPetId",
                table: "PetImages",
                column: "PetsStrayPetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdoptionRequests");

            migrationBuilder.DropTable(
                name: "PetImages");

            migrationBuilder.DropTable(
                name: "USER_REGISTRATION");

            migrationBuilder.DropTable(
                name: "PetsStray");

            migrationBuilder.DropTable(
                name: "Pets");
        }
    }
}
