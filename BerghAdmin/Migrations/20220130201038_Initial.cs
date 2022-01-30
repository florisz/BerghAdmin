﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentPersoonId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Betalingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bedrag = table.Column<float>(type: "real", nullable: true),
                    DatumTijd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BetalingType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Betalingen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BihzDonaties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonationId = table.Column<int>(type: "int", nullable: false),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CreatieDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WijzigDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonatieBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionKosten = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RegistratieFee = table.Column<bool>(type: "bit", nullable: false),
                    RegistratieFeeBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotaalBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NettoBedrag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    BetaalStatus = table.Column<int>(type: "int", nullable: false),
                    BetaalStatusOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BetaalTransactieId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BetaalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BetaalOmschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountIban = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountBic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersoonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BihzDonaties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documenten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    TemplateType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsMergeTemplate = table.Column<bool>(type: "bit", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documenten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Evenementen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeplandeDatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evenementen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rollen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Beschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeervoudBeschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rollen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Facturen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bedrag = table.Column<float>(type: "real", nullable: true),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerzonden = table.Column<bool>(type: "bit", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "VerzondenMails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Onderwerp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerzendDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "BihzProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    CreatieDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WijzigDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExterneReferentie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoelBedrag = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotaalBedrag = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AantalDonaties = table.Column<int>(type: "int", nullable: false),
                    DoelBedragBereikt = table.Column<bool>(type: "bit", nullable: false),
                    Zichtbaar = table.Column<bool>(type: "bit", nullable: false),
                    Gesloten = table.Column<bool>(type: "bit", nullable: false),
                    Beeindigd = table.Column<bool>(type: "bit", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonatieUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvenementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BihzProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BihzProjects_Evenementen_EvenementId",
                        column: x => x.EvenementId,
                        principalTable: "Evenementen",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FietsTochten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FietsTochten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FietsTochten_Evenementen_Id",
                        column: x => x.Id,
                        principalTable: "Evenementen",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GolfDagen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Locatie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfDagen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GolfDagen_Evenementen_Id",
                        column: x => x.Id,
                        principalTable: "Evenementen",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Donateur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsVerwijderd = table.Column<bool>(type: "bit", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plaats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Land = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPersoonId = table.Column<int>(type: "int", nullable: true),
                    Geslacht = table.Column<int>(type: "int", nullable: true),
                    Voorletters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voornaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achternaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tussenvoegsel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Telefoon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobiel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAdres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAdresExtra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donateur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donateur_BihzProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "BihzProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Donateur_Donateur_ContactPersoonId",
                        column: x => x.ContactPersoonId,
                        principalTable: "Donateur",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BihzActies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatieDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WijzigDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExterneReferentie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voornaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tussenvoegsels = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achternaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoelBedrag = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotaalBedrag = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AantalDonaties = table.Column<int>(type: "int", nullable: false),
                    DoelBedragBereikt = table.Column<bool>(type: "bit", nullable: false),
                    Beeindigd = table.Column<bool>(type: "bit", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoneerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersoonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BihzActies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BihzActies_Donateur_PersoonId",
                        column: x => x.PersoonId,
                        principalTable: "Donateur",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BihzUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    CreatieDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WijzigDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Voornaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tussenvoegsels = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achternaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Straat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HuisNummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HuisNummerToevoeging = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Woonplaats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Land = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefoon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Geslacht = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersoonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BihzUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BihzUsers_Donateur_PersoonId",
                        column: x => x.PersoonId,
                        principalTable: "Donateur",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Donaties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                });

            migrationBuilder.CreateTable(
                name: "EvenementPersoon",
                columns: table => new
                {
                    DeelnemersId = table.Column<int>(type: "int", nullable: false),
                    IsDeelnemerVanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvenementPersoon", x => new { x.DeelnemersId, x.IsDeelnemerVanId });
                    table.ForeignKey(
                        name: "FK_EvenementPersoon_Donateur_DeelnemersId",
                        column: x => x.DeelnemersId,
                        principalTable: "Donateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvenementPersoon_Evenementen_IsDeelnemerVanId",
                        column: x => x.IsDeelnemerVanId,
                        principalTable: "Evenementen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_MailbccGeadresseerden_Donateur_bccGeadresseerdenId",
                        column: x => x.bccGeadresseerdenId,
                        principalTable: "Donateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailbccGeadresseerden_VerzondenMails_bccGeadresseerdenId1",
                        column: x => x.bccGeadresseerdenId1,
                        principalTable: "VerzondenMails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_MailccGeadresseerden_Donateur_ccGeadresseerdenId",
                        column: x => x.ccGeadresseerdenId,
                        principalTable: "Donateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailccGeadresseerden_VerzondenMails_ccGeadresseerdenId1",
                        column: x => x.ccGeadresseerdenId1,
                        principalTable: "VerzondenMails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_MailGeadresseerden_Donateur_GeadresseerdenId",
                        column: x => x.GeadresseerdenId,
                        principalTable: "Donateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailGeadresseerden_VerzondenMails_GeadresseerdenId1",
                        column: x => x.GeadresseerdenId1,
                        principalTable: "VerzondenMails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_PersoonRol_Donateur_PersonenId",
                        column: x => x.PersonenId,
                        principalTable: "Donateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersoonRol_Rollen_RollenId",
                        column: x => x.RollenId,
                        principalTable: "Rollen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BihzActies_PersoonId",
                table: "BihzActies",
                column: "PersoonId",
                unique: true,
                filter: "[PersoonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BihzProjects_EvenementId",
                table: "BihzProjects",
                column: "EvenementId",
                unique: true,
                filter: "[EvenementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BihzUsers_PersoonId",
                table: "BihzUsers",
                column: "PersoonId",
                unique: true,
                filter: "[PersoonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Donateur_ContactPersoonId",
                table: "Donateur",
                column: "ContactPersoonId");

            migrationBuilder.CreateIndex(
                name: "IX_Donateur_ProjectId",
                table: "Donateur",
                column: "ProjectId");

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
                name: "IX_EvenementPersoon_IsDeelnemerVanId",
                table: "EvenementPersoon",
                column: "IsDeelnemerVanId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_EmailTekstId",
                table: "Facturen",
                column: "EmailTekstId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_FactuurTekstId",
                table: "Facturen",
                column: "FactuurTekstId");

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
                name: "IX_PersoonRol_RollenId",
                table: "PersoonRol",
                column: "RollenId");

            migrationBuilder.CreateIndex(
                name: "IX_VerzondenMails_InhoudId",
                table: "VerzondenMails",
                column: "InhoudId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "EvenementPersoon");

            migrationBuilder.DropTable(
                name: "FietsTochten");

            migrationBuilder.DropTable(
                name: "GolfDagen");

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
                name: "VerzondenMails");

            migrationBuilder.DropTable(
                name: "Donateur");

            migrationBuilder.DropTable(
                name: "Rollen");

            migrationBuilder.DropTable(
                name: "Documenten");

            migrationBuilder.DropTable(
                name: "BihzProjects");

            migrationBuilder.DropTable(
                name: "Evenementen");
        }
    }
}