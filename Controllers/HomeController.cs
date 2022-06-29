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

         // Liste des taches de l'utilisateur en cours triés par priorité
         Utilisateur utilisateur = db.Utilisateurs.FirstOrDefault(u => u.nom == User.Identity.Name);
         ViewBag.listeTaches = db.Taches.ToList().Where(t => t.utilisateur == utilisateur.id).OrderByDescending(t => t.importance);
         return View();
      }

    
      public ActionResult sapproprierrTache(int id)
      {
         // Utiliser dans la vue home : une utilisateur s'affecte une tache non affectée
         Tach tache = db.Taches.Find(id);
         Utilisateur utilisateur = db.Utilisateurs.FirstOrDefault(u => u.nom == User.Identity.Name);
         tache.utilisateur = utilisateur.id;
         db.Entry(tache).State = EntityState.Modified;
         db.SaveChanges();
         return RedirectToAction("index");
      }
      public ActionResult TerminerTache(int id)
      {
         Tach tache = db.Taches.Find(id);
         tache.Termine = true;
   
         db.SaveChanges();
         return RedirectToAction("index");

      }
      

   }
}