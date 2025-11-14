using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoUniversity.Migrations
{
    /// <inheritdoc />
    public partial class simplifyModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_InstructorID",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_InstructorID",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "InstructorID",
                table: "Departments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstructorID",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_InstructorID",
                table: "Departments",
                column: "InstructorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_InstructorID",
                table: "Departments",
                column: "InstructorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
