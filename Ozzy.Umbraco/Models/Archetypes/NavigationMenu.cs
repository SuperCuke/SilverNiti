using Archetype.Models;
using System.Collections.Generic;
using System.Linq;

namespace Ozzy.Umbraco.Models.Archetypes
{
    public class NavigationMenu : List<NavigationItem>
    {
        public NavigationMenu(IEnumerable<NavigationItem> items)
          : base(items)
        {
        }

        public NavigationMenu(IEnumerable<ArchetypeFieldsetModel> model)
        {
            foreach (ArchetypeFieldsetModel fieldset in model)
                this.Add(new NavigationItem(fieldset));
        }

        public static List<NavigationItem> GetItems(ArchetypeModel model)
        {
            return model.Select(fieldset => new NavigationItem(fieldset)).ToList();
        }
    }
}
