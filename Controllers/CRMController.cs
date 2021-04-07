using CRM.Models;
using CRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

/*
        Author:Sagar Srivastava,
        Aim:Contains code for CRM CRUD operation,
        Change Log: code ,developer name ,task description ,date
       =========================================================================================
        #CC01 ,Chandan Singh ,Added message for successfully adding an user ,30-Jan-2021
        #CC02 ,Chandan Singh ,update user list ,05-feb-2021
        #CC03 ,Sagar Srivastava ,Authentication cookies check for security ,11-feb-2021

 */

namespace CRM.Controllers
{
    [RoutePrefix("CRM")]
    [Authorize]
    public class CRMController : AppBaseController
    {
        // GET: CRM
        private AdminService service = new AdminService();
        [Authorize]
        public ActionResult Index(string status="")
        {
          
                if (!string.IsNullOrEmpty(status))
                {
                    ViewBag.Message = "User Added Successfully";
                }
                else
                {
                    ViewBag.Message = "";
                }
                /*#CC01  added end*/
                if (userData != null)
                {

                    if (userData.userType == 1)
                    {
                        return RedirectToAction("Details");
                    }

                    return View("view_AddUser", service.GetRoles(userData));
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
                      
        }

        // GET: CRM/Details/5
        //public ActionResult Details(string viewbag="")
        //{
        //    RecordsView_List records=service.GetRecords(userData);
        //    return View("ViewRecords" ,records);
        //}
        public ActionResult Details(string response = "")
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
                RecordsView_List records = service.GetRecords(userData);
                ViewBag.response = response;
                return View("ViewRecords", records);
            }
        }

