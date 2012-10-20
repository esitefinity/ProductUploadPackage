using System;
using System.Collections.Generic;
using System.Linq;
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

                rowToInsert.IsActive = Convert.ToBoolean(dataRow[11]);

                rowToInsert.CustomFieldData = new List<CustomFieldData>();

                for (int i = 12; i < config.NumberOfColumns; i++)
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
                rowToInsert.IsActive = Convert.ToBoolean(dataRow[5]);

                rowToInsert.CorrespondingRowData = dataRow;

                dataToInsertInDatabase.Add(rowToInsert);
            }
            return dataToInsertInDatabase;
        }
  

    }
}
