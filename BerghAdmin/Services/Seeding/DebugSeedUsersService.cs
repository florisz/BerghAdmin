using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;

using Microsoft.AspNetCore.Identity;

using System.Security.Claims;

namespace BerghAdmin.Services.Seeding;

public class DebugSeedUsersService : ISeedUsersService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IRolService _rolService;

    public DebugSeedUsersService(
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
                Voorletters = "R",
                Voornaam = "Richard",
                Achternaam = "de Zwart",
                Adres = "Straat 32",
                EmailAdres = "richard@dezwartenco.nl",
                GeboorteDatum = new DateTime(2002, 1, 1),
                Geslacht = GeslachtEnum.Man,
                Land = "Nederland",
                Mobiel = "06-12345678",
                Plaats = "Breda",
                Postcode = "6900 AB",
                Telefoon = "onbekend",
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Golfer], rollen[RolTypeEnum.CommissieLid] }
            };
            await InsertUser(persoon, rollen, "ict", AdministratorPolicyHandler.Claim);

            persoon = new Persoon
            {
                Voorletters = "R",
                Voornaam = "Reinald",
                Achternaam = "Baart",
                Adres = "Straat 32",
                EmailAdres = "reinald.baart@gmail.com",
                GeboorteDatum = new DateTime(2002, 1, 1),
                Geslacht = GeslachtEnum.Man,
                Land = "Nederland",
                Mobiel = "06-12345678",
                Plaats = "Breda",
                Postcode = "6900 AB",
                Telefoon = "onbekend",
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Golfer], rollen[RolTypeEnum.CommissieLid] }
            };
            await InsertUser(persoon, rollen, "webmaster", AdministratorPolicyHandler.Claim);
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
            UserName = $"{naam}@berghinhetzadel.nl",
            Email = $"{naam}@berghinhetzadel.nl",
            AccessFailedCount = 0,
            EmailConfirmed = true,
            LockoutEnabled = false,
            LockoutEnd = null,
            PhoneNumber = "",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
        };

        var result = await this._userManager.CreateAsync(user, "qwerty@123");
        if (result.Succeeded)
        {
            await this._userManager.AddClaimAsync(user, claim);
        }
    }

}
