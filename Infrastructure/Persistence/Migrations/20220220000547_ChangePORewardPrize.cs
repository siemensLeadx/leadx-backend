using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class ChangePORewardPrize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 1, 1 },
                column: "LeadWithPOPrize",
                value: 1500m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 1, 2 },
                column: "LeadWithPOPrize",
                value: 2000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 1, 3 },
                column: "LeadWithPOPrize",
                value: 2500m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 1, 4 },
                column: "LeadWithPOPrize",
                value: 3000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 2, 1 },
                column: "LeadWithPOPrize",
                value: 1000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 2, 2 },
                column: "LeadWithPOPrize",
                value: 1500m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 2, 3 },
                column: "LeadWithPOPrize",
                value: 2000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 2, 4 },
                column: "LeadWithPOPrize",
                value: 2500m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 3, 1 },
                column: "LeadWithPOPrize",
                value: 500m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 3, 2 },
                column: "LeadWithPOPrize",
                value: 1000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 3, 3 },
                column: "LeadWithPOPrize",
                value: 1500m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 3, 4 },
                column: "LeadWithPOPrize",
                value: 2000m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 1, 1 },
                column: "LeadWithPOPrize",
                value: 3000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 1, 2 },
                column: "LeadWithPOPrize",
                value: 4000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 1, 3 },
                column: "LeadWithPOPrize",
                value: 5000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 1, 4 },
                column: "LeadWithPOPrize",
                value: 6000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 2, 1 },
                column: "LeadWithPOPrize",
                value: 2000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 2, 2 },
                column: "LeadWithPOPrize",
                value: 3000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 2, 3 },
                column: "LeadWithPOPrize",
                value: 4000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 2, 4 },
                column: "LeadWithPOPrize",
                value: 5000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 3, 1 },
                column: "LeadWithPOPrize",
                value: 1000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 3, 2 },
                column: "LeadWithPOPrize",
                value: 2000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 3, 3 },
                column: "LeadWithPOPrize",
                value: 3000m);

            migrationBuilder.UpdateData(
                table: "RewardPrizes",
                keyColumns: new[] { "RewardClassId", "RewardCriteriaId" },
                keyValues: new object[] { 3, 4 },
                column: "LeadWithPOPrize",
                value: 4000m);
        }
    }
}
