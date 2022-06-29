using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WAGestPerso.Models;

namespace WAGestPerso.Controllers
{
   [Authorize]
   public class TachesController : Controller
    {
      BD_GESTPERSOEntities db = new BD_GESTPERSOEntities();
      // GET: Taches
      public ActionResult Index()
       {
           return View();
       }

      // ---------------------------------------------------------------------------------------
      public ActionResult AjoutTache(string tri = "nom")
      {
         // Action qui affiche les taches
         try
         {
            // Par defaut on tri par le nom des utilisateur, joijnture à gauche il y a des taches sans utilisateur
            var ListeTaches =
            from taches in db.Taches
            join utilisateurs in db.Utilisateurs on taches.utilisateur equals utilisateurs.id into gj
            from x in gj.DefaultIfEmpty()

            orderby x.nom
            select taches;
            
             if ((tri == "importance"))
             {
               // todo: requete inutile ??  ListeTaches = db.Taches.ToList().OrderByDescending(t => t.importance);
               ListeTaches =
                from taches in db.Taches
                orderby taches.importance descending
                select taches;
             }
             else if ((tri == "date"))
             {
                ListeTaches =
               from taches in db.Taches
               orderby taches.date_saisie
               select taches;
             } 

            //   ViewBag.ListeTaches = db.Taches.ToList().OrderBy(t => t.utilisateur.);
            ViewBag.ListeTaches = ListeTaches;
            ViewBag.ListeUtilisateurs = db.Utilisateurs.ToList();

            return View();
         }
         catch (Exception)
         {
            return HttpNotFound();
         }
      }

      [HttpPost]
      public ActionResult AjoutTache(Tach tache)
      {
         try
         {
            if(ModelState.IsValid)
            {
               tache.date_saisie = DateTime.Now;
               // si des fichiers sont présents on les ajoutent
               tache.fichiers = "";
               for (int i = 0; i < Request.Files.Count; i++)
               {
                  HttpPostedFileBase currentFile = Request.Files[i];
                  if (currentFile != null && currentFile.ContentLength > 0)
                  {
                     var fileName = Path.GetFileName(currentFile.FileName);
                     var path = Path.Combine(Server.MapPath("~/Fichiers"), fileName);
                     if (i > 0) tache.fichiers += ";";
                     tache.fichiers += fileName;

                     currentFile.SaveAs(path);
                  }                    
               }
               //Gestion des priorités
               if (tache.date_urgente <= DateTime.Now)
               {
                  tache.importance = 3;
               }
               else if (tache.date_prioritaire <= DateTime.Now)
               {
                  tache.importance = 2;
               }
               else if (tache.date_prioritaire <= DateTime.Now)
               {
                  tache.importance = 1;
               }
               else
               {
                  tache.importance = 0;
               }
               db.Taches.Add(tache);
               db.SaveChanges();

            }
            return RedirectToAction("AjoutTache");
         }
         catch (Exception E)
         {

            return HttpNotFound();
         }
      }

      [HttpGet]
      public ActionResult affichageTrie()
      {
         // Par defaut on trie par nom
         if ((Request["tri"] == null) || (Request["tri"] == "nom"))
         {
            return RedirectToAction("AjoutTache");
         }
         else if (Request["tri"] == "importance")
         {
            return RedirectToAction("AjoutTache", new { tri = "importance" } );
         }
         else if (Request["tri"] == "date")
         {
            return RedirectToAction("AjoutTache", new { tri = "date" });
         }
         else
            return RedirectToAction("AjoutTache");

      }
      public ActionResult ModifierTache(int id)
      {
         try
         {
            ViewBag.ListeTaches = db.Taches.ToList();
            ViewBag.listeUtilisateurs = db.Utilisateurs.ToList();
            Tach tache = db.Taches.Find(id);
            if (tache != null)
            {
               return View("AjoutTache", tache);
            }
            return RedirectToAction("AjoutTache");
         }
         catch (Exception)
         {

            return HttpNotFound();
         }
      }

      [HttpPost]
      public ActionResult ModifierTache(Tach tache)
      {

         try
         {
            if(ModelState.IsValid)
            {
               db.Entry(tache).State = EntityState.Modified;

               // Si des fichiers sont renseignés on les récupère
               if (Request.Files.Count > 0) { tache.fichiers = ""; }
               for (int i = 0; i < Request.Files.Count; i++)
               {
                  HttpPostedFileBase currentFile = Request.Files[i];
                  if (currentFile != null && currentFile.ContentLength > 0)
                  {
                     var fileName = Path.GetFileName(currentFile.FileName);
                     var path = Path.Combine(Server.MapPath("~/Fichiers"), fileName);
                     if (i > 0) tache.fichiers += ";";
                     tache.fichiers += fileName;
                     currentFile.SaveAs(path);
                  }
               }

               db.SaveChanges();
            }
            return RedirectToAction("AjoutTache");

         }
         catch
         {
            return HttpNotFound();
         }

      }

      public ActionResult TachesNonAffecte ()
      {
         try
         {
            
            ViewBag.listeNonAffecte = db.Taches.ToList().Where(r => r.utilisateur == null);
            ViewBag.nbNonAffecte = db.Taches.ToList().Where(r => r.utilisateur == null).Count();

            return View();
         }
         catch (Exception)
         {

            return HttpNotFound();
         }
      }

      public ActionResult  SupprimerTache(int id)
      {
         try
         {
            Tach tache = db.Taches.Find(id);
            if (tache != null)
            {
               db.Taches.Remove(tache);
               db.SaveChanges();
            }

         }
         // todo : gerer les differentes exceptions attention souvent innerException il faudrait sauvegarder dans un fichier ou base et envoyer
         // un mail à l'administrateur avec le service déja présent
         catch (Exception)
         {
            return HttpNotFound();
         }
         return RedirectToAction("AjoutTache");
      }

   }
}