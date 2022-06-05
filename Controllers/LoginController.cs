using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WAGestPerso.Models;

namespace WAGestPerso.Controllers
{
   public class LoginController : Controller
   {
      // GET: Login
      public ActionResult Login()
      {
         LoginModel lm = new LoginModel();
         return View(lm);
      }

      [HttpPost]
      public ActionResult login(LoginModel loginModel, string returnUrl)
      {
           if (!string.IsNullOrEmpty(loginModel.UserName)
             && !string.IsNullOrEmpty(loginModel.Password))
         //if ((loginModel.UserName == "admin") && (loginModel.Password == "admin"))
         {
            BD_GESTPERSOEntities db = new BD_GESTPERSOEntities();
            Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.nom == loginModel.UserName);
            if (user == null || (FormsAuthentication.HashPasswordForStoringInConfigFile(loginModel.Password, "SHA1") != user.mdp))
            {
               return RedirectToAction("Login");
            }

            FormsAuthentication.SetAuthCookie(loginModel.UserName, false);
            /* var ticket = new FormsAuthenticationTicket(1, loginModel.UserName, DateTime.Now,
                DateTime.Now.AddMinutes(60), false, "administrateur|gerant");
             var encryptedTicket = FormsAuthentication.Encrypt(ticket);
             var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

             Response.Cookies.Add(cookie);*/

         //   Roles.AddUserToRole(loginModel.UserName, "Administrators");
            if (string.IsNullOrEmpty(returnUrl))
               return RedirectToAction("Index", "Home");
            else
               return Redirect(returnUrl);
         }
         else
         {
            return RedirectToAction("Login");
         }
      }
      // ---------------------------------------------------------------------------------------
      [HttpGet]
      public ActionResult LogOut()
      {
         FormsAuthentication.SignOut();
         return RedirectToAction("Login");
         //return View("Login");
      }

      // ---------------------------------------------------------------------------------------

   }
}