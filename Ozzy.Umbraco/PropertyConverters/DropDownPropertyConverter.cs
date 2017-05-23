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
    [PropertyValueType(typeof(object))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.ContentCache)]
    public class DropDownPropertyConverter : PropertyValueConverterBase, IPropertyValueConverterMeta, IPropertyValueConverter
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {

            return propertyType.PropertyEditorAlias.Equals("Umbraco.DropDown");

        }

        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null) return null;
            var preValueCollection = ApplicationContext.Current.Services.DataTypeService
                .GetPreValuesCollectionByDataTypeId(propertyType.DataTypeId);
            var preValue = preValueCollection.IsDictionaryBased ?
                preValueCollection.PreValuesAsDictionary.FirstOrDefault(p => p.Value.Id == (int)source).Value :
                preValueCollection.PreValuesAsArray.FirstOrDefault(p => p.Id == (int)source);
            return new ListItem(preValue);
        }

        public Type GetPropertyValueType(PublishedPropertyType propertyType)
        {
            return typeof(object);
        }

        public PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType, PropertyCacheValue cacheValue)
        {
            return PropertyCacheLevel.ContentCache;
        }
    }
}
