using Ozzy.Umbraco.DocumentTypes;
using Ozzy.Umbraco.Models.Archetypes;
using Ozzy.Umbraco.ViewModels;
using SilverNiti.Core.ContentModels;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace SilverNiti.Core.ViewModels
{
    public class SilverNitiViewModel : BaseViewModel
    {
        public IPublishedContent Root { get; private set; }
        public SilverNitiHomePage Home { get; private set; }
        public NavigationMenu Navigation { get; private set; }
        public NavigationViewModel<NavigationViewModelItem> NavigationViewModel { get; private set; }

        public SilverNitiViewModel(IPageBase content) : this(content, new UmbracoHelper(UmbracoContext.Current))
        {
        }

        public SilverNitiViewModel(IPageBase content, UmbracoHelper umbraco) : base(content)
        {
            Root = umbraco.TypedContentAtRoot().First();
            Home = new SilverNitiHomePage(Root);
            Navigation = new NavigationMenu(Home.MainMenu);
            NavigationViewModel = new NavigationViewModel<NavigationViewModelItem>(Navigation, Content);
        }
    }
}