        // GET: CRM/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: CRM/Create
        [HttpPost]
        public ActionResult Create(FormCollection model)
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
                    // TODO: Add insert logic here
                    string response = service.AddRecord(model, userData);
                    return RedirectToAction("Details", "CRM", new { response = response });
                }
                catch
                {
                    return View();
                }
            }
        
        }
        
        public ActionResult ViewUserList(string status="")
        {
            /* #CC03 start*/
            HttpCookie authCookie = Request.Cookies["__RequestVerificationToken"];
            HttpCookie envCookie = Request.Cookies[".AspNet.ApplicationCookie"];
            if (authCookie == null || envCookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            /* #CC03 else*/
            else
            {
                return View("View_ViewUserList", service.GetUserList(userData, 1));
            }
        }

        public ActionResult FilterUserList(string filter ,string datefilter)
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
                int mode = 1;
                if (!string.IsNullOrEmpty(datefilter))
                {
                    mode = 2;
                }
                return View("View_ViewUserList", service.GetUserList(userData, mode, filter, datefilter));
            }
        }
        
        public ActionResult ExportUserList(string filter, string datefilter)
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
                int mode = 1;
                if (!string.IsNullOrEmpty(datefilter))
                {
                    mode = 2;
                }

                return View("View_ViewUserList", service.GetUserList(userData, mode, filter, datefilter, 1, 1));
            }
        }

        public ActionResult FilterRecords(string filter ,string datefilter ,int mode)
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
                RecordsView_List recordObj = service.GetRecords(userData, filter, datefilter, mode);
                if (recordObj.list.Count == 0)
                    ViewBag.recordStatus = "No Record Found.";
                return View("ViewRecords", recordObj);
            }
        }

        public ActionResult ExportRecords(string filter, string datefilter, int mode)
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
                int operationId = 1;
                return View("ViewRecords", service.GetRecords(userData, filter, datefilter, mode, operationId));
            }
           
        }
        
        public ActionResult AddUser()
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
                return View("view_AddUser", service.GetRoles());
            }
        }

        [HttpPost]
        public ActionResult AddUser(FormCollection model)
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

                    // TODO: Add insert logic here@ViewBag.SuccessMessage
                    model.Add("ResponseCode" , Guid.NewGuid().ToString());
                    string response = service.AddUser(model);
                    if(response== "success")
                    {
                        Email(model ,model["ResponseCode"]);
                    }
                    //ViewBag.Message = "User Added Sussesfully";
                    return RedirectToAction("Index", new { status = response });
                }
                catch
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(DeleteModel record)
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
                    // TODO: Add delete logic here
                    ViewBag.message = service.DeleteUserService(record);
                    return PartialView("partial_records", service.GetUserList(userData, 1));
                }
                catch
                {
                    return View();
                }
            }
     
        }

        [HttpPost]
        public ActionResult ActivateUser(DeleteModel record)
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
                    // TODO: Add delete logic here
                    ViewBag.message = service.ActivateUserService(record);
                    return PartialView("partial_records", service.GetUserList(userData, 1));
                }
                catch
                {
                    return View();
                }
            }

        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
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
                string status = service.UpdateRecord(collection, userData);
                return RedirectToAction("Details", "CRM", new { response = status });
            }
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            /* #CC03 start*/
            HttpCookie authCookie = Request.Cookies["__RequestVerificationToken"];
            HttpCookie envCookie = Request.Cookies[".AspNet.ApplicationCookie"];
            if (authCookie == null || envCookie == null)
            {
                return (Json("Login to continue", JsonRequestBehavior.AllowGet));
            }
            /* #CC03 end*/
            else
            {
                // TODO: Add update logic here
                RecordsView_Model result = new RecordsView_Model();
                result = service.GetRecordsById(id);
                ViewBag.Template = "Edit";
                return (Json(result, JsonRequestBehavior.AllowGet));
            }
        }

        // GET: CRM/Delete/5
        public ActionResult Delete()
        {
            return View();
        }
        /*#CC02  added start*/
        public ActionResult EditUser( int id)
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
                user_Model usermodel = new user_Model();
                usermodel.userId = id;
                usermodel.userType = userData.userType;
                UserEditModel model = service.GetUsers(usermodel, 3);
                return View("View_EditRecord", model);
            }
        }

        [HttpPost]
        public ActionResult EditUser(FormCollection collection)
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
                //user_Model usermodel = new user_Model();
                //usermodel.userId = id;
                //usermodel.userType = userData.userType;
                userUpdate_Response model = service.UpdateUserDetails(collection);
                return RedirectToAction("ViewUserList");
            }
   
        }
        /*#CC02  added end*/

        // POST: CRM/Delete/5
        [HttpPost]
        public ActionResult Delete(DeleteModel record)
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
                    // TODO: Add delete logic here
                    ViewBag.message = service.DeleteRecord(record);
                    return RedirectToAction("ViewRecords");
                }
                catch
                {
                    return View();
                }
            }
      
        }

        [Route("ActivationLink/{responseCode}")]
        public ActionResult ActivateLink(string responseCode)
        {
            string status =service.UpdateUserStatus( responseCode);
            return RedirectToAction("Logoff" ,"Account");
        }


        public string Email(FormCollection model ,string responseCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("naromesh962@gmail.com", model["username"].ToString());
                    var receiverEmail = new MailAddress(model["email"].ToString(), "Receiver");
                    var password = /*"Massman123@#@"*/"Kapil@4484";
                    var subject = "Email Verification.";
                    var body = @"<a href='http://192.168.1.100:81/CRM/ActivateLink?responseCode=" + responseCode+ "'>Activation Link</a>";
//                    var body = @"<!DOCTYPE html>"+
//                                "<html lang=\"en\">"+
//                                "<head>"+
//                                "<meta charset=\"UTF-8\">"+
//                                "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">"+
//                                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">"+
//                                "<title>CRM</title>"+
//                                "<link rel=\"stylesheet\" href=\"css/style.css\">"+
//                                "<link rel=\"stylesheet\" href=\"css/fontawesome-free-5.15.1-web/css/all.css\">"+
//                                " <style>"+
        
//                                ".verification_wrapper {"+
//                                 "width: 100%;"+
//                                "height: 100vh;"+
//                                "display: -webkit-box;"+
//                                "display: -ms-flexbox;"+
//                                "display: flex;"+
//                                "-webkit-box-pack: center;"+
//                                "-ms-flex-pack: center;"+
//                                "justify-content: center;"+
//                                "-webkit-box-align: center;"+
//                                "-ms-flex-align: center;"+
//                                "align-items: center;"+
//                                "-ms-flex-wrap: wrap;"+
//                                "flex-wrap: wrap;}"+

//                                ".verification_wrapper .Verification-content {"+
//                                "width: 50%;"+
//                                "margin: 0 auto;"+
//                                "overflow: hidden; }"+

//        ".verification_wrapper .Verification-content .logo {"+
//           " width: 100%;"+
//            "padding: 1rem 0;}"+

//            ".verification_wrapper .Verification-content .logo figure a {"+
//               " text-decoration: none;}"+

//                ".verification_wrapper .Verification-content .logo figure a img {"+
//                   " max-width: 24%;}"+

//        ".verification_wrapper .Verification-content .evelop-banner .mail-wrp {"+
//            "width: 100%;"+
//            "height: 200px; "+
//            "overflow: hidden;"+
//            "background: #36b7e7;" +
//            "display: -webkit-box;" +
//            "display: -ms-flexbox; " +
//            "display: flex;" +
//            "-webkit-box-pack: center; " +
//            "- ms-flex-pack: center;" +
//            "justify -content: center;" +
//            "-webkit-box-align: center;" +
//            "-ms-flex-align: center;" +
//            "align -items: center;" +
//            " -ms-flex-wrap: wrap;" +
//            "flex -wrap: wrap;" +
//            "border -top-right-radius: 5px;" +
//            "border -top-left-radius: 5px;}" +

//            ".verification_wrapper .Verification-content .evelop-banner .mail-wrp .fa-envelope {" +
//                "font -size: 10em;" +
//                "-webkit-transform: rotate(45deg);" +
//                "transform: rotate(45deg);" +
//                "color: #000; }" +

//            ".verification_wrapper .Verification-content .evelop-banner .mail-wrp img {" +
//                "max -width: 35%;" +
//                "text -align: center;" +
//                "-webkit-animation: imganime 3s linear infinite;" +
//               " animation: imganime 3s linear infinite;}" +

//        ".verification_wrapper .Verification-content .evelop-banner .evn-cnt {" +
//           " text -align: center;" +
//           " width: 100%;" +
//            "background: #eee;" +
//            "padding: 1rem 1rem 2rem;" +
//           " border -bottom-right-radius: 5px;" +
//            "border -bottom-left-radius: 5px;}" +

//           " .verification_wrapper .Verification-content .evelop-banner .evn-cnt h2 {" +
//                "font -size: 2em;" +
//                "font -weight: bold;" +
//                "text -transform: capitalize;" +
//                "color: #262f3a;" +
//                "letter -spacing: 1px;" +
//               " word -spacing: 1px;}" +

//            ".verification_wrapper .Verification-content .evelop-banner .evn-cnt p {" +
//                "font -size: 1em;" +
//                "text -transform: capitalize;" +
//                "letter -spacing: 1px;" +
//               " word -spacing: 1px;" +
//                "padding: 1rem 2rem;" +
//                "line -height: 1.7;" +
//                "color: #5d5d5d;}" +

//           " .verification_wrapper .Verification-content .evelop-banner .evn-cnt a {" +
//                "display: inline-block;" +
//                "margin: 1rem 0 0;" +
//                "padding: 1rem 2.5rem;" +
//                "border -radius: 5px;" +
//                "text -transform: uppercase;"+
//                "font-size: 1em;" +
//                "background: #262f3a;" +
//                "color: #fff;" +
//                "-webkit-transition: all .3s ease-in-out;" +
//                "transition: all .3s ease-in-out;" +
//               " outline: none;" +
//                "-webkit-box-shadow: none;" +
//                "box -shadow: none;" +
//                "text -decoration: none;" +
//                "letter -spacing: 1px;" +
//                "word -spacing: 3px; }" +

//                ".verification_wrapper .Verification-content .evelop-banner .evn-cnt a:hover {" +
//                    "-webkit-transition: all .3s ease-in-out;" +
//                    "transition: all .3s ease-in-out;" +
//                    "background: #000;" +
//                    "-webkit-transform: scale(0.99);" +
//                    "transform: scale(0.99); }" +

//"@-webkit-keyframes imganime {" +
//  "  0 %, 100% {" +
//      "  -webkit-transform: scale(0.88);" +
//       " transform: scale(0.88);" +
//        "-webkit-transition: all .3s ease-in-out;" +
//        "transition: all .3s ease-in-out;" +
//        "-webkit-filter: drop-shadow(1px 3px 4px #000);" +
//        "filter: drop-shadow(1px 3px 4px #000);}" +

//                "50% {" +
//                    "-webkit-transition: all .3s ease-in-out;" +
//                     "transition: all .3s ease-in-out;" +
//                     "-webkit-transform: scale(1.1);" +
//                    " transform: scale(1.1);" +
//                    " -webkit-filter: opacity(0.5);" +
//                    " filter: opacity(0.5);}}" +

//                 "@keyframes imganime {" +
//                 " 0 %, 100% {" +
//       " -webkit-transform: scale(0.88);" +
//        "transform: scale(0.88);" +
//        "-webkit-transition: all .3s ease-in-out;" +
//        "transition: all .3s ease-in-out;" +
//        "-webkit-filter: drop-shadow(1px 3px 4px #000);" +
//        "filter: drop-shadow(1px 3px 4px #000);}" +

//    "50 % {" +
//        "-webkit-transition: all .3s ease-in-out;" +
//        "transition: all .3s ease-in-out;" +
//        "-webkit-transform: scale(1.1);" +
//        "transform: scale(1.1);" +
//        "-webkit-filter: opacity(0.5);" +
//        "filter: opacity(0.5);}}" +
//    "</style>" +
//"</head>" +
//"<body>" +
//    "< main class=\"verification_wrapper\">" +
//       " < div class=\"Verification-content\">" +
//           " < div class=\"logo\">" +
//                "< figure>" +
//                    "< a href=\"#\"><img src=\"image/logo1.png\" alt=\"\"></a>"+
//                "</figure>" +
//            "</div>" +
//            "< div class=\"evelop-banner\">" +
//                "< div class=\"mail-wrp\">" +
//                     "< img src=\"image/stars.png\" alt=\"img\">" +
//                "</div>" +
//                "< div class=\"evn-cnt\">" +
//                    "< h2>Email Confirmation</h2>" +
//                    "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Quibusdam nemo rem quis ea illo laudantium eum, dolore explicabo suscipit! Expedita?</p>" +
//                     "< a href='#'>Verify email <address></address></a>"+
//                "</div>" +
//            "</div>" +
//        "</div>" +
//    "</main>" +
//"</body>" +
//"</html>";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)

                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml=true
                    })
                    {
                        smtp.Send(mess);
                    }
                    return "success";
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return "failed";
        }
    }
}
