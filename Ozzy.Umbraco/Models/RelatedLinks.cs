using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Umbraco.Core.Logging;

namespace Ozzy.Umbraco.Models
{
  public class RelatedLinks : List<Link>
  {
    public RelatedLinks(string propertyData)
    {
      foreach (JToken linkItem in JsonConvert.DeserializeObject<JArray>(propertyData))
      {
        Link link = new Link(linkItem);
        if (!link.InternalLinkDeleted)
          this.Add(link);
        else
          LogHelper.Warn<RelatedLinks>(string.Format("Related Links value converter skipped a link as the node has been unpublished/deleted (Internal Link NodeId: {0}, Link Caption: \"{1}\")", (object) link.Url, (object) link.Name));
      }
    }

    public RelatedLinks(IEnumerable<Link> links)
    {
      foreach (Link link in links)
        this.Add(link);
    }
  }
}
