using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog.Model;
using Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Import
{
    internal class CsvTransformer
    {
        internal static List<ProductImportModel> ConvertCsvDataToProductImportModel(CsvData csvData, UploadConfig config)
        {
            List<ProductImportModel> dataToInsertInDatabase = new List<ProductImportModel>();
            foreach (var dataRow in csvData.Rows)
            {
                ProductImportModel rowToInsert = new ProductImportModel();
                rowToInsert.Title = dataRow[0];
                rowToInsert.ProductTypeTitle = dataRow[1];
                rowToInsert.Description = dataRow[2];
                rowToInsert.Url = dataRow[3];
                rowToInsert.Price = Convert.ToDecimal(dataRow[4]);
                rowToInsert.Weight = Convert.ToDecimal(dataRow[5]);
                rowToInsert.Sku = dataRow[6];

                rowToInsert.ImagesPath = dataRow[7].Split(config.MultipleItemsSeparator).ToList();
                rowToInsert.DocumentsAndFilesPath = dataRow[8].Split(config.MultipleItemsSeparator).ToList();
                rowToInsert.Departments = dataRow[9].Split(config.MultipleItemsSeparator).ToList();
                rowToInsert.Tags = dataRow[10].Split(config.MultipleItemsSeparator).ToList();

                rowToInsert.TrackInventory = GetTrackInventory(dataRow[11]);
                rowToInsert.InventoryAmount = GetSafeInt(dataRow[12]);
                rowToInsert.OutOfStockOption = GetOutOfStockOption(dataRow[13]);

                rowToInsert.IsActive = Convert.ToBoolean(dataRow[14]);

                rowToInsert.CustomFieldData = new List<CustomFieldData>();

                for (int i = 15; i < csvData.Header.Length; i++)
                {
                    CustomFieldData customFieldData = new CustomFieldData { PropertyName = csvData.Header[i], PropertyValue = dataRow[i] };
                    rowToInsert.CustomFieldData.Add(customFieldData);
                }

                rowToInsert.CorrespondingRowData = dataRow;

                dataToInsertInDatabase.Add(rowToInsert);
            }
            return dataToInsertInDatabase;
        }


        internal static List<ProductVariationImportModel> ConvertCsvDataToProductVariationImportModel(CsvData csvData, UploadConfig config)
        {
            List<ProductVariationImportModel> dataToInsertInDatabase = new List<ProductVariationImportModel>();
            foreach (var dataRow in csvData.Rows)
            {
                ProductVariationImportModel rowToInsert = new ProductVariationImportModel();
                rowToInsert.ProductNameSku = dataRow[0];
                rowToInsert.AttributeName = dataRow[1];
                rowToInsert.ValueName = dataRow[2];
                rowToInsert.Sku = dataRow[3];
                rowToInsert.AdditionalPrice = Convert.ToDecimal(dataRow[4]);
                rowToInsert.TrackInventory = GetTrackInventory(dataRow[5]);
                rowToInsert.InventoryAmount = GetSafeInt(dataRow[6]);
                rowToInsert.OutOfStockOption = GetOutOfStockOption(dataRow[7]);
                rowToInsert.IsActive = Convert.ToBoolean(dataRow[8]);

                rowToInsert.CorrespondingRowData = dataRow;

                dataToInsertInDatabase.Add(rowToInsert);
            }
            return dataToInsertInDatabase;
        }


        #region Private Methods
        private static int GetSafeInt(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return 0;
            }
            return Convert.ToInt32(stringValue);
        }

        private static TrackInventory GetTrackInventory(string trackInventoryString)
        {
            if (trackInventoryString == "1")
            {
                return TrackInventory.Track;
            }
            if (trackInventoryString == "2")
            {
                return TrackInventory.TrackByVariations;
            }
            return TrackInventory.DonotTrack; //return this by default
        }


        private static OutOfStockOption GetOutOfStockOption(string outOfStockOptionString)
        {
            if (outOfStockOptionString == "0")
            {
                return OutOfStockOption.DisplayAndAllowOrders;
            }
            if (outOfStockOptionString == "2")
            {
                return OutOfStockOption.DoNotDisplayTheProduct;
            }
            return OutOfStockOption.DisplayButDontAllowOrders; //return this by default
        }

        #endregion
    }
}
