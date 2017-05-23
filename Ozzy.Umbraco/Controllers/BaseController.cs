using Ozzy.Umbraco.DocumentTypes;
using Ozzy.Umbraco.ViewModels;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Ozzy.Umbraco
{
    public class BaseController : SurfaceController, IRenderMvcController, IController
    {
        protected bool EnsurePhysicalViewExists(string template)
        {
            if (ViewEngines.Engines.FindView(this.ControllerContext, template, (string)null).View != null)
                return true;
            LogHelper.Warn<RenderMvcController>("No physical template file was found for template " + template);
            return false;
        }

        protected ActionResult CurrentTemplate<T>(T model)
        {
            string str = this.ControllerContext.RouteData.Values["action"].ToString();
            if (!this.EnsurePhysicalViewExists(str))
                return (ActionResult)this.Content("");
            return (ActionResult)this.View(str, (object)model);
        }

        public virtual ActionResult Index(RenderModel model)
        {
            var page = model.Content as IPageBase;
            if (page != null)
            {
                return this.CurrentTemplate<BaseViewModel>(new BaseViewModel(page));
            }
            else
            {
                return this.CurrentTemplate(model.Content);
            }

        }
    }
}
