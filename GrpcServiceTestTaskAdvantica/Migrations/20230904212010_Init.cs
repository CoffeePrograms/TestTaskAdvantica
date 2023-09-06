using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GrpcServiceTestTaskAdvantica.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Worker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Birthday = table.Column<long>(type: "bigint", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    HaveChildren = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Worker",
                columns: new[] { "Id", "Birthday", "FirstName", "HaveChildren", "LastName", "MiddleName", "Sex" },
                values: new object[,]
                {
                    { 1, 627670080000000000L, "Ivan", false, "Ivanov", "Ivanovich", 1 },
                    { 2, 627145632000000000L, "Petr", true, "Petrov", "Petrovich", 1 },
                    { 3, 621412992000000000L, "Nika", false, "Niloaeva", "Nikolaebna", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Worker");
        }
    }
}
