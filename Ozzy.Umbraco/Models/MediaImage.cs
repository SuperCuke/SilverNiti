using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Ozzy.Umbraco.Models
{
  public class MediaImage
  {
    public string Url { get; set; }

    public virtual bool HasValue
    {
      get
      {
        return !string.IsNullOrEmpty(this.Url);
      }
    }

    public IPublishedContent Media { get; set; }

    public ImageCrop MediaCrop { get; set; }

    public MediaImage(IPublishedContent media)
    {
      this.Media = media;
      this.MediaCrop = new ImageCrop(media);
      this.Url = this.Media.GetCropUrl(new int?(), new int?(), "umbracoFile", (string) null, new int?(), new ImageCropMode?(), new ImageCropAnchor?(), false, false, true, (string) null, new ImageCropRatioMode?(), true);
    }
  }
}
