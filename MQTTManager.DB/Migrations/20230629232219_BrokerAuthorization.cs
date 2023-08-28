using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MQTTManager.DB.Migrations
{
    /// <inheritdoc />
    public partial class BrokerAuthorization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrokerAuthorization",
                columns: table => new
                {
                    BrokerId = table.Column<int>(type: "integer", nullable: false),
                    Login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HashPassword = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerAuthorization", x => x.BrokerId);
                    table.ForeignKey(
                        name: "FK_BrokerAuthorization_BrokerConfig_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "BrokerConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerAuthorization");
        }
    }
}
