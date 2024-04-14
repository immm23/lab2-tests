using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace OOPLab1.ViewModels
{
    #region Attribues
    [ExcludeFromCodeCoverage]
    #endregion
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public ChangeRoleViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }

    }
}
