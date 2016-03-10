using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Mvc;

namespace TestIPrincipal.Controllers
{
    public class HomeController : ApiController
    {
        public ActionResult Index()
        {
            return null;
        }

        public bool AdminHasAccessToResource(string resouceId)
        {
            return User.IsInternalAdmin() || User.HasAccessToResource(resouceId);
        }
    }
    
    public static class IPrincipalUserExtensions
    {
        public static bool IsInternalAdmin(this IPrincipal user)
        {
            var claimsPrincipal = user.ToClaimsPrincipal();
            return claimsPrincipal.HasClaim(ClaimTypes.SuperAdmin, "1");
        }

        public static bool HasAccessToResource(this IPrincipal user, string resouceId)
        {
            var claimsPrincipal = user.ToClaimsPrincipal();
            return claimsPrincipal.HasClaim(ClaimTypes.Resource, resouceId);
        }

        public static ClaimsPrincipal ToClaimsPrincipal(this IPrincipal user)
        {
            var claimsPrincipal = user as ClaimsPrincipal;

            if (claimsPrincipal == null)
            {
                throw new Exception("No Claims Principal");
            }

            return claimsPrincipal;
        }
    }

    public static class ClaimTypes
    {
        public static string SuperAdmin = "SuperAdmin";
        public static string Resource = "Resource";
    }
}