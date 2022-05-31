using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WAGestPerso.Models;

namespace WAGestPerso.Controllers
{

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
               db.Taches.Add(tache);
               db.SaveChanges();
               //RedirectToAction("AjoutTache");
            }
            return RedirectToAction("AjoutTache");
         }
         catch (Exception)
         {

            return HttpNotFound();
         }
      }
   }


}