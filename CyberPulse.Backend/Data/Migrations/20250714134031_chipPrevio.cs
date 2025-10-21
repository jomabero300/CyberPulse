using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberPulse.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class chipPrevio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sataus",
                schema: "Gene",
                table: "Sataus");

            migrationBuilder.DropIndex(
                name: "IX_Sataus_Name",
                schema: "Gene",
                table: "Sataus");

            migrationBuilder.EnsureSchema(
                name: "Chip");

            migrationBuilder.RenameTable(
                name: "Sataus",
                schema: "Gene",
                newName: "Status",
                newSchema: "Gene");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                schema: "Admi",
                table: "AspNetUsers",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "Admi",
                table: "AspNetUsers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "Admi",
                table: "AspNetUsers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                schema: "Admi",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentId",
                schema: "Admi",
                table: "AspNetUsers",
                type: "varchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Nivel",
                schema: "Gene",
                table: "Status",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                schema: "Gene",
                table: "Status",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "Gene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Image = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriorityBets",
                schema: "Chip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorityBets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPrograms",
                schema: "Chip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TriningLevels",
                schema: "Chip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriningLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfPoblations",
                schema: "Chip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfPoblations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfTrainings",
                schema: "Chip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfTrainings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                schema: "Gene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Gene",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChipPrograms",
                schema: "Chip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Designation = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    PriorityBetId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    SupportFic = table.Column<bool>(type: "bit", nullable: false),
                    TriningLevelId = table.Column<int>(type: "int", nullable: false),
                    TypeOfTraining = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    WingMeasure = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChipPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChipPrograms_PriorityBets_PriorityBetId",
                        column: x => x.PriorityBetId,
                        principalSchema: "Chip",
                        principalTable: "PriorityBets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChipPrograms_TriningLevels_TriningLevelId",
                        column: x => x.TriningLevelId,
                        principalSchema: "Chip",
                        principalTable: "TriningLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "Gene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalSchema: "Gene",
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Neighborhoods",
                schema: "Gene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neighborhoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Neighborhoods_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Gene",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chips",
                schema: "Chip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apprentices = table.Column<int>(type: "int", nullable: false),
                    ChipNo = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    ChipProgramId = table.Column<int>(type: "int", nullable: false),
                    Company = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    AlertDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    NeighborhoodId = table.Column<int>(type: "int", nullable: false),
                    TrainingProgramId = table.Column<int>(type: "int", nullable: false),
                    TypeOfTrainingId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Justification = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Monday = table.Column<string>(type: "varchar(23)", maxLength: 23, nullable: false),
                    Tuesday = table.Column<string>(type: "varchar(23)", maxLength: 23, nullable: false),
                    Wednesday = table.Column<string>(type: "varchar(23)", maxLength: 23, nullable: false),
                    Tursday = table.Column<string>(type: "varchar(23)", maxLength: 23, nullable: false),
                    Friday = table.Column<string>(type: "varchar(23)", maxLength: 23, nullable: false),
                    Saturday = table.Column<string>(type: "varchar(23)", maxLength: 23, nullable: false),
                    Sunday = table.Column<string>(type: "varchar(23)", maxLength: 23, nullable: false),
                    StatuId = table.Column<int>(type: "int", nullable: false),
                    idEsta = table.Column<bool>(type: "bit", nullable: false),
                    Holiday = table.Column<bool>(type: "bit", nullable: false),
                    SentStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chips_AspNetUsers_InstructorId",
                        column: x => x.InstructorId,
                        principalSchema: "Admi",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chips_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Admi",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chips_ChipPrograms_ChipProgramId",
                        column: x => x.ChipProgramId,
                        principalSchema: "Chip",
                        principalTable: "ChipPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chips_Neighborhoods_NeighborhoodId",
                        column: x => x.NeighborhoodId,
                        principalSchema: "Gene",
                        principalTable: "Neighborhoods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chips_Status_StatuId",
                        column: x => x.StatuId,
                        principalSchema: "Gene",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chips_TrainingPrograms_TrainingProgramId",
                        column: x => x.TrainingProgramId,
                        principalSchema: "Chip",
                        principalTable: "TrainingPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chips_TypeOfTrainings_TypeOfTrainingId",
                        column: x => x.TypeOfTrainingId,
                        principalSchema: "Chip",
                        principalTable: "TypeOfTrainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChipPoblations",
                schema: "Chip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChipId = table.Column<int>(type: "int", nullable: false),
                    TypePoblationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChipPoblations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChipPoblations_Chips_ChipId",
                        column: x => x.ChipId,
                        principalSchema: "Chip",
                        principalTable: "Chips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChipPoblations_TypeOfPoblations_TypePoblationId",
                        column: x => x.TypePoblationId,
                        principalSchema: "Chip",
                        principalTable: "TypeOfPoblations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CountryId",
                schema: "Admi",
                table: "AspNetUsers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Status_Name_Nivel",
                schema: "Gene",
                table: "Status",
                columns: new[] { "Name", "Nivel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChipPoblations_ChipId",
                schema: "Chip",
                table: "ChipPoblations",
                column: "ChipId");

            migrationBuilder.CreateIndex(
                name: "IX_ChipPoblations_TypePoblationId_ChipId",
                schema: "Chip",
                table: "ChipPoblations",
                columns: new[] { "TypePoblationId", "ChipId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChipPrograms_Code_Version",
                schema: "Chip",
                table: "ChipPrograms",
                columns: new[] { "Code", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChipPrograms_PriorityBetId",
                schema: "Chip",
                table: "ChipPrograms",
                column: "PriorityBetId");

            migrationBuilder.CreateIndex(
                name: "IX_ChipPrograms_TriningLevelId",
                schema: "Chip",
                table: "ChipPrograms",
                column: "TriningLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Chips_ChipProgramId",
                schema: "Chip",
                table: "Chips",
                column: "ChipProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Chips_InstructorId",
                schema: "Chip",
                table: "Chips",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Chips_NeighborhoodId",
                schema: "Chip",
                table: "Chips",
                column: "NeighborhoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Chips_StatuId",
                schema: "Chip",
                table: "Chips",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Chips_TrainingProgramId",
                schema: "Chip",
                table: "Chips",
                column: "TrainingProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Chips_TypeOfTrainingId",
                schema: "Chip",
                table: "Chips",
                column: "TypeOfTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_Chips_UserId",
                schema: "Chip",
                table: "Chips",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId_Name",
                schema: "Gene",
                table: "Cities",
                columns: new[] { "StateId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                schema: "Gene",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Neighborhoods_CityId_Name",
                schema: "Gene",
                table: "Neighborhoods",
                columns: new[] { "CityId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriorityBets_Name",
                schema: "Chip",
                table: "PriorityBets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId_Name",
                schema: "Gene",
                table: "States",
                columns: new[] { "CountryId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_Name",
                schema: "Chip",
                table: "TrainingPrograms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriningLevels_Name",
                schema: "Chip",
                table: "TriningLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfPoblations_Name",
                schema: "Chip",
                table: "TypeOfPoblations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfTrainings_Name",
                schema: "Chip",
                table: "TypeOfTrainings",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Countries_CountryId",
                schema: "Admi",
                table: "AspNetUsers",
                column: "CountryId",
                principalSchema: "Gene",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Countries_CountryId",
                schema: "Admi",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ChipPoblations",
                schema: "Chip");

            migrationBuilder.DropTable(
                name: "Chips",
                schema: "Chip");

            migrationBuilder.DropTable(
                name: "TypeOfPoblations",
                schema: "Chip");

            migrationBuilder.DropTable(
                name: "ChipPrograms",
                schema: "Chip");

            migrationBuilder.DropTable(
                name: "Neighborhoods",
                schema: "Gene");

            migrationBuilder.DropTable(
                name: "TrainingPrograms",
                schema: "Chip");

            migrationBuilder.DropTable(
                name: "TypeOfTrainings",
                schema: "Chip");

            migrationBuilder.DropTable(
                name: "PriorityBets",
                schema: "Chip");

            migrationBuilder.DropTable(
                name: "TriningLevels",
                schema: "Chip");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "Gene");

            migrationBuilder.DropTable(
                name: "States",
                schema: "Gene");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "Gene");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CountryId",
                schema: "Admi",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                schema: "Gene",
                table: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Status_Name_Nivel",
                schema: "Gene",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "Admi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                schema: "Admi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nivel",
                schema: "Gene",
                table: "Status");

            migrationBuilder.RenameTable(
                name: "Status",
                schema: "Gene",
                newName: "Sataus",
                newSchema: "Gene");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                schema: "Admi",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "Admi",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "Admi",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sataus",
                schema: "Gene",
                table: "Sataus",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sataus_Name",
                schema: "Gene",
                table: "Sataus",
                column: "Name",
                unique: true);
        }
    }
}
