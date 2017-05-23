using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Ozzy.Umbraco.PropertyConverters
{
  [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.ContentCache)]
  public class MultipleMediaPickerPropertyConverter : PropertyValueConverterBase, IPropertyValueConverterMeta, IPropertyValueConverter
  {
    public override bool IsConverter(PublishedPropertyType propertyType)
    {
      return propertyType.PropertyEditorAlias.Equals("Umbraco.MultipleMediaPicker");
    }

    public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
    {
      if (MultipleMediaPickerPropertyConverter.IsMultipleDataType(propertyType.DataTypeId))
        return (object) ((IEnumerable<string>) source.ToString().Split(new string[1]
        {
          ","
        }, StringSplitOptions.RemoveEmptyEntries)).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
      Attempt<int> attempt = source.TryConvertTo<int>();
      if (attempt.Success)
        return (object) attempt.Result;
      if (((IEnumerable<string>) source.ToString().Split(new string[1]
      {
        ","
      }, StringSplitOptions.RemoveEmptyEntries)).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>().Length > 0)
      {
        string message = string.Format("Data type \"{0}\" is not set to allow multiple items but appears to contain multiple items, check the setting and save the data type again", (object) ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionById(propertyType.DataTypeId).Name);
        LogHelper.Warn<MultipleMediaPickerPropertyConverter>(message);
        throw new Exception(message);
      }
      return (object) null;
    }

    public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
    {
      if (source == null)
        return (object) null;
      if (UmbracoContext.Current == null)
        return (object) null;
      UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
      if (MultipleMediaPickerPropertyConverter.IsMultipleDataType(propertyType.DataTypeId))
      {
        int[] numArray = (int[]) source;
        IEnumerable<IPublishedContent> publishedContents = Enumerable.Empty<IPublishedContent>();
        if (numArray.Length > 0)
          publishedContents = umbracoHelper.TypedMedia(numArray).Where<IPublishedContent>((Func<IPublishedContent, bool>) (x => x != null));
        return (object) publishedContents;
      }
      int id = (int) source;
      return (object) umbracoHelper.TypedMedia(id);
    }

    public Type GetPropertyValueType(PublishedPropertyType propertyType)
    {
      if (!MultipleMediaPickerPropertyConverter.IsMultipleDataType(propertyType.DataTypeId))
        return typeof (IPublishedContent);
      return typeof (IEnumerable<IPublishedContent>);
    }

    public PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType, PropertyCacheValue cacheValue)
    {
      return PropertyCacheLevel.ContentCache;
    }

    private static bool IsMultipleDataType(int dataTypeId)
    {
      PreValue preValue = ApplicationContext.Current.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dataTypeId).PreValuesAsDictionary.FirstOrDefault<KeyValuePair<string, PreValue>>((Func<KeyValuePair<string, PreValue>, bool>) (x => string.Equals(x.Key, "multiPicker", StringComparison.InvariantCultureIgnoreCase))).Value;
      if (preValue != null)
        return preValue.Value.TryConvertTo<bool>().Result;
      return false;
    }
  }
}
