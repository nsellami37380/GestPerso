using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WAGestPerso.Models;

namespace WAGestPerso.Controllers
{
   [Authorize]
   public class HomeController : Controller
   {
      BD_GESTPERSOEntities db = new BD_GESTPERSOEntities();
      public ActionResult Index()
      {
        
         ViewBag.listeUtilisateurs = db.Utilisateurs.ToList().OrderBy(r => r.nom);
         ViewBag.nbNonAffecte = db.Taches.ToList().Where(r => r.utilisateur == null).Count();
         ViewBag.listeNonAffecte = db.Taches.ToList().Where(r => r.utilisateur == null);


         return View();
      }

      public ActionResult About()
      {
         ViewBag.Message = "Your application description page.";

         return View();
      }

      public ActionResult Contact()
      {
         ViewBag.Message = "Your contact page.";

         return View();
      }
      public ActionResult sapproprierrTache(int id)
      {
         Tach tache = db.Taches.Find(id);
         tache.utilisateur = 4;
         db.Entry(tache).State = EntityState.Modified;
         db.SaveChanges();
         return RedirectToAction("index");
      }

   }
}