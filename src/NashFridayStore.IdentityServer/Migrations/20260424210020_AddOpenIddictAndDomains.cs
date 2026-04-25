using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NashFridayStore.IdentityServer.Migrations
{
    /// <inheritdoc />
    public partial class AddOpenIddictAndDomains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictApplications",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    application_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    client_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    client_secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    client_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    concurrency_token = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    consent_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    display_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_names = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    json_web_key_set = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    post_logout_redirect_uris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    redirect_uris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    requirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    settings = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_applications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictScopes",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    concurrency_token = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_names = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resources = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_scopes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    deleted_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    normalized_user_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    security_stamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "bit", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "bit", nullable: false),
                    access_failed_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    claim_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    claim_value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "auth",
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictAuthorizations",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    application_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    concurrency_token = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scopes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_authorizations", x => x.id);
                    table.ForeignKey(
                        name: "fk_open_iddict_authorizations_open_iddict_applications_application_id",
                        column: x => x.application_id,
                        principalSchema: "auth",
                        principalTable: "OpenIddictApplications",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "admins",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.id);
                    table.ForeignKey(
                        name: "fk_admins_users_id",
                        column: x => x.id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    claim_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    claim_value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "auth",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    provider_key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    provider_display_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "auth",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "auth",
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "auth",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    login_provider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                    table.ForeignKey(
                        name: "fk_customers_users_id",
                        column: x => x.id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictTokens",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    application_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    authorization_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    concurrency_token = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    expiration_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    redemption_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reference_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    type = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_open_iddict_tokens_open_iddict_applications_application_id",
                        column: x => x.application_id,
                        principalSchema: "auth",
                        principalTable: "OpenIddictApplications",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_open_iddict_tokens_open_iddict_authorizations_authorization_id",
                        column: x => x.authorization_id,
                        principalSchema: "auth",
                        principalTable: "OpenIddictAuthorizations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                schema: "auth",
                table: "AspNetRoleClaims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "auth",
                table: "AspNetRoles",
                column: "normalized_name",
                unique: true,
                filter: "[normalized_name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                schema: "auth",
                table: "AspNetUserClaims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                schema: "auth",
                table: "AspNetUserLogins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id",
                schema: "auth",
                table: "AspNetUserRoles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_applications_client_id",
                schema: "auth",
                table: "OpenIddictApplications",
                column: "client_id",
                unique: true,
                filter: "[client_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_authorizations_application_id_status_subject_type",
                schema: "auth",
                table: "OpenIddictAuthorizations",
                columns: new[] { "application_id", "status", "subject", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_scopes_name",
                schema: "auth",
                table: "OpenIddictScopes",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_tokens_application_id_status_subject_type",
                schema: "auth",
                table: "OpenIddictTokens",
                columns: new[] { "application_id", "status", "subject", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_tokens_authorization_id",
                schema: "auth",
                table: "OpenIddictTokens",
                column: "authorization_id");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_tokens_reference_id",
                schema: "auth",
                table: "OpenIddictTokens",
                column: "reference_id",
                unique: true,
                filter: "[reference_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "auth",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "ix_users_is_deleted",
                schema: "auth",
                table: "users",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "auth",
                table: "users",
                column: "normalized_user_name",
                unique: true,
                filter: "[normalized_user_name] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "customers",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "OpenIddictScopes",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "OpenIddictTokens",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "users",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "OpenIddictAuthorizations",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "OpenIddictApplications",
                schema: "auth");
        }
    }
}
