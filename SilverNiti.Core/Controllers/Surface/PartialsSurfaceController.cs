using Ozzy.Umbraco.Models.Archetypes;
using Ozzy.Umbraco.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace SilverNiti.Core.Controllers
{
    public class PartialsSurfaceController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult MainNavigation()
        {
            var root = Umbraco.TypedContentAtRoot().First();
            var home = new SilverNiti.Core.ContentModels.SilverNitiHomePage(root);
            var mainMenu = new NavigationMenu(home.MainMenu);
            var navigation = new NavigationViewModel<NavigationViewModelItem>(mainMenu, CurrentPage);
            return PartialView("MainNavigation", navigation);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            var root = Umbraco.TypedContentAtRoot().First();
            var navigation = new NavigationViewModel<NavigationViewModelItem>(root);
            return PartialView("Navigation", navigation);
        }

        [ChildActionOnly]
        public ActionResult Layout()
        {
            var root = Umbraco.TypedContentAtRoot().First();
            var navigation = new NavigationViewModel<NavigationViewModelItem>(root);
            return PartialView("Navigation", navigation);
        }
    }
}
