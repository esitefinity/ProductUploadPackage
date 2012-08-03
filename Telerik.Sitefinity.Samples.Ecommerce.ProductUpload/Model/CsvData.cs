using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model
{
    internal class CsvData
    {
        public string[] Header { get; set; }
        public List<string[]> Rows { get; set; }
    }
}
