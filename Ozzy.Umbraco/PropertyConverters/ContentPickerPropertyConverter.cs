using System.Collections.Generic;
using System.Globalization;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Ozzy.Umbraco.PropertyConverters
{
  [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.ContentCache)]
  [PropertyValueType(typeof (IPublishedContent))]
  public class ContentPickerPropertyConverter : PropertyValueConverterBase
  {
    private static readonly List<string> _propertiesToExclude = new List<string>()
    {
      "umbracoInternalRedirectId".ToLower(CultureInfo.InvariantCulture),
      "umbracoRedirect".ToLower(CultureInfo.InvariantCulture)
    };

    public override bool IsConverter(PublishedPropertyType propertyType)
    {
      return propertyType.PropertyEditorAlias.Equals("Umbraco.ContentPickerAlias");
    }

    public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
    {
      Attempt<int> attempt = source.TryConvertTo<int>();
      if (attempt.Success)
        return (object) attempt.Result;
      return (object) null;
    }

    public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
    {
      if (source == null)
        return (object) null;
      if (UmbracoContext.Current != null && (propertyType.PropertyTypeAlias == null || !ContentPickerPropertyConverter._propertiesToExclude.Contains(propertyType.PropertyTypeAlias.ToLower(CultureInfo.InvariantCulture))))
        return (object) new UmbracoHelper(UmbracoContext.Current).TypedContent(source);
      return source;
    }

    public override object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview)
    {
      return (object) source.ToString();
    }
  }
}
