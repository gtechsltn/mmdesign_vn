using Mmdesign.Helpers;
using Mmdesign.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Mmdesign.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: /Account/LogIn
        [HttpGet]
        public ActionResult LogIn(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        // POST: /Account/LogIn => Admin/Index
        [HttpPost]
        public ActionResult LogIn(LogInModel LogInView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                string userName = LogInView.UserName;
                string password = LogInView.Password;

                if (Membership.ValidateUser(userName, password))
                {
                    var user = Membership.GetUser(LogInView.UserName, false) as CustomMembershipUser;
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = user.Roles.Select(r => r.RoleName).ToList()
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                                                                                            (
                                                                                                1, LogInView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                                                                                            );

                        //Encrypt Forms Authentication Ticket
                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, enTicket);
                        Response.Cookies.Add(faCookie);
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return Redirect("~/Project/Manage");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên người dùng hoặc Mật khẩu không hợp lệ.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tên người dùng hoặc Mật khẩu không hợp lệ.");
            }
            return View(LogInView);
        }

        [Authorize]
        [HttpGet]
        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();

            //return Redirect("~/Account/LogIn");

            return RedirectToAction("LogIn", "Account", null);
        }
    }
}