using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticClinicManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddingDoctorToTreatmentPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppTreatmentPlans_DoctorId",
                table: "AppTreatmentPlans",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTreatmentPlans_AbpUsers_DoctorId",
                table: "AppTreatmentPlans",
                column: "DoctorId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTreatmentPlans_AbpUsers_DoctorId",
                table: "AppTreatmentPlans");

            migrationBuilder.DropIndex(
                name: "IX_AppTreatmentPlans_DoctorId",
                table: "AppTreatmentPlans");
        }
    }
}
