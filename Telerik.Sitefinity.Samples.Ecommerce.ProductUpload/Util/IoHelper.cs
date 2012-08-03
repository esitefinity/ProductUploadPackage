using System;
using System.Linq;
using System.IO;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Util
{
    public class IoHelper
    {
        public static void ValidateFileExsistence(string filePath)
        {
            //Delete the file if it exsists already
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static void ValidateFolderExsistence(string folderPath)
        {
            //Create the upload folder if one doesn't exsist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}