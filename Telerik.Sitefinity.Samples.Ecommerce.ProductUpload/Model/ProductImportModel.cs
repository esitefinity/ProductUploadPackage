using System;
using System.Linq;
using System.Collections.Generic;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model
{
    public class ProductImportModel : ImportModel
    {
        public ProductImportModel()
        {

        }
        public string Title { get; set; }
        public string ProductTypeTitle { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string Sku { get; set; }
        public List<string> ImagesPath { get; set; }
        public List<string> Departments { get; set; }
        public List<string> Tags { get; set; }
        public bool IsActive { get; set; }

        public List<CustomFieldData> CustomFieldData { get; set; }
    }
}