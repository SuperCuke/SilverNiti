using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Ozzy.Umbraco.DocumentTypes
{
    public interface IPageBase : IPublishedContent
    {
        string Title { get; }
        string MetaTitle { get; }
        string Keywords { get; }
        string Description { get; }
        string NavigationTitle { get; }
        bool HideFromNavigation { get; }
        bool ShowChildsInNavigation { get; }
        bool HideInSitemap { get; }
    }
}
