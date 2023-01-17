using System.Linq;

namespace Database.Users
{
    public class UserRepository
    {
        private readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool UserHasPermission(string userId, Permissions permission)
        {
            return _dbContext.UserPermission.Any(x => x.UserId == userId && x.PermissionId == (int)permission);
        }
    }

    public enum Permissions
    {
        Account_Management = 1,
        Catalog_Import = 2,
        Catalog_Delete = 3,
        Tag_Management = 4,
        Catalog_Update = 5
    }
}
