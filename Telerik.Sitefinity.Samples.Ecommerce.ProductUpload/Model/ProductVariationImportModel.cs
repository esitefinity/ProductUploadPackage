
namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model
{
    public class ProductVariationImportModel : ImportModel
    {
        public string ProductNameSku { get; set; }
        public string AttributeName { get; set; }
        public string ValueName { get; set; }
        public string Sku { get; set; }
        public decimal AdditionalPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
