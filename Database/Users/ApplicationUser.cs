using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Database.Users
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<UserPermission> UserPermission { get; set; }
    }
}
