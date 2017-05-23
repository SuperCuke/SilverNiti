using Archetype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return model.Select<ArchetypeFieldsetModel, NavigationItem>((Func<ArchetypeFieldsetModel, NavigationItem>)(fieldset => new NavigationItem(fieldset))).ToList<NavigationItem>();
        }
    }
}
