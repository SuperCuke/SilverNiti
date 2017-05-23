using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Ozzy.Umbraco
{
    public static class ArchetypeHelpers
    {
        const string ContentPartial = "~/App_Plugins/Ozzy/Grid/Content/content.cshtml";
        public static MvcHtmlString ArchetypeContent(this HtmlHelper htmlHelper, object content)
        {
            var viewData = new ViewDataDictionary() { { "model", content } };
            return htmlHelper.Partial(ContentPartial, viewData);
        }
    }
}
