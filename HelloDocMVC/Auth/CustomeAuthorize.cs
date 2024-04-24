
using BLL_Business_Logic_Layer_.Interface;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HalloDoc.mvc.Auth
{


    [AttributeUsage(AttributeTargets.All)]
    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly string _role;
        private readonly string _menu;
        public CustomAuthorize(string role = "", string menu = null)
        {
            _role = role;
            this._menu = menu;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtServices = context.HttpContext.RequestServices.GetService<IJwtService>();
            var _admin = context.HttpContext.RequestServices.GetService<IAdminDash>();

            if (jwtServices == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "LoginPage" }));
                return;
            }


            var request = context.HttpContext.Request;
            var roleMain = context.HttpContext.Session.GetInt32("roleId");
            var token = request.Cookies["jwt"];

            if (token == null || !jwtServices.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "LoginPage" }));
                return;
            }
            var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "LoginPage" }));
                return;
            }

            if (string.IsNullOrWhiteSpace(_role) || roleClaim.Value != _role)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "LoginPage" }));

            }


            if (_menu != null)
            {
                List<string> roleMenu = _admin.GetListOfRoleMenu((int)roleMain);
                bool f = false;
                if (roleMenu.Any(r => r == _menu))
                {
                    f = true;
                }
                if (!f)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "AccessDenied" }));
                    return;
                }
            }
        }
    }
}
