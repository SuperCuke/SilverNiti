using Ozzy.Umbraco;
using Ozzy.Umbraco.DocumentTypes;
using SilverNiti.Core.ViewModels;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace SilverNiti.Core.Controllers
{
    public class SilverNitiPageController : BaseController
    {
        public override ActionResult Index(RenderModel model)
        {
            var page = model.Content as IPageBase;
            if (page != null)
            {
                return this.CurrentTemplate<SilverNitiViewModel>(new SilverNitiViewModel(page, Umbraco));
            }
            else
            {
                return this.CurrentTemplate(model.Content);
            }
        }
    }
}