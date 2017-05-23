using Archetype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Umbraco.Models.Archetypes
{
    public class NavigationItem
    {
        public Link Link { get; set; }

        public bool HasChildrens { get; set; }

        public NavigationMenu Childrens { get; set; }

        public NavigationItem(ArchetypeFieldsetModel fieldset)
        {
            if (!string.Equals(fieldset.Alias, "navigationMenu") && !string.Equals(fieldset.Alias, "singleItem"))
                throw new InvalidOperationException("this no a navigation menu");
            this.Link = fieldset.GetValue<Link>("item");
            if (!string.Equals(fieldset.Alias, "navigationMenu"))
                return;
            this.Childrens = new NavigationMenu((IEnumerable<ArchetypeFieldsetModel>)fieldset.GetValue<ArchetypeModel>("childrens"));
            this.HasChildrens = this.Childrens.Any<NavigationItem>();
        }
    }
}
