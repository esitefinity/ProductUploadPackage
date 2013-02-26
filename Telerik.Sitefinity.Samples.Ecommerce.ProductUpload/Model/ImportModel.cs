using Telerik.Sitefinity.Modules.Ecommerce.Catalog.Model;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model
{
    public class ImportModel
    {
        public string[] CorrespondingRowData
        {
            get;
            internal set;
        }

        public TrackInventory TrackInventory { get; set; }
        public int InventoryAmount { get; set; }
        public OutOfStockOption OutOfStockOption { get; set; }
    }
}
