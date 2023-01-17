using CatalogManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;

namespace OrionDashboard.Web.Attributes
{
    public class RequireLoginAttribute : Attribute, IAuthorizationFilter
    {
        private bool _forceLogin;

        public RequireLoginAttribute(bool forceLogin = false)
        {
            _forceLogin = forceLogin;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissionsService = context.HttpContext.RequestServices.GetService(typeof(PermissionsService)) as PermissionsService;
            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (!_forceLogin && permissionsService.AllowAnonymous())
                return;

            if (userId == null)
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
