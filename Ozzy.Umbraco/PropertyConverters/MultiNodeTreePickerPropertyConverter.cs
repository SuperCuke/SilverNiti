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
  [PropertyValueType(typeof (IEnumerable<IPublishedContent>))]
  [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.ContentCache)]
  public class MultiNodeTreePickerPropertyConverter : PropertyValueConverterBase
  {
    public override bool IsConverter(PublishedPropertyType propertyType)
    {
      return propertyType.PropertyEditorAlias.Equals("Umbraco.MultiNodeTreePicker");
    }

    public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
    {
      return (object) ((IEnumerable<string>) source.ToString().Split(new string[1]
      {
        ","
      }, StringSplitOptions.RemoveEmptyEntries)).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
    }

    public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
    {
      if (source == null)
        return (object) null;
      int[] numArray = (int[]) source;
      IEnumerable<IPublishedContent> publishedContents = Enumerable.Empty<IPublishedContent>();
      if (UmbracoContext.Current == null)
        return (object) null;
      UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
      if (numArray.Length <= 0)
        return (object) publishedContents;
      List<IPublishedContent> source1 = new List<IPublishedContent>();
      foreach (int id in numArray)
      {
        UmbracoObjectTypes umbracoObjectTypes = UmbracoObjectTypes.Unknown;
        try
        {
          umbracoObjectTypes = ApplicationContext.Current.Services.EntityService.GetObjectType(id);
        }
        catch (Exception ex)
        {
        }
        if (umbracoObjectTypes == UmbracoObjectTypes.Document)
        {
          IPublishedContent publishedContent = umbracoHelper.TypedContent(id);
          source1.Add(publishedContent);
        }
        else if (umbracoObjectTypes == UmbracoObjectTypes.Media)
        {
          IPublishedContent publishedContent = umbracoHelper.TypedMedia(id);
          source1.Add(publishedContent);
        }
        else if (umbracoObjectTypes == UmbracoObjectTypes.Member)
        {
          IPublishedContent publishedContent = umbracoHelper.TypedMember(id);
          source1.Add(publishedContent);
        }
      }
      return (object) source1.Where<IPublishedContent>((Func<IPublishedContent, bool>) (x => x != null));
    }
  }
}
