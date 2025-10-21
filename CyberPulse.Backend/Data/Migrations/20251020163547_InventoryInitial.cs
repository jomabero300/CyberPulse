using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberPulse.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InventoryInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Inve");

            migrationBuilder.CreateTable(
                name: "BudgetTypes",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ivas",
                schema: "Gene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    Worth = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ivas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ivas_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lots",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lots_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programs_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Segments",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segments_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnitMeasurements",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Symbol = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BaseValue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitMeasurements_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Validities",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Validities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Validities_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgramLots",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    LotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramLots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramLots_Lots_LotId",
                        column: x => x.LotId,
                        principalSchema: "Inve",
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramLots_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalSchema: "Inve",
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Families_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalSchema: "Inve",
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Families_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValidityId = table.Column<int>(type: "int", nullable: false),
                    BudgetTypeId = table.Column<int>(type: "int", nullable: false),
                    Rubro = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    Worth = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budgets_BudgetTypes_BudgetTypeId",
                        column: x => x.BudgetTypeId,
                        principalSchema: "Inve",
                        principalTable: "BudgetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Budgets_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Budgets_Validities_ValidityId",
                        column: x => x.ValidityId,
                        principalSchema: "Inve",
                        principalTable: "Validities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseProgramLots",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    ProgramLotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseProgramLots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseProgramLots_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Inve",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseProgramLots_ProgramLots_ProgramLotId",
                        column: x => x.ProgramLotId,
                        principalSchema: "Inve",
                        principalTable: "ProgramLots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    FamilyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalSchema: "Inve",
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPrograms",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    BudgetTypeId = table.Column<int>(type: "int", nullable: false),
                    ValidityId = table.Column<int>(type: "int", nullable: false),
                    Worth = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetPrograms_BudgetTypes_BudgetTypeId",
                        column: x => x.BudgetTypeId,
                        principalSchema: "Inve",
                        principalTable: "BudgetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetPrograms_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "Inve",
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetPrograms_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalSchema: "Inve",
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetPrograms_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetPrograms_Validities_ValidityId",
                        column: x => x.ValidityId,
                        principalSchema: "Inve",
                        principalTable: "Validities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    UnitMeasurementId = table.Column<int>(type: "int", nullable: false),
                    ClasseId = table.Column<int>(type: "int", nullable: false),
                    LotId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Classes_ClasseId",
                        column: x => x.ClasseId,
                        principalSchema: "Inve",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Lots_LotId",
                        column: x => x.LotId,
                        principalSchema: "Inve",
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_UnitMeasurements_UnitMeasurementId",
                        column: x => x.UnitMeasurementId,
                        principalSchema: "Inve",
                        principalTable: "UnitMeasurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetLots",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetProgramId = table.Column<int>(type: "int", nullable: false),
                    ProgramLotId = table.Column<int>(type: "int", nullable: false),
                    ValidityId = table.Column<int>(type: "int", nullable: false),
                    Worth = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLots_BudgetPrograms_BudgetProgramId",
                        column: x => x.BudgetProgramId,
                        principalSchema: "Inve",
                        principalTable: "BudgetPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetLots_ProgramLots_ProgramLotId",
                        column: x => x.ProgramLotId,
                        principalSchema: "Inve",
                        principalTable: "ProgramLots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetLots_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetLots_Validities_ValidityId",
                        column: x => x.ValidityId,
                        principalSchema: "Inve",
                        principalTable: "Validities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCurrentValues",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValidityId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    IvaId = table.Column<int>(type: "int", nullable: false),
                    Worth = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(3,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCurrentValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCurrentValues_Ivas_IvaId",
                        column: x => x.IvaId,
                        principalSchema: "Gene",
                        principalTable: "Ivas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCurrentValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Inve",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCurrentValues_Validities_ValidityId",
                        column: x => x.ValidityId,
                        principalSchema: "Inve",
                        principalTable: "Validities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetCourses",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetLotId = table.Column<int>(type: "int", nullable: false),
                    ValidityId = table.Column<int>(type: "int", nullable: false),
                    CourseProgramLotId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Worth = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCourses", x => x.Id);
                    table.CheckConstraint("CK_EndDateGreaterthanInitial", "[EndDate] > [StartDate]");
                    table.ForeignKey(
                        name: "FK_BudgetCourses_BudgetLots_BudgetLotId",
                        column: x => x.BudgetLotId,
                        principalSchema: "Inve",
                        principalTable: "BudgetLots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetCourses_CourseProgramLots_CourseProgramLotId",
                        column: x => x.CourseProgramLotId,
                        principalSchema: "Inve",
                        principalTable: "CourseProgramLots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetCourses_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetCourses_Validities_ValidityId",
                        column: x => x.ValidityId,
                        principalSchema: "Inve",
                        principalTable: "Validities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductQuotations",
                schema: "Inve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetCourseId = table.Column<int>(type: "int", nullable: false),
                    ProductCurrentValueId = table.Column<int>(type: "int", nullable: false),
                    RequestedQuantity = table.Column<int>(type: "int", nullable: false),
                    AcceptedQuantity = table.Column<int>(type: "int", nullable: false),
                    QuotedValue = table.Column<decimal>(type: "decimal(14,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQuotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductQuotations_BudgetCourses_BudgetCourseId",
                        column: x => x.BudgetCourseId,
                        principalSchema: "Inve",
                        principalTable: "BudgetCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductQuotations_ProductCurrentValues_ProductCurrentValueId",
                        column: x => x.ProductCurrentValueId,
                        principalSchema: "Inve",
                        principalTable: "ProductCurrentValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCourses_BudgetLotId",
                schema: "Inve",
                table: "BudgetCourses",
                column: "BudgetLotId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCourses_CourseProgramLotId",
                schema: "Inve",
                table: "BudgetCourses",
                column: "CourseProgramLotId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCourses_StatuId",
                schema: "Inve",
                table: "BudgetCourses",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCourses_ValidityId_CourseProgramLotId_StartDate",
                schema: "Inve",
                table: "BudgetCourses",
                columns: new[] { "ValidityId", "CourseProgramLotId", "StartDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLots_BudgetProgramId",
                schema: "Inve",
                table: "BudgetLots",
                column: "BudgetProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLots_ProgramLotId",
                schema: "Inve",
                table: "BudgetLots",
                column: "ProgramLotId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLots_StatuId",
                schema: "Inve",
                table: "BudgetLots",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLots_ValidityId",
                schema: "Inve",
                table: "BudgetLots",
                column: "ValidityId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPrograms_BudgetId",
                schema: "Inve",
                table: "BudgetPrograms",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPrograms_BudgetTypeId",
                schema: "Inve",
                table: "BudgetPrograms",
                column: "BudgetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPrograms_ProgramId",
                schema: "Inve",
                table: "BudgetPrograms",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPrograms_StatuId",
                schema: "Inve",
                table: "BudgetPrograms",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPrograms_ValidityId",
                schema: "Inve",
                table: "BudgetPrograms",
                column: "ValidityId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetTypeId",
                schema: "Inve",
                table: "Budgets",
                column: "BudgetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_StatuId",
                schema: "Inve",
                table: "Budgets",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_ValidityId",
                schema: "Inve",
                table: "Budgets",
                column: "ValidityId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTypes_Name",
                schema: "Inve",
                table: "BudgetTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Code",
                schema: "Inve",
                table: "Classes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_FamilyId_Code_Name",
                schema: "Inve",
                table: "Classes",
                columns: new[] { "FamilyId", "Code", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_StatuId",
                schema: "Inve",
                table: "Classes",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgramLots_CourseId_ProgramLotId",
                schema: "Inve",
                table: "CourseProgramLots",
                columns: new[] { "CourseId", "ProgramLotId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgramLots_ProgramLotId",
                schema: "Inve",
                table: "CourseProgramLots",
                column: "ProgramLotId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Name",
                schema: "Inve",
                table: "Courses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StatuId",
                schema: "Inve",
                table: "Courses",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_Code",
                schema: "Inve",
                table: "Families",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Families_SegmentId_Code_Name",
                schema: "Inve",
                table: "Families",
                columns: new[] { "SegmentId", "Code", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Families_StatuId",
                schema: "Inve",
                table: "Families",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Ivas_Name",
                schema: "Gene",
                table: "Ivas",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ivas_StatuId",
                schema: "Gene",
                table: "Ivas",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_Name",
                schema: "Inve",
                table: "Lots",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_StatuId",
                schema: "Inve",
                table: "Lots",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCurrentValues_IvaId",
                schema: "Inve",
                table: "ProductCurrentValues",
                column: "IvaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCurrentValues_ProductId",
                schema: "Inve",
                table: "ProductCurrentValues",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCurrentValues_ValidityId_ProductId",
                schema: "Inve",
                table: "ProductCurrentValues",
                columns: new[] { "ValidityId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuotations_BudgetCourseId",
                schema: "Inve",
                table: "ProductQuotations",
                column: "BudgetCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuotations_ProductCurrentValueId",
                schema: "Inve",
                table: "ProductQuotations",
                column: "ProductCurrentValueId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ClasseId_Name",
                schema: "Inve",
                table: "Products",
                columns: new[] { "ClasseId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                schema: "Inve",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_LotId",
                schema: "Inve",
                table: "Products",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StatuId",
                schema: "Inve",
                table: "Products",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitMeasurementId",
                schema: "Inve",
                table: "Products",
                column: "UnitMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramLots_LotId",
                schema: "Inve",
                table: "ProgramLots",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramLots_ProgramId_LotId",
                schema: "Inve",
                table: "ProgramLots",
                columns: new[] { "ProgramId", "LotId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_Name",
                schema: "Inve",
                table: "Programs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_StatuId",
                schema: "Inve",
                table: "Programs",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Code",
                schema: "Inve",
                table: "Segments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Name",
                schema: "Inve",
                table: "Segments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Segments_StatuId",
                schema: "Inve",
                table: "Segments",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitMeasurements_Name",
                schema: "Inve",
                table: "UnitMeasurements",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitMeasurements_StatuId",
                schema: "Inve",
                table: "UnitMeasurements",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Validities_StatuId",
                schema: "Inve",
                table: "Validities",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Validities_Value",
                schema: "Inve",
                table: "Validities",
                column: "Value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductQuotations",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "BudgetCourses",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "ProductCurrentValues",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "BudgetLots",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "CourseProgramLots",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "Ivas",
                schema: "Gene");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "BudgetPrograms",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "ProgramLots",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "Classes",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "UnitMeasurements",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "Lots",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "Programs",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "Families",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "BudgetTypes",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "Validities",
                schema: "Inve");

            migrationBuilder.DropTable(
                name: "Segments",
                schema: "Inve");
        }
    }
}
