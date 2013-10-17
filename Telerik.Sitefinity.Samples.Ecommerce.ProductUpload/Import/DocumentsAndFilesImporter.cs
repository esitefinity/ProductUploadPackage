using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model;
using Telerik.Sitefinity.Ecommerce.Catalog.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Import
{
    internal class DocumentsAndFilesImporter
    {
        internal static List<ProductDocumentFileInfo> ImportDocuments(List<string> documentsAndFilesPath, UploadConfig config)
        {
            List<ProductDocumentFileInfo> productDocumentFileInfos = new List<ProductDocumentFileInfo>();

            LibrariesManager librariesManager = LibrariesManager.GetManager();

            Library libraryToUploadDocumentsTo = librariesManager.GetDocumentLibrary(config.UploadToLibraryId); 

            foreach (var documentPath in documentsAndFilesPath)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(documentPath);
                    if (fileInfo == null)
                    {
                        continue;
                    }

                    //To create an image you have to be logged in as an admin.
                    Document document = librariesManager.CreateDocument();

                    var extension = fileInfo.Extension;
                    var documentTitle = fileInfo.Name;
                    if (extension.Length > 0)
                    {
                        documentTitle = documentTitle.Substring(0, documentTitle.Length - extension.Length);
                    }
                    document.Parent = libraryToUploadDocumentsTo;
                    document.Title = documentTitle;
                    document.UrlName = documentTitle.ToLower().Replace(' ', '-');
                    librariesManager.RecompileItemUrls<Document>(document);
                    using (var fileStream = fileInfo.OpenRead())
                    {
                        librariesManager.Upload(document, fileStream, fileInfo.Extension);
                    }
                    librariesManager.SaveChanges();

                    Document liveDocument = librariesManager.Lifecycle.Publish(document) as Document;                    

                    ProductDocumentFileInfo documentFileInfo = new ProductDocumentFileInfo
                    {
                        FileInfo = fileInfo,
                        Library = libraryToUploadDocumentsTo,
                        Document = liveDocument,
                    };

                    productDocumentFileInfos.Add(documentFileInfo);
                }
                catch
                {
                    //catching so even if one document fails rest succeeds
                }
            }

            librariesManager.SaveChanges();

            return productDocumentFileInfos;
        }

        internal static List<ProductFile> GetProductDocumentsAndFiles(List<ProductDocumentFileInfo> importedDocuments)
        {
            List<ProductFile> productFiles = new List<ProductFile>();
            foreach (var importedDocument in importedDocuments)
            {
                ProductFile productFile = new ProductFile();
                
                productFile.Id = importedDocument.Document.Id;
                productFile.Title = importedDocument.Document.Title;
                productFile.Url = importedDocument.Document.Url;
                productFile.FileName = importedDocument.Document.FilePath;
                productFile.FileSize = importedDocument.Document.TotalSize.ToString();
                productFile.Extension = importedDocument.Document.Extension;
                productFile.UploadedDate = importedDocument.Document.DateCreated.ToShortDateString();
                productFiles.Add(productFile);
            }

            return productFiles;
        }

        internal static List<ProductFile> ImportDocumentsAndGetProductDocuments(ProductImportModel productImportModel, UploadConfig config)
        {
            List<ProductDocumentFileInfo> importedDocuments = DocumentsAndFilesImporter.ImportDocuments(productImportModel.DocumentsAndFilesPath, config);

            List<ProductFile> productDocumentsAndFiles = DocumentsAndFilesImporter.GetProductDocumentsAndFiles(importedDocuments);

            return productDocumentsAndFiles;
        }
    }
}
