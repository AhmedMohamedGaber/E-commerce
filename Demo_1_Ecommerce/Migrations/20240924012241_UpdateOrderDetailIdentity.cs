using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo_1_Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderDetailIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            // Drop the existing Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderDetails");

            // Recreate the Id column as an identity column
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderDetails",
                type: "int",
                nullable: false
            )
            .Annotation("SqlServer:Identity", "1, 1");

            // Add primary key constraint back
            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            // Drop the newly created Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderDetails");

            // Recreate the Id column without the identity
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0); // Ensure to set a default value

            // Add primary key constraint back
            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "Id");
        }
    }
}
