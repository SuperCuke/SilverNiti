using SilverNiti.Core.ViewModels;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using System;
using SilverNiti.Core.Domain;

namespace SilverNiti.Core.Controllers.Surface
{
    public class ContactFormController : SurfaceController
    {
        private Func<SilverNitiDb> _dbFactory;

        public ContactFormController(Func<SilverNitiDb> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        [ChildActionOnly]
        public ActionResult Index(string email)
        {
            var viewModel = new ContactFormViewModel();
            return PartialView("ContactForm", viewModel);
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var message = new ContactFormMessage(model.Name, model.Email, model.Message);
            using (var db = _dbFactory())
            {
                db.ContactFormMessages.Add(message);
                db.SaveChanges();
            }
            TempData["success"] = true;

            return RedirectToCurrentUmbracoPage();
        }
    }
}
