using System.Collections.Generic;
using System.Linq;

namespace Ozzy.Umbraco.Models
{
    public class CssClassMapper
    {
        public static readonly CssClassMapper ButtonStyles = new CssClassMapper(new Dictionary<string, string>()
        {
            {"Style 1","btn-style-one"},
            {"Style 2","btn-style-two"},
            {"Style 3","btn-style-three"}
        }, "btn-style-one");

        private Dictionary<string, string> _mapping;
        private string _defaultValue;

        public CssClassMapper(Dictionary<string, string> maping, string defaultValue = "")
        {
            _mapping = maping;
            _defaultValue = defaultValue;
        }

        public string GetClassName(string value)
        {
            if (value == null) return _defaultValue;
            return _mapping.TryGetValue(value, out var result) ? result : _defaultValue;
        }

        public string GetClassName(IEnumerable<string> value)
        {
            var classes = value
                .Select(v => GetClassName(v))
                .Distinct();
            return string.Join(" ", classes);
        }
    }
}
