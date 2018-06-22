using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //初始化数据库
            Database.SetInitializer(new CreateDatabaseIfNotExists<BamnContext>());
            using (var bamnContext = new BamnContext())
            {
                //如果不存在数据库则创建
                bamnContext.Database.Initialize(true);
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //HttpCookie authcookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            //FormsAuthenticationTicket authticket = FormsAuthentication.Decrypt(authcookie.Value);
            ////解密 
            //Context.User = new GenericPrincipal(Context.User.Identity, null);//存到httpcontext.user中 

        }
    }
}
