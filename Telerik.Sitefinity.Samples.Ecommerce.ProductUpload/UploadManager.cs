using System;
using System.Linq;
using System.Collections.Generic;
using Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Import;
using Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload
{
    public class UploadManager
    {
        private readonly UploadConfig configuration;

        public UploadManager()
        {
            this.configuration = new UploadConfig();
        }

        public UploadManager(UploadConfig configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            this.configuration = configuration;
        }

        public ImportStatistic ImportProductsFromCsvFile(string filePath)
        {
            CsvData csvData = CsvProcessor.ParseFileAndGetCsvData(filePath, configuration);

            List<ProductImportModel> dataToInsertInDatabase = CsvTransformer.ConvertCsvDataToProductImportModel(csvData, configuration);

            return ProductImporter.SaveProducts(dataToInsertInDatabase, configuration);
        }

        public ImportStatistic ImportProductsVariationsFromCsvFile(string filePath)
        {
            CsvData csvData = CsvProcessor.ParseFileAndGetCsvData(filePath, configuration);

            List<ProductVariationImportModel> dataToInsertInDatabase = CsvTransformer.ConvertCsvDataToProductVariationImportModel(csvData, configuration);

            return ProductImporter.SaveProductVariations(dataToInsertInDatabase, configuration);
        }

        
       
        
    }
}
