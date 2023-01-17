using Database.Users;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CatalogManager
{
    public class PermissionsService
    {
        private readonly PermissionOptions _configuration;
        private readonly UserRepository _userRepository;

        public PermissionsService(IOptions<PermissionOptions> configuration, UserRepository userRepository)
        {
            _configuration = configuration.Value;
            _userRepository = userRepository;
        }

        public PermissionResponse HasPermission(ClaimsPrincipal user, Permissions permission)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier);

            if (!_configuration.RequirePermission)
                return PermissionResponse.HasPermission;

            if (userId == null)
                return PermissionResponse.LoggedOut;

            var hasPerm = _userRepository.UserHasPermission(userId.Value, permission);

            return hasPerm ? PermissionResponse.HasPermission : PermissionResponse.NoPermission;
        }

        public bool AllowAnonymous()
        {
            return _configuration.AllowAnonymous;
        }
    }

    public enum PermissionResponse
    {
        LoggedOut,
        HasPermission,
        NoPermission
    }

    public class PermissionOptions
    {
        /// <summary>
        /// Allow users to view without being logged in.
        /// </summary>
        public bool AllowAnonymous { get; set; }
        /// <summary>
        /// Require users to have a specific permission to view a page
        /// </summary>
        public bool RequirePermission { get; set; }
    }

}
