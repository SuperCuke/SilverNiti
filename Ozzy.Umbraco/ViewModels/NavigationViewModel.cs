using Ozzy.Umbraco.Models.Archetypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace Ozzy.Umbraco.ViewModels
{
    public class NavigationViewModel<T> : List<T> where T : NavigationViewModelItem, new()
    {
        public bool IsOpen { get; set; }

        public NavigationViewModel(IPublishedContent root)
        {
            foreach (IPublishedContent children in root.Children)
            {
                T instance = Activator.CreateInstance<T>();
                instance.Title = children.Name;
                instance.Url = children.Url;
                this.Add(instance);
            }
        }

        public NavigationViewModel(IEnumerable<NavigationItem> menu, IPublishedContent currentPage)
        {
            foreach (NavigationItem menuItem in menu)
            {
                T instance = Activator.CreateInstance<T>();
                instance.FillFrom(menuItem, currentPage);
                this.Add(instance);
            }
            this.IsOpen = this.Any<T>((Func<T, bool>)(c =>
            {
                if (c.IsActiveNode)
                    return c.HasChildrens;
                return false;
            }));
        }
    }
}
