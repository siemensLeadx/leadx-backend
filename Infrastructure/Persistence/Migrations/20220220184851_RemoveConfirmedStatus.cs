using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class RemoveConfirmedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from LeadStatusHistory where StatusId = 3;");
            migrationBuilder.Sql("delete from Notifications where LeadStatusId = 3;");
            migrationBuilder.Sql("update Leads set CurrentLeadStatusId = 2 where CurrentLeadStatusId = 3;");

            migrationBuilder.DeleteData(
                table: "LeadStatuses",
                keyColumn: "Id",
                keyValue: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {          
            migrationBuilder.InsertData(
                table: "LeadStatuses",
                columns: new[] { "Id", "BackgroundColor", "NameAr", "NameEn", "TextColor" },
                values: new object[] { 3, "#cac8e0", "مؤكدة", "Confirmed", "#2b2483" });
        }
    }
}
