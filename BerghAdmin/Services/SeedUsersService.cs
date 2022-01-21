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
            await InsertUser(rollen, "admin", AdministratorPolicyHandler.Claim);
            await InsertUser(rollen, "aap", BeheerFietsersPolicyHandler.Claim);
            await InsertUser(rollen, "noot", BeheerGolfersPolicyHandler.Claim);
            await InsertUser(rollen, "mies", BeheerAmbassadeursPolicyHandler.Claim);
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


    private async Task InsertUser(Dictionary<RolTypeEnum, Rol> rollen, string naam, Claim claim)
    {
        var persoon = new Persoon
        {
            Voorletters = "F.",
            Voornaam = "Floris",
            Achternaam = naam,
            Adres = "Berkenlaan 12",
            EmailAdres = "fzwarteveen@mail.com",
            GeboorteDatum = new DateTime(2002, 1, 1),
            Geslacht = GeslachtEnum.Man,
            Land = "Nederland",
            Mobiel = "06-12345678",
            Plaats = "Beek",
            Postcode = "7037 CA",
            Telefoon = "onbekend",
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.Vrijwilliger] }
        };

        await _dbContext.AddAsync(persoon);
        await _dbContext.SaveChangesAsync();

        var user = new User
        {
            CurrentPersoonId = persoon.Id,
            Name = naam,
            //Roles = new string[] { "admin" },
            UserName = $"{naam}@bihz.nl",
            Email = "fzwarteveen@gmail.com",
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
