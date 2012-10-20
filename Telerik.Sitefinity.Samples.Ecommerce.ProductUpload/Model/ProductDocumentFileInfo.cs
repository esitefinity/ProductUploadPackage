using System.IO;
using Telerik.Sitefinity.Libraries.Model;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model
{
    internal class ProductDocumentFileInfo
    {
        public FileInfo FileInfo { get; set; }
        public Library Library { get; set; }
        public Document Document { get; set; }
    }
}
