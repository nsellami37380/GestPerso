using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

namespace WAGestPerso
{
   public class Startup
   {
      [assembly:OwinStartupAttribute(typeof(WAGestPerso.Startup))]
      public void Configuration(IAppBuilder app)
      {
         //ConfigureAuth(app);
      }
      public void PopulateUserAndRoles()
      {

      }
   }
}