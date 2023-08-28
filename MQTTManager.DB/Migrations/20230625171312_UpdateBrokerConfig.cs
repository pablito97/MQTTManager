using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MQTTManager.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBrokerConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "BrokerConfig",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "BrokerConfig");
        }
    }
}
