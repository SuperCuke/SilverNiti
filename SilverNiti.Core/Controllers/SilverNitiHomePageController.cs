using Ozzy.Umbraco;
using Ozzy.Umbraco.DocumentTypes;
using Ozzy.Umbraco.ViewModels;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace SilverNiti.Core.Controllers
{
    public class SilverNitiHomePageController : BaseController
    {        
        public override ActionResult Index(RenderModel model)
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