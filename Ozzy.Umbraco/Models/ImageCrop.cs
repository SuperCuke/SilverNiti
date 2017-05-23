using Umbraco.Core.Models;
using Umbraco.Web;

namespace Ozzy.Umbraco.Models
{
  public class ImageCrop
  {
    public IPublishedContent ContentNode { get; set; }

    public ImageCrop(IPublishedContent node)
    {
      this.ContentNode = node;
    }

    public object AsDynamicCrop()
    {
      return this.ContentNode.AsDynamic();
    }
  }
}
