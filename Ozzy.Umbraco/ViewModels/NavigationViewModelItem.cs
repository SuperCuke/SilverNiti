using Ozzy.Umbraco.Models;
using Ozzy.Umbraco.Models.Archetypes;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Ozzy.Umbraco.ViewModels
{
    public class NavigationViewModelItem
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }

        public bool IsActiveNode { get; set; }

        public IHtmlString Css { get; set; }

        public bool HasChildrens => Childrens.Any();

        public List<NavigationViewModelItem> Childrens { get; set; }

        public NavigationViewModelItem()
        {
            Childrens = new List<NavigationViewModelItem>();
            Css = MvcHtmlString.Empty;
        }

        public NavigationViewModelItem(NavigationItem menuItem, IPublishedContent currentPage) : this()
        {
            FillFrom(menuItem, currentPage);
        }

        public void FillFrom(NavigationItem menuItem, IPublishedContent currentPage)
        {
            if (menuItem == null || menuItem.Link == null || (menuItem.Link.Equals((object)Link.Empty) || menuItem.Link.InternalLinkDeleted))
                return;
            var content = menuItem.Link.PublishedItem;
            //if (content.GetPropertyValue<bool>("hideFromNavigation")) { }
            //if (content.GetPropertyValue<bool>("hideInSitemap")) { }
            //if (content.GetPropertyValue<bool>("navigationTitle")) { }
            //if (content.GetPropertyValue<bool>("showChildsInNavigation")) { }
            Title = content.GetPropertyValue<string>("navigationTitle").NotEmpty() ?? menuItem.Link.Name.NotEmpty() ?? content.Name.NotEmpty();
            Url = menuItem.Link.Url ?? content.Url;
            if (menuItem.Childrens != null && menuItem.Childrens.Any())
                Childrens.AddRange(menuItem.Childrens.Select(c => new NavigationViewModelItem(c, currentPage)));
            IsActiveNode = content.IsAncestorOrSelf(currentPage);
            IsActive = content.IsEqual(currentPage);
            Css = CreateCss();
        }

        public virtual IHtmlString CreateCss()
        {
            return (this.IsActive ? "active" : string.Empty).CssClass();
        }
    }

    public static class StringExtensions
    {
        public static string NotEmpty(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return (string)null;
            return source;
        }

        public static string NotWhiteSpace(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return (string)null;
            return source;
        }
    }

    public static class HtmlExtensions
    {
        public static IHtmlString CssClass(this string @class)
        {
            if (!@class.IsNullOrWhiteSpace())
                return (IHtmlString)MvcHtmlString.Create(string.Format(" class=\"{0}\"", (object)@class));
            return (IHtmlString)MvcHtmlString.Create(string.Empty);
        }

        public static IHtmlString CssClassIfTrue(this bool some, string @class)
        {
            if (!some)
                return (IHtmlString)MvcHtmlString.Create(string.Empty);
            return (IHtmlString)MvcHtmlString.Create(string.Format(" class=\"{0}\"", (object)@class));
        }

        public static IHtmlString HtmlIfTrue(this bool some, string html)
        {
            if (!some)
                return (IHtmlString)MvcHtmlString.Create(string.Empty);
            return (IHtmlString)MvcHtmlString.Create(html);
        }
    }
}
