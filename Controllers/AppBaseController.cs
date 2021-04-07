using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class AppBaseController : Controller
    {

        // GET: AppBase
        protected static user_Model userData=null;
        public AppBaseController() 
        {
            userData =getClaims();
            string role = string.Empty;
            if (userData != null)
            {

                switch (userData.userType)
                {
                    case 1:
                        role = "user";
                        break;
                    case 2:
                        role = "manager";
                        break;
                    case 3:
                        role = "admin";
                        break;
                }
                ViewBag.role = role;
                ViewBag.username = userData.username;
            }
        }


        public static user_Model getClaims() 
        {
            var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            if (claims.Count>0 )
            {
                userData = new user_Model();

                userData.userId = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value);
                userData.userType = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", StringComparison.OrdinalIgnoreCase))?.Value);
                userData.username = claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", StringComparison.OrdinalIgnoreCase))?.Value;
            }
            return userData;
        }
    }
}
