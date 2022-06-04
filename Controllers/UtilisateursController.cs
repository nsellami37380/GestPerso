﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WAGestPerso.Models;
using System.Data.Entity;

namespace WAGestPerso.Controllers
{
    public class UtilisateursController : Controller
    {
      BD_GESTPERSOEntities db = new BD_GESTPERSOEntities();
      // GET: Utilisateurs
      public ActionResult AjoutUtilisateur()
        {
         try
         {
           
            ViewBag.listeUtilisateurs = db.Utilisateurs.ToList();
            return View();
         }
         catch (Exception)
         {
            return HttpNotFound();
         } 

        }

      [HttpPost]
      public ActionResult AjoutUtilisateur(Utilisateur utilisateur)
      {
         try
         {
            if (ModelState.IsValid)
            {

               if (Request.Files.Count > 0)
               {
                  var file = Request.Files[0];
                  if (file != null && file.ContentLength > 0)
                  {
                     var fileName = Path.GetFileName(file.FileName);
                     var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                     file.SaveAs(path);
                     utilisateur.photo = fileName;
                  }
               }


               db.Utilisateurs.Add(utilisateur);
               db.SaveChanges();
              
            }
            return RedirectToAction("AjoutUtilisateur");
         }
         catch (Exception)
         {
            return HttpNotFound();
         }
      }

      
       public ActionResult ModifierUtilisateur(int id)
      {
         try
         {
            ViewBag.listeUtilisateurs = db.Utilisateurs.ToList();
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur != null)
            {
               return View("AjoutUtilisateur", utilisateur);
            }
            return RedirectToAction("AjoutUtilisateur");
         }
         catch (Exception)
         {

            return HttpNotFound();
         }
      }

      [HttpPost]
      public ActionResult ModifierUtilisateur(Utilisateur utilisateur)
      {
         try
         {
            if (ModelState.IsValid)
            {
               db.Entry(utilisateur).State = EntityState.Modified;
               db.SaveChanges();
            }
            return RedirectToAction("AjoutUtilisateur");
         }
         catch
         {
            return HttpNotFound();
         }
      }

      public ActionResult SupprimerUtilisateur(int id)
      {
         try
         {
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur != null)
            {
               db.Utilisateurs.Remove(utilisateur);
               db.SaveChanges();
            }

         }
         catch (Exception)
         {
            return HttpNotFound();
         }
         return RedirectToAction("AjoutUtilisateur");
      }


   }
}