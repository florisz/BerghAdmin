using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    /// <inheritdoc />
    public partial class Extended1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LoginCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Betalingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Bedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DatumTijd = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    BetalingType = table.Column<int>(type: "int", nullable: false),
                    Munt = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Volgnummer = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tegenrekening = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NaamTegenpartij = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NaamUiteindelijkePartij = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NaamInitierendePartij = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BICTegenpartij = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BatchID = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TransactieReferentie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MachtigingsKenmerk = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncassantID = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BetalingsKenmerk = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Omschrijving1 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Omschrijving2 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Omschrijving3 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RedenRetour = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OorspronkelijkBedrag = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OorspronkelijkMunt = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Koers = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Betalingen", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BihzDonaties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DonationId = table.Column<int>(type: "int", nullable: false),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CreatieDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WijzigDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DonatieBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionKosten = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RegistratieFee = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RegistratieFeeBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotaalBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NettoBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    BetaalStatus = table.Column<int>(type: "int", nullable: false),
                    BetaalStatusOp = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    BetaalTransactieId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BetaalId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BetaalOmschrijving = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountIban = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountBic = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PersoonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BihzDonaties", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BihzProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Slug = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    CreatieDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WijzigDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExterneReferentie = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Titel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Omschrijving = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DoelBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotaalBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AantalDonaties = table.Column<int>(type: "int", nullable: false),
                    DoelBedragBereikt = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Zichtbaar = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Gesloten = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Beeindigd = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DonatieUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FietstochtId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BihzProjects", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Documenten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    TemplateType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<byte[]>(type: "longblob", nullable: false),
                    IsMergeTemplate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Owner = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documenten", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Donateur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmailAdres = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsVerwijderd = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Adres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Postcode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Plaats = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Land = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefoon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mobiel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Opmerkingen = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donateur", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fietstochten",
                columns: table => new
                {
                    FietstochtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeplandeDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    KentaaProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fietstochten", x => x.FietstochtId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Golfdagen",
                columns: table => new
                {
                    GolfdagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Locatie = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Titel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeplandeDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    KentaaProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Golfdagen", x => x.GolfdagId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rollen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Beschrijving = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MeervoudBeschrijving = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rollen", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Facturen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nummer = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Omschrijving = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bedrag = table.Column<float>(type: "float", precision: 18, scale: 2, nullable: true),
                    Datum = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsVerzonden = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FactuurType = table.Column<int>(type: "int", nullable: false),
                    EmailTekstId = table.Column<int>(type: "int", nullable: true),
                    FactuurTekstId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facturen_Documenten_EmailTekstId",
                        column: x => x.EmailTekstId,
                        principalTable: "Documenten",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Facturen_Documenten_FactuurTekstId",
                        column: x => x.FactuurTekstId,
                        principalTable: "Documenten",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VerzondenMails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Onderwerp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VerzendDatum = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    InhoudId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerzondenMails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerzondenMails_Documenten_InhoudId",
                        column: x => x.InhoudId,
                        principalTable: "Documenten",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Personen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Geslacht = table.Column<int>(type: "int", nullable: false),
                    Voorletters = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Voornaam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Achternaam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tussenvoegsel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EmailAdresExtra = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KledingMaten = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nummer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Handicap = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Buggy = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    IsReserve = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personen_BihzProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "BihzProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Personen_Donateur_Id",
                        column: x => x.Id,
                        principalTable: "Donateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Donaties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DatumTijd = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Omschrijving = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DonateurId = table.Column<int>(type: "int", nullable: true),
                    FactuurId = table.Column<int>(type: "int", nullable: true),
                    KentaaDonatieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donaties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donaties_BihzDonaties_KentaaDonatieId",
                        column: x => x.KentaaDonatieId,
                        principalTable: "BihzDonaties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Donaties_Donateur_DonateurId",
                        column: x => x.DonateurId,
                        principalTable: "Donateur",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Donaties_Facturen_FactuurId",
                        column: x => x.FactuurId,
                        principalTable: "Facturen",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BihzActies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Slug = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatieDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WijzigDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExterneReferentie = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Voornaam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tussenvoegsels = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Achternaam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Titel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Omschrijving = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DoelBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotaalBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AantalDonaties = table.Column<int>(type: "int", nullable: false),
                    DoelBedragBereikt = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Beeindigd = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DoneerUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PersoonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BihzActies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BihzActies_Personen_PersoonId",
                        column: x => x.PersoonId,
                        principalTable: "Personen",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BihzUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    CreatieDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WijzigDatum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Voornaam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tussenvoegsels = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Achternaam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adres2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Straat = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HuisNummer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HuisNummerToevoeging = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Postcode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Woonplaats = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Land = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefoon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Geslacht = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PersoonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BihzUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BihzUsers_Personen_PersoonId",
                        column: x => x.PersoonId,
                        principalTable: "Personen",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FietstochtPersoon",
                columns: table => new
                {
                    DeelnemersId = table.Column<int>(type: "int", nullable: false),
                    FietstochtenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FietstochtPersoon", x => new { x.DeelnemersId, x.FietstochtenId });
                    table.ForeignKey(
                        name: "FK_FietstochtPersoon_Fietstochten_FietstochtenId",
                        column: x => x.FietstochtenId,
                        principalTable: "Fietstochten",
                        principalColumn: "FietstochtId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FietstochtPersoon_Personen_DeelnemersId",
                        column: x => x.DeelnemersId,
                        principalTable: "Personen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GolfdagPersoon",
                columns: table => new
                {
                    DeelnemersId = table.Column<int>(type: "int", nullable: false),
                    GolfdagenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfdagPersoon", x => new { x.DeelnemersId, x.GolfdagenId });
                    table.ForeignKey(
                        name: "FK_GolfdagPersoon_Golfdagen_GolfdagenId",
                        column: x => x.GolfdagenId,
                        principalTable: "Golfdagen",
                        principalColumn: "GolfdagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GolfdagPersoon_Personen_DeelnemersId",
                        column: x => x.DeelnemersId,
                        principalTable: "Personen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MailbccGeadresseerden",
                columns: table => new
                {
                    bccGeadresseerdenId = table.Column<int>(type: "int", nullable: false),
                    bccGeadresseerdenId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailbccGeadresseerden", x => new { x.bccGeadresseerdenId, x.bccGeadresseerdenId1 });
                    table.ForeignKey(
                        name: "FK_MailbccGeadresseerden_Personen_bccGeadresseerdenId",
                        column: x => x.bccGeadresseerdenId,
                        principalTable: "Personen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailbccGeadresseerden_VerzondenMails_bccGeadresseerdenId1",
                        column: x => x.bccGeadresseerdenId1,
                        principalTable: "VerzondenMails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MailccGeadresseerden",
                columns: table => new
                {
                    ccGeadresseerdenId = table.Column<int>(type: "int", nullable: false),
                    ccGeadresseerdenId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailccGeadresseerden", x => new { x.ccGeadresseerdenId, x.ccGeadresseerdenId1 });
                    table.ForeignKey(
                        name: "FK_MailccGeadresseerden_Personen_ccGeadresseerdenId",
                        column: x => x.ccGeadresseerdenId,
                        principalTable: "Personen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailccGeadresseerden_VerzondenMails_ccGeadresseerdenId1",
                        column: x => x.ccGeadresseerdenId1,
                        principalTable: "VerzondenMails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MailGeadresseerden",
                columns: table => new
                {
                    GeadresseerdenId = table.Column<int>(type: "int", nullable: false),
                    GeadresseerdenId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailGeadresseerden", x => new { x.GeadresseerdenId, x.GeadresseerdenId1 });
                    table.ForeignKey(
                        name: "FK_MailGeadresseerden_Personen_GeadresseerdenId",
                        column: x => x.GeadresseerdenId,
                        principalTable: "Personen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailGeadresseerden_VerzondenMails_GeadresseerdenId1",
                        column: x => x.GeadresseerdenId1,
                        principalTable: "VerzondenMails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PersoonRol",
                columns: table => new
                {
                    PersonenId = table.Column<int>(type: "int", nullable: false),
                    RollenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersoonRol", x => new { x.PersonenId, x.RollenId });
                    table.ForeignKey(
                        name: "FK_PersoonRol_Personen_PersonenId",
                        column: x => x.PersonenId,
                        principalTable: "Personen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersoonRol_Rollen_RollenId",
                        column: x => x.RollenId,
                        principalTable: "Rollen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sponsoren",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DebiteurNummer = table.Column<int>(type: "int", nullable: true),
                    Naam = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactPersoonId = table.Column<int>(type: "int", nullable: false),
                    CompagnonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsoren", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sponsoren_Donateur_Id",
                        column: x => x.Id,
                        principalTable: "Donateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sponsoren_Personen_CompagnonId",
                        column: x => x.CompagnonId,
                        principalTable: "Personen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sponsoren_Personen_ContactPersoonId",
                        column: x => x.ContactPersoonId,
                        principalTable: "Personen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ambassadeur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ToegezegdBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotaalBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DatumAangebracht = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Pakket = table.Column<int>(type: "int", nullable: false),
                    Fax = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambassadeur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ambassadeur_Sponsoren_Id",
                        column: x => x.Id,
                        principalTable: "Sponsoren",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GolfdagSponsor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    GolfdagId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfdagSponsor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GolfdagSponsor_Golfdagen_GolfdagId",
                        column: x => x.GolfdagId,
                        principalTable: "Golfdagen",
                        principalColumn: "GolfdagId");
                    table.ForeignKey(
                        name: "FK_GolfdagSponsor_Sponsoren_Id",
                        column: x => x.Id,
                        principalTable: "Sponsoren",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BihzActies_ActionId",
                table: "BihzActies",
                column: "ActionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BihzActies_PersoonId",
                table: "BihzActies",
                column: "PersoonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BihzDonaties_DonationId",
                table: "BihzDonaties",
                column: "DonationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BihzProjects_ProjectId",
                table: "BihzProjects",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BihzUsers_PersoonId",
                table: "BihzUsers",
                column: "PersoonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BihzUsers_UserId",
                table: "BihzUsers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donaties_DonateurId",
                table: "Donaties",
                column: "DonateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Donaties_FactuurId",
                table: "Donaties",
                column: "FactuurId");

            migrationBuilder.CreateIndex(
                name: "IX_Donaties_KentaaDonatieId",
                table: "Donaties",
                column: "KentaaDonatieId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_EmailTekstId",
                table: "Facturen",
                column: "EmailTekstId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_FactuurTekstId",
                table: "Facturen",
                column: "FactuurTekstId");

            migrationBuilder.CreateIndex(
                name: "IX_FietstochtPersoon_FietstochtenId",
                table: "FietstochtPersoon",
                column: "FietstochtenId");

            migrationBuilder.CreateIndex(
                name: "IX_GolfdagPersoon_GolfdagenId",
                table: "GolfdagPersoon",
                column: "GolfdagenId");

            migrationBuilder.CreateIndex(
                name: "IX_GolfdagSponsor_GolfdagId",
                table: "GolfdagSponsor",
                column: "GolfdagId");

            migrationBuilder.CreateIndex(
                name: "IX_MailbccGeadresseerden_bccGeadresseerdenId1",
                table: "MailbccGeadresseerden",
                column: "bccGeadresseerdenId1");

            migrationBuilder.CreateIndex(
                name: "IX_MailccGeadresseerden_ccGeadresseerdenId1",
                table: "MailccGeadresseerden",
                column: "ccGeadresseerdenId1");

            migrationBuilder.CreateIndex(
                name: "IX_MailGeadresseerden_GeadresseerdenId1",
                table: "MailGeadresseerden",
                column: "GeadresseerdenId1");

            migrationBuilder.CreateIndex(
                name: "IX_Personen_ProjectId",
                table: "Personen",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PersoonRol_RollenId",
                table: "PersoonRol",
                column: "RollenId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsoren_CompagnonId",
                table: "Sponsoren",
                column: "CompagnonId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsoren_ContactPersoonId",
                table: "Sponsoren",
                column: "ContactPersoonId");

            migrationBuilder.CreateIndex(
                name: "IX_VerzondenMails_InhoudId",
                table: "VerzondenMails",
                column: "InhoudId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ambassadeur");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Betalingen");

            migrationBuilder.DropTable(
                name: "BihzActies");

            migrationBuilder.DropTable(
                name: "BihzUsers");

            migrationBuilder.DropTable(
                name: "Donaties");

            migrationBuilder.DropTable(
                name: "FietstochtPersoon");

            migrationBuilder.DropTable(
                name: "GolfdagPersoon");

            migrationBuilder.DropTable(
                name: "GolfdagSponsor");

            migrationBuilder.DropTable(
                name: "MailbccGeadresseerden");

            migrationBuilder.DropTable(
                name: "MailccGeadresseerden");

            migrationBuilder.DropTable(
                name: "MailGeadresseerden");

            migrationBuilder.DropTable(
                name: "PersoonRol");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BihzDonaties");

            migrationBuilder.DropTable(
                name: "Facturen");

            migrationBuilder.DropTable(
                name: "Fietstochten");

            migrationBuilder.DropTable(
                name: "Golfdagen");

            migrationBuilder.DropTable(
                name: "Sponsoren");

            migrationBuilder.DropTable(
                name: "VerzondenMails");

            migrationBuilder.DropTable(
                name: "Rollen");

            migrationBuilder.DropTable(
                name: "Personen");

            migrationBuilder.DropTable(
                name: "Documenten");

            migrationBuilder.DropTable(
                name: "BihzProjects");

            migrationBuilder.DropTable(
                name: "Donateur");
        }
    }
}
