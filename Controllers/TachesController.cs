﻿using System;
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

      public ActionResult AjoutTache(string tri = "nom")
      {
         try
         {
            var ListeTaches =
            from taches in db.Taches
            join utilisateurs in db.Utilisateurs on taches.utilisateur equals utilisateurs.id
            orderby utilisateurs.nom
            select taches;
            ViewBag.Message = "Gérer vos taches ";
            if ((tri == "importance"))
            {
                ListeTaches =
               from taches in db.Taches
               join utilisateurs in db.Utilisateurs on taches.utilisateur equals utilisateurs.id
               orderby taches.importance
               select taches;
            }
            else if ((tri == "date"))
            {
               ListeTaches =
              from taches in db.Taches
              join utilisateurs in db.Utilisateurs on taches.utilisateur equals utilisateurs.id
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

      [HttpGet]
      public ActionResult affichageTrie()
      {
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
         catch (Exception)
         {
            return HttpNotFound();
         }
         return RedirectToAction("AjoutTache");
      }

   }
}