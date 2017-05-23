using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Ozzy.Umbraco.Models
{
    public class ListItem
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int SortOrder { get; set; }
        public ListItem(PreValue value)
        {
            Id = value.Id;
            Value = value.Value;
            SortOrder = value.SortOrder;
        }
        public ListItem() { }
    }
}
