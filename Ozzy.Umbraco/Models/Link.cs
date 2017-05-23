using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Ozzy.Umbraco.Models
{
  public class Link
  {
    public static Link Empty = new Link();

    public IPublishedContent PublishedItem { get; set; }

    public int? Id { get; private set; }

    public string Name { get; private set; }

    public LinkType Type { get; private set; }

    public string Url { get; private set; }

    public bool InternalLinkDeleted { get; private set; }

    public string Target { get; private set; }

    private Link()
    {
    }

    public Link(JToken linkItem)
    {
      this.Id = linkItem.Value<int?>((object) "id");
      this.Name = linkItem.Value<string>((object) "name");
      this.Type = this.Id.HasValue ? (linkItem.Value<bool>((object) "isMedia") ? LinkType.Media : LinkType.Content) : LinkType.External;
      if (this.Id.HasValue && UmbracoContext.Current != null)
      {
        UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        this.PublishedItem = this.Type == LinkType.Content ? umbracoHelper.TypedContent(this.Id.Value) : umbracoHelper.TypedMedia(this.Id.Value);
      }
      this.Url = this.PublishedItem != null ? this.PublishedItem.Url : linkItem.Value<string>((object) "url");
      if (this.Id.HasValue && this.PublishedItem == null)
      {
        this.InternalLinkDeleted = true;
        this.Url = "#";
      }
      this.Target = linkItem.Value<string>((object) "target");
    }

    public Link(IPublishedContent content, string target = "_blank")
    {
      this.Id = new int?(content.Id);
      this.Name = content.Name;
      this.Type = content.ItemType == PublishedItemType.Content ? LinkType.Content : LinkType.Media;
      this.PublishedItem = content;
      this.Url = content.Url;
      this.InternalLinkDeleted = false;
      this.Target = target;
    }
  }
}
