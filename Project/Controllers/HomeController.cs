using Project.Models;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Archive()
        {
            ArchiveModel model = new ArchiveModel();
            return View(model);
        }

        public int GameCommander(int index, string command, int param)
        {
            GameEngine Engine = Session["GameEngine"] as GameEngine;

            switch(command)
            {
                case "new":
                    return Engine.NewGame();
                case "step":
                    return Engine.Step(index, param);
                default:
                    return -1000;
            }
        }
    }
}