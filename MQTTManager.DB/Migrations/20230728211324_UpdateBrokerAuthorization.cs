using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MQTTManager.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBrokerAuthorization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Configuration",
                table: "BrokerConfig",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Configuration",
                table: "BrokerConfig");
        }
    }
}
