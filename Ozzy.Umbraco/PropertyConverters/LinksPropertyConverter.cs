using Ozzy.Umbraco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Ozzy.Umbraco.PropertyConverters
{
    [PropertyValueType(typeof(RelatedLinks))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.ContentCache)]
    public class LinksPropertyConverter : PropertyValueConverterBase, IPropertyValueConverterMeta, IPropertyValueConverter
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            if (!propertyType.PropertyEditorAlias.Equals("Ozzy.MultiUrlPicker"))
                return propertyType.PropertyEditorAlias.Equals("Ozzy.MultiUrlPicker");
            return true;
        }

        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null)
                return (object)null;
            string propertyData = source.ToString();
            if (LinksPropertyConverter.IsMultipleDataType(propertyType.DataTypeId))
            {
                if (UmbracoContext.Current == null)
                    return (object)null;
                return (object)new RelatedLinks(propertyData);
            }
            if (UmbracoContext.Current == null)
                return (object)null;
            return (object)new RelatedLinks(propertyData).FirstOrDefault<Link>();
        }

        public Type GetPropertyValueType(PublishedPropertyType propertyType)
        {
            if (!LinksPropertyConverter.IsMultipleDataType(propertyType.DataTypeId))
                return typeof(Link);
            return typeof(IEnumerable<Link>);
        }

        public PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType, PropertyCacheValue cacheValue)
        {
            return PropertyCacheLevel.ContentCache;
        }

        private static bool IsMultipleDataType(int dataTypeId)
        {
            PreValue preValue = ApplicationContext.Current.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dataTypeId).PreValuesAsDictionary.FirstOrDefault<KeyValuePair<string, PreValue>>((Func<KeyValuePair<string, PreValue>, bool>)(x => string.Equals(x.Key, "maxNumberOfItems", StringComparison.InvariantCultureIgnoreCase))).Value;
            if (preValue == null)
                return false;
            if (!preValue.Value.Equals(string.Empty))
                return preValue.Value.TryConvertTo<int>().Result > 1;
            return true;
        }
    }
}
