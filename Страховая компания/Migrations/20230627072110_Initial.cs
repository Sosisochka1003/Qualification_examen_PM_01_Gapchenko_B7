using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Страховая_компания.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    cid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    patronymic = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    passport_series = table.Column<int>(type: "integer", nullable: false),
                    passport_number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.cid);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    eid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    branch = table.Column<int>(type: "integer", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    patronymic = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.eid);
                });

            migrationBuilder.CreateTable(
                name: "objectofinsurance",
                columns: table => new
                {
                    ooiid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_objectofinsurance", x => x.ooiid);
                });

            migrationBuilder.CreateTable(
                name: "treaty",
                columns: table => new
                {
                    tid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_of_conclusion = table.Column<DateOnly>(type: "date", nullable: false),
                    id_client = table.Column<int>(type: "integer", nullable: false),
                    clientscid = table.Column<int>(type: "integer", nullable: false),
                    id_emp = table.Column<int>(type: "integer", nullable: false),
                    employeeseid = table.Column<int>(type: "integer", nullable: false),
                    id_object = table.Column<int>(type: "integer", nullable: false),
                    objectsooiid = table.Column<int>(type: "integer", nullable: false),
                    insurance_payment = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_treaty", x => x.tid);
                    table.ForeignKey(
                        name: "FK_treaty_client_clientscid",
                        column: x => x.clientscid,
                        principalTable: "client",
                        principalColumn: "cid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_treaty_employee_employeeseid",
                        column: x => x.employeeseid,
                        principalTable: "employee",
                        principalColumn: "eid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_treaty_objectofinsurance_objectsooiid",
                        column: x => x.objectsooiid,
                        principalTable: "objectofinsurance",
                        principalColumn: "ooiid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "insurancepayment",
                columns: table => new
                {
                    ipid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    id_treaty = table.Column<int>(type: "integer", nullable: false),
                    treatytid = table.Column<int>(type: "integer", nullable: false),
                    payout_amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurancepayment", x => x.ipid);
                    table.ForeignKey(
                        name: "FK_insurancepayment_treaty_treatytid",
                        column: x => x.treatytid,
                        principalTable: "treaty",
                        principalColumn: "tid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_insurancepayment_treatytid",
                table: "insurancepayment",
                column: "treatytid");

            migrationBuilder.CreateIndex(
                name: "IX_treaty_clientscid",
                table: "treaty",
                column: "clientscid");

            migrationBuilder.CreateIndex(
                name: "IX_treaty_employeeseid",
                table: "treaty",
                column: "employeeseid");

            migrationBuilder.CreateIndex(
                name: "IX_treaty_objectsooiid",
                table: "treaty",
                column: "objectsooiid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "insurancepayment");

            migrationBuilder.DropTable(
                name: "treaty");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "objectofinsurance");
        }
    }
}
