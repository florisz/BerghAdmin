using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;

using Microsoft.AspNetCore.Identity;

using System.Security.Claims;

namespace BerghAdmin.Services;

public class SeedUsersService : ISeedUsersService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IRolService _rolService;

    public SeedUsersService(
        ApplicationDbContext dbContext,
        UserManager<User> userManager,
        IRolService rolService)
    {
        this._dbContext = dbContext;
        this._rolService = rolService;
        this._userManager = userManager;
    }

    public async Task SeedUsersData()
    {
        if (DatabaseHasUsers())
        {
            return;
            }
        var rollen = GetRollen();

        if (rollen != null)
        {
            var persoon = new Persoon
            {
                Voorletters = "F",
                Voornaam = "Floris",
                Achternaam = "Zwarteveen",
                Adres = "Berkenlaan 12",
                EmailAdres = "fzwarteveen@gmail.com",
                GeboorteDatum = new DateTime(2002, 1, 1),
                Geslacht = GeslachtEnum.Man,
                Land = "Nederland",
                Mobiel = "06-12345678",
                Plaats = "Beek",
                Postcode = "7037 CA",
                Telefoon = "onbekend",
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.CommissieLid] }
            };
            await InsertUser(persoon, rollen, "admin", AdministratorPolicyHandler.Claim);

            persoon = new Persoon
            {
                Voorletters = "L P",
                Voornaam = "Lars-Peter",
                Achternaam = "Reumer",
                Adres = "Laan 12",
                EmailAdres = "lpreumer@mail.com",
                GeboorteDatum = new DateTime(2002, 1, 1),
                Geslacht = GeslachtEnum.Man,
                Land = "Nederland",
                Mobiel = "06-12345678",
                Plaats = "Arnhem",
                Postcode = "6100 DT",
                Telefoon = "onbekend",
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.CommissieLid] }
            };
            await InsertUser(persoon, rollen, "secretaris", AdministratorPolicyHandler.Claim);

            persoon = new Persoon
            {
                Voorletters = "G",
                Voornaam = "Gerard",
                Achternaam = "Hendriksen",
                Adres = "Straat 32",
                EmailAdres = "ghendriksen@bedrijf.com",
                GeboorteDatum = new DateTime(2002, 1, 1),
                Geslacht = GeslachtEnum.Man,
                Land = "Nederland",
                Mobiel = "06-12345678",
                Plaats = "Zevenaar",
                Postcode = "6900 AB",
                Telefoon = "onbekend",
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Golfer], rollen[RolTypeEnum.CommissieLid] }
            };
            await InsertUser(persoon, rollen, "golfbeheer", BeheerGolfersPolicyHandler.Claim);

            persoon = new Persoon
            {
                Voorletters = "W",
                Voornaam = "Wilbert",
                Achternaam = "Esselink",
                Adres = "Straat 102",
                EmailAdres = "wilbert@mail.com",
                GeboorteDatum = new DateTime(2002, 1, 1),
                Geslacht = GeslachtEnum.Man,
                Land = "Nederland",
                Mobiel = "06-12345678",
                Plaats = "Bergh",
                Postcode = "7000 XS",
                Telefoon = "onbekend",
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Ambassadeur], rollen[RolTypeEnum.CommissieLid] }
            };
            await InsertUser(persoon, rollen, "ambassadeurbeheer", BeheerAmbassadeursPolicyHandler.Claim);
        }
    }

    private Dictionary<RolTypeEnum, Rol> GetRollen()
    {
        var allRollen = new[] { RolTypeEnum.Ambassadeur,
                                    RolTypeEnum.Begeleider, 
                                    RolTypeEnum.CommissieLid,
                                    RolTypeEnum.Golfer, 
                                    RolTypeEnum.MailingAbonnee,
                                    RolTypeEnum.Fietser, 
                                    RolTypeEnum.VriendVan,
                                    RolTypeEnum.Vrijwilliger
                                };

        var rollen = new Dictionary<RolTypeEnum, Rol>();

        foreach (var rolEnum in allRollen)
        {
            var rol = _rolService.GetRolById(rolEnum);
            rollen.Add(rolEnum, rol);
        }
        
        return rollen;
    }

    private bool DatabaseHasUsers()
        => this._userManager.Users.Count() > 0;


    private async Task InsertUser(Persoon persoon, Dictionary<RolTypeEnum, Rol> rollen, string naam, Claim claim)
    {
        await _dbContext.AddAsync(persoon);
        await _dbContext.SaveChangesAsync();

        var user = new User
        {
            CurrentPersoonId = persoon.Id,
            Name = naam,
            //Roles = new string[] { "admin" },
            UserName = $"{naam}@bihz.nl",
            Email = $"{naam}@berghinhetzadel.nl",
            AccessFailedCount = 0,
            EmailConfirmed = true,
            LockoutEnabled = false,
            LockoutEnd = null,
            PhoneNumber = "",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false
        };

        var result = await this._userManager.CreateAsync(user, "qwerty@123");
        if (result.Succeeded)
        {
            await this._userManager.AddClaimAsync(user, claim);
        }
    }

}
