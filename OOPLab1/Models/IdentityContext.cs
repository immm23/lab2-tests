using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace OOPLab1.Models
{
    #region Attribues
    [ExcludeFromCodeCoverage]
    #endregion
    public class IdentityContext : IdentityDbContext<IdentityUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
