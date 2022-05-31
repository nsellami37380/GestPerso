using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WAGestPerso.Controllers
{
    public class UtilisateursController : Controller
    {
        // GET: Utilisateurs
        public ActionResult AjoutUtilisateur()
        {
            return View();
        }
    }
}