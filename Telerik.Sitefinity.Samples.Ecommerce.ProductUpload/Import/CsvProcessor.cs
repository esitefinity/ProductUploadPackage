using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Import
{
    internal class CsvProcessor
    {
        internal static CsvData ParseFileAndGetCsvData(string filePath, UploadConfig config)
        {
            CsvConfiguration configuration = new CsvConfiguration();
            configuration.HasHeaderRecord = true;

            CsvReader csvReader = new CsvReader(new StreamReader(filePath), configuration);

            string[] header = default(string[]);
            
            List<string[]> rows = new List<string[]>();
            string[] row;
            while (csvReader.Read())
            {
                header = csvReader.FieldHeaders;

                row = new string[config.NumberOfColumns];
                for (int j = 0; j < config.NumberOfColumns; j++)
                {
                    row[j] = csvReader.GetField(j);
                }
                
                rows.Add(row);
            }

            return new CsvData { Header = header, Rows = rows };
        }
    }
}
