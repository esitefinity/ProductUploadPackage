using System.Collections.Generic;
using Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.ErrorHandling;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model
{
    public class ImportStatistic
    {
        public int TotalNumberOfRecordsProcessed { get; set; }
        public int NumberOfSuccessfulRecords { get; set; }
        public int NumberOfFailedRecords { get; set; }
        public List<ImportError> Errors { get; set; }
    }
}
