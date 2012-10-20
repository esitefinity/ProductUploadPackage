using System;
using System.Linq;
using Telerik.Sitefinity.Modules.Libraries;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload
{
    public class UploadConfig
    {
        private int numberOfColumns = 8;
        private Guid uploadToAlbumId = LibrariesModule.DefaultImagesLibraryId;
        private Guid uploadToLibraryId = LibrariesModule.DefaultDocumentsLibraryId;
        private char multipleItemsSeparator = '|';

        public virtual int NumberOfColumns
        {
            get
            {
                return numberOfColumns;
            }
            set
            {
                numberOfColumns = value;
            }
        }

        public virtual Guid UploadToAlbumId
        {
            get
            {
                return uploadToAlbumId;
            }
            set
            {
                uploadToAlbumId = value;
            }
        }

        public virtual Guid UploadToLibraryId
        {
            get
            {
                return uploadToLibraryId;
            }
            set
            {
                uploadToLibraryId = value;
            }
        }

        public virtual char MultipleItemsSeparator
        {
            get
            {
                return multipleItemsSeparator;
            }
            set
            {
                multipleItemsSeparator = value;
            }
        }
    }
}
