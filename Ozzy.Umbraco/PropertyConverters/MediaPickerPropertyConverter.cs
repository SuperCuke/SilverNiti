using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Ozzy.Umbraco.PropertyConverters
{
  [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.ContentCache)]
  [PropertyValueType(typeof (IPublishedContent))]
  public class MediaPickerPropertyConverter : PropertyValueConverterBase
  {
    public override bool IsConverter(PublishedPropertyType propertyType)
    {
      return propertyType.PropertyEditorAlias.Equals("Umbraco.MediaPicker");
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
      if (UmbracoContext.Current != null)
        return (object) new UmbracoHelper(UmbracoContext.Current).TypedMedia(source);
      return (object) null;
    }
  }
}
