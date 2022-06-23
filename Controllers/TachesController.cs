using System;
using System.Collections.Generic;
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

      public ActionResult AjoutTache()
      {
         try
         {
            ViewBag.Message = "Gérer vos taches ";
            ViewBag.ListeTaches = db.Taches.ToList();
            ViewBag.listeUtilisateurs = db.Utilisateurs.ToList();

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
                
               db.Taches.Add(tache);
               db.SaveChanges();
               //RedirectToAction("AjoutTache");
            }
            return RedirectToAction("AjoutTache");
         }
         catch (Exception E)
         {

            return HttpNotFound();
         }
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
         catch (Exception)
         {
            return HttpNotFound();
         }
         return RedirectToAction("AjoutTache");
      }

   }
}