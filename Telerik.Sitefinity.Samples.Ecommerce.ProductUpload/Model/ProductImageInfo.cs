using System.IO;
using Telerik.Sitefinity.Libraries.Model;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model
{
    internal class ProductImageInfo
    {
        public FileInfo ImageInfo { get; set; }
        public Album Album { get; set; }
        public Image Image { get; set; }
    }
}
