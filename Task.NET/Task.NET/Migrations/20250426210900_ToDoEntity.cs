using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.NET.Migrations
{
    /// <inheritdoc />
    public partial class ToDoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDos",
                columns: table => new
                {
                    iId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    dtDateTimeOfExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iComplete = table.Column<int>(type: "int", nullable: false),
                    iInsertedUserId = table.Column<int>(type: "int", nullable: false),
                    dtInsertedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    iUpdatedUserId = table.Column<int>(type: "int", nullable: true),
                    dtUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDos", x => x.iId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDos");
        }
    }
}
