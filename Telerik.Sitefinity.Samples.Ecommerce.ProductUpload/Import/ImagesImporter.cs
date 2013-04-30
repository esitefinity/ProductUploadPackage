using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Telerik.Sitefinity.Data.ContentLinks;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model;
using Telerik.Sitefinity.Ecommerce.Catalog.Model;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Import
{
    internal class ImagesImporter
    {
        internal static List<ProductImageInfo> ImportImages(List<string> imagesPath, UploadConfig config)
        {
            List<ProductImageInfo> productImageInfos = new List<ProductImageInfo>();

            LibrariesManager librariesManager = LibrariesManager.GetManager();

            Album albumToUploadImagesTo = librariesManager.GetAlbum(config.UploadToAlbumId);

            foreach (var imagePath in imagesPath)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(imagePath);
                    if (fileInfo == null)
                    {
                        continue;
                    }


                    //To create an image you have to be logged in as an admin.
                    Telerik.Sitefinity.Libraries.Model.Image image = librariesManager.CreateImage();
                    image.AlternativeText = "Some alt text";

                    var extension = fileInfo.Extension;
                    var imageTitle = fileInfo.Name;
                    if (extension.Length > 0)
                    {
                        imageTitle = imageTitle.Substring(0, imageTitle.Length - extension.Length);
                    }
                    image.Parent = albumToUploadImagesTo;
                    image.Title = imageTitle;
                    image.UrlName = imageTitle.ToLower().Replace(' ', '-');
                    librariesManager.RecompileItemUrls<Telerik.Sitefinity.Libraries.Model.Image>(image);
                    using (var fileStream = fileInfo.OpenRead())
                    {
                        librariesManager.Upload(image, fileStream, fileInfo.Extension);
                    }
                    librariesManager.Lifecycle.Publish(image);

                    ProductImageInfo imageInfo = new ProductImageInfo
                    {
                        ImageInfo = fileInfo,
                        Album = albumToUploadImagesTo,
                        Image = image,
                    };

                    productImageInfos.Add(imageInfo);
                }
                catch
                {
                    //catching so even if one image fails the rest goes on
                }
            }

            librariesManager.SaveChanges();

            return productImageInfos;
        }

        internal static List<ProductImage> GetProductImages(List<ProductImageInfo> importedImages)
        {
            List<ProductImage> productImages = new List<ProductImage>();

            foreach (var importedImage in importedImages)
            {
                ProductImage productImage = new ProductImage();
                productImage.AlbumId = importedImage.Album.Id;
                productImage.Album = importedImage.Album.Title.Value;
                productImage.Id = importedImage.Image.Id;
                productImage.Width = importedImage.Image.Width;
                productImage.Height = importedImage.Image.Height;
                productImage.Title = importedImage.Image.Title;
                productImage.Url = importedImage.Image.Url;
                productImage.AlternativeText = importedImage.Image.AlternativeText;
                productImage.FileName = importedImage.Image.FilePath;
                productImage.FileSize = importedImage.ImageInfo.Length.ToString();

                productImages.Add(productImage);
            }

            return productImages;
        }

        internal static List<ProductImage> ImportImagesAndGetProductImages(ProductImportModel productImportModel, UploadConfig config)
        {
            List<ProductImageInfo> importedImages = ImagesImporter.ImportImages(productImportModel.ImagesPath, config);

            List<ProductImage> productImages = ImagesImporter.GetProductImages(importedImages);

            return productImages;
        }
    }
}
