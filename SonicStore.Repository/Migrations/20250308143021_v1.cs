using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonicStore.Repository.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    register_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    google_account = table.Column<bool>(type: "bit", nullable: false),
                    by_admin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__3213E83F1227836C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brand_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    brand_image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Brand__3213E83F07E0250E", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3213E83F4E6EE1EC", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total_price = table.Column<double>(type: "float", nullable: true),
                    payment_method = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    transaction_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__3213E83F31074084", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3213E83F5E85A4EC", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    update_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    brand_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__3213E83FD41103F3", x => x.id);
                    table.ForeignKey(
                        name: "FK_Product_Category_category_id",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Product__brand_i__4924D839",
                        column: x => x.brand_id,
                        principalTable: "Brand",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "status_payment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    update_by = table.Column<int>(type: "int", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    create_by = table.Column<int>(type: "int", nullable: true),
                    payment_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__status_payment__3213E83F", x => x.id);
                    table.ForeignKey(
                        name: "FK_status_payment_Payment_payment_id",
                        column: x => x.payment_id,
                        principalTable: "Payment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dob = table.Column<DateTime>(type: "datetime", nullable: false),
                    email = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    update_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    update_by = table.Column<int>(type: "int", nullable: false),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3213E83FD23B5FEA", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__User__account_id__3DB3258D",
                        column: x => x.account_id,
                        principalTable: "Account",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Product_Image",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product___3213E83F88F164DB", x => x.id);
                    table.ForeignKey(
                        name: "FK__Product_I__produ__4CF5691D",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storage = table.Column<int>(type: "int", nullable: false),
                    original_price = table.Column<double>(type: "float", nullable: false),
                    sale_price = table.Column<double>(type: "float", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.id);
                    table.ForeignKey(
                        name: "FK_Storage_Product_product_id",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    approved_by = table.Column<int>(type: "int", nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.id);
                    table.ForeignKey(
                        name: "FK_Campaigns_User_created_by",
                        column: x => x.created_by,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Address",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User_Add__3213E83F371BAF55", x => x.id);
                    table.ForeignKey(
                        name: "FK__User_Addr__user___4183B671",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Budgets_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storage_id = table.Column<int>(type: "int", nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    user_address_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => new { x.storage_id, x.customer_id, x.user_address_id, x.id });
                    table.UniqueConstraint("AK_Cart_id", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cart_Storage_storage_id",
                        column: x => x.storage_id,
                        principalTable: "Storage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cart_User_Address_user_address_id",
                        column: x => x.user_address_id,
                        principalTable: "User_Address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cart_User_customer_id",
                        column: x => x.customer_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Checkout",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    sale_id = table.Column<int>(type: "int", nullable: true),
                    cart_id = table.Column<int>(type: "int", nullable: true),
                    payment_id = table.Column<int>(type: "int", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__3213E83FCBD4A6FA", x => x.id);
                    table.ForeignKey(
                        name: "FK_Checkout_Cart_cart_id",
                        column: x => x.cart_id,
                        principalTable: "Cart",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Order__payment_i__62E4AA3C",
                        column: x => x.payment_id,
                        principalTable: "Payment",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "status_order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    update_by = table.Column<int>(type: "int", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    create_by = table.Column<int>(type: "int", nullable: true),
                    checkout_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Status__3213E83F97D90152", x => x.id);
                    table.ForeignKey(
                        name: "FK_status_order_Checkout_checkout_id",
                        column: x => x.checkout_id,
                        principalTable: "Checkout",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Account__F3DBC572D661B57A",
                table: "Account",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CampaignId",
                table: "Budgets",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_created_by",
                table: "Campaigns",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_customer_id",
                table: "Cart",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_user_address_id",
                table: "Cart",
                column: "user_address_id");

            migrationBuilder.CreateIndex(
                name: "IX_Checkout_cart_id",
                table: "Checkout",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_Checkout_payment_id",
                table: "Checkout",
                column: "payment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_brand_id",
                table: "Product",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_category_id",
                table: "Product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Image_product_id",
                table: "Product_Image",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_status_order_checkout_id",
                table: "status_order",
                column: "checkout_id");

            migrationBuilder.CreateIndex(
                name: "IX_status_payment_payment_id",
                table: "status_payment",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_product_id",
                table: "Storage",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__User__46A222CC47970D57",
                table: "User",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__User__AB6E6164D8A3A270",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__User__B43B145F81F86A8C",
                table: "User",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Address_user_id",
                table: "User_Address",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "Product_Image");

            migrationBuilder.DropTable(
                name: "status_order");

            migrationBuilder.DropTable(
                name: "status_payment");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Checkout");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Storage");

            migrationBuilder.DropTable(
                name: "User_Address");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
