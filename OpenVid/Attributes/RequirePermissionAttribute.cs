using CatalogManager;
using Database.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace OrionDashboard.Web.Attributes
{
    public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
    {
        private Permissions _permission;

        public RequirePermissionAttribute(Permissions permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissionsService = context.HttpContext.RequestServices.GetService(typeof(PermissionsService)) as PermissionsService;

            var response = permissionsService.HasPermission(context.HttpContext.User, _permission);

            if (response == PermissionResponse.LoggedOut)
                context.Result = new RedirectResult("/Login");
            else if (response == PermissionResponse.NoPermission)
                context.Result = new StatusCodeResult(401);

            return;

        }
    }

}
