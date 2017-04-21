using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project
{
    public class MvcApplication : System.Web.HttpApplication
    {
        void Session_Start(object sender, EventArgs e)
        {
            Session.Add("GameEngine", new GameEngine());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
