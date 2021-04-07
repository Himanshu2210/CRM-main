using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using CRM.App_Start;
using CRM.Models;
using CRM.Services;
using System.Security.Cryptography;
using System.Text;
using CRM.utils;
using System.Net.Mail;
using System.Net;
/*
Author:Sagar Srivastava,
Aim:Contains code for account management,
Change Log: code ,developer name ,task description ,date
=========================================================================================
#CC01 ,Chandan Singh ,after sission out redirect to login page ,04-feb-2021
#CC02 ,Sagar Srivastava ,Authentication cookies check for security ,11-feb-2021
*/

namespace CRM.Controllers
{
    public class AccountController : Controller
    {
        
        CRM.App_Start.DatabaseContext db = new DatabaseContext();
        // GET: Account
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            try
            {
              
                if (this.Request.IsAuthenticated)

                {
                    /*#CC02 start*/
                    HttpCookie authCookie = Request.Cookies["__RequestVerificationToken"];
                    HttpCookie envCookie = Request.Cookies[".AspNet.ApplicationCookie"];
                    if(authCookie == null || envCookie==null)
                    {
                        return this.View("View_Login");
                    }
                    /*#CC02 end*/
                    else
                    {
                        return this.RedirectToLocal(returnUrl);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return this.View("View_Login");
        }

        public ActionResult RedirectToLogin()
        {
            return View("View_Login");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            /* #CC03 start*/
            HttpCookie authCookie = Request.Cookies["__RequestVerificationToken"];
            HttpCookie envCookie = Request.Cookies[".AspNet.ApplicationCookie"];
            if (authCookie == null || envCookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            /* #CC03 end*/
            else
            {
                try
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return this.Redirect(returnUrl);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                return this.RedirectToAction("Logout", "Account");
            }
           
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManger = ctx.Authentication;
            authenticationManger.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            NoCacheAttribute obj = new NoCacheAttribute();
            //obj.OnResultExecuting();
            ManageCache();
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Duration =20)]
        public ActionResult Login(Login_Model model, string returnUrl)
        {
            //string returnUrl = "Account";
                string controller = "Login";
            ////string returnUrl = model.returnUrl;
            //if (!string.IsNullOrEmpty(model.returnUrl) && !string.IsNullOrEmpty(model.controller))
            //{
            //    returnUrl = model.returnUrl;
            //    controller = model.controller;
            //}
            try
            {
                if (ModelState.IsValid)
                {

                    //var logininfo = db.User_Log.Where(x => (x.email.ToLower() == model.UserName.ToLower() || x.phone_number == model.UserName) && x.password == model.Password).ToList();
                    AdminService userObj = new AdminService();
                    
                    user_Model logininfo = userObj.ValidateUsers(model);
                    if (logininfo != null)
                    {
                        
                        var logindetails = logininfo;
                        this.SigninUser(logininfo, false);
                        return this.RedirectToLocal(returnUrl);

                        //return this.RedirectToAction(returnUrl ,controller);
                    }
                    ///*#CC01  added start*/
                    //else if ()
                    //{
                    //    ModelState.AddModelError(string.Empty, "Your account suspend please contact administrator");
                    //}
                    ///*#CC01  added end*/
                    else
                    {
                        
                            //this.SendSuspendEmail(logininfo);
                            //ModelState.AddModelError(string.Empty, "Your account suspend please contact administrator");
                            ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return this.View("View_Login" );
        }
        /*#CC01  added start*/
        //public string encrypt(string password)
        //{
        //    string msg = "";
        //    byte[] encode = new byte[password.Length];
        //    encode = Encoding.UTF8.GetBytes(password);
        //    msg = Convert.ToBase64String(encode);
        //    return msg;
        //}
        /*#CC01  added start*/

        private void SigninUser(user_Model userdata, bool isPersistent)
        {
            var Claims = new List<Claim>();
            try
            {
                
                Claims.Add(new Claim(ClaimTypes.Name, userdata.username));
                Claims.Add(new Claim(ClaimTypes.Role, userdata.userType.ToString()));
                Claims.Add(new Claim(ClaimTypes.NameIdentifier, userdata.userId.ToString()));

                var claimIdenties = new ClaimsIdentity(Claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SendSuspendEmail( user_Model data)
        {

            using (MailMessage mm = new MailMessage("chandancist.022@gmail.com" , data.username))
            
            {
                
                 mm.Subject = "Your Account is Suspended";
                string body = "Hello " + data.username + ",";
                body += "<br /><br />Please Contact administrator";
                body += "<br /><br />Thanks";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("chandanmassmancybergreeks.com", "Massman123@#@");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                
            }
        }

        public void ManageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
}
