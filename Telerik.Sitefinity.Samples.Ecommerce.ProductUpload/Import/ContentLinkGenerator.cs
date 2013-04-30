using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Data.ContentLinks;
using Telerik.Sitefinity.Model.ContentLinks;
using Telerik.Sitefinity.Ecommerce.Catalog.Model;
using Telerik.Sitefinity.Modules.Libraries;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Import
{
    internal class ContentLinkGenerator
    {
        internal static void GenerateContentLinksForProductImages(Product product)
        {
            ContentLinksManager contentLinksManager = ContentLinksManager.GetManager();

            LibrariesManager librariesManager = LibrariesManager.GetManager();

            IEnumerable<ContentLink> contentLinks = contentLinksManager.GetContentLinks().Where(cl => cl.ParentItemId == product.Id && cl.ComponentPropertyName == "ProductImage").ToList();

            IEnumerable<Guid> persistedIds = contentLinks.Select(cl => cl.ChildItemId);
            List<ProductImage> imagesToAdd = product.Images.Where(i => !persistedIds.Contains(i.Id)).ToList();
            
            var createdContentLinks = new List<ContentLink>();
            int ordinal = 0;

            foreach (ProductImage productImageToAdd in imagesToAdd)
            {
                Telerik.Sitefinity.Libraries.Model.Image temporaryImage = librariesManager.GetImage(productImageToAdd.Id);

                ContentLink contentLink = contentLinksManager.CreateContentLink("ProductImage", product, temporaryImage);
                contentLink.Ordinal = ordinal;
                ordinal++;
                createdContentLinks.Add(contentLink);
            }

            contentLinksManager.SaveChanges();

        }

        internal static void GenerateContentLinksForProductDocuments(Product product)
        {
            ContentLinksManager contentLinksManager = ContentLinksManager.GetManager();

            LibrariesManager librariesManager = LibrariesManager.GetManager();

            IEnumerable<ContentLink> contentLinks = contentLinksManager.GetContentLinks().Where(cl => cl.ParentItemId == product.Id && cl.ComponentPropertyName == "ProductDocumentsAndFiles").ToList();

            IEnumerable<Guid> persistedIds = contentLinks.Select(cl => cl.ChildItemId);
            List<ProductFile> documentsToAdd = product.DocumentsAndFiles.Where(i => !persistedIds.Contains(i.Id)).ToList();
            var createdContentLinks = new List<ContentLink>();
            int ordinal = 0;

            foreach (ProductFile documentToAdd in documentsToAdd)
            {
                Telerik.Sitefinity.Libraries.Model.Document temporaryDocument = librariesManager.GetDocument(documentToAdd.Id);

                ContentLink contentLink = contentLinksManager.CreateContentLink("ProductDocumentsAndFiles", product, temporaryDocument);
                contentLink.Ordinal = ordinal;
                ordinal++;

                createdContentLinks.Add(contentLink);
            }

            contentLinksManager.SaveChanges();

        }
    }
}
