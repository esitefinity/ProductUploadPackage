using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.Sitefinity.Ecommerce.Catalog.Model;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog;
using Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Model;
using Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.ErrorHandling;
using System.Web.Script.Serialization;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Import
{
    internal class ProductImporter
    {
        /// <summary>
        /// Imports list of <see cref="ProductImportModel"/> to database
        /// </summary>
        /// <param name="dataToInsertInDatabase"></param>
        /// <returns></returns>
        internal static ImportStatistic SaveProducts(List<ProductImportModel> data, UploadConfig config)
        {
            CatalogManager catalogManager = CatalogManager.GetManager();

            List<ImportError> importErrors = new List<ImportError>();

            int numberOfRecordsProcessed = 0;
            int numberOfSuccessfulRecords = 0;
            int numberOfFailedRecords = 0;

            foreach (ProductImportModel productImportModel in data)
            {

                try
                {
                    numberOfRecordsProcessed++;

                    ProductType productType = catalogManager.GetProductTypes().Where(pt => pt.Title == productImportModel.ProductTypeTitle).FirstOrDefault();
                    if (productType != null)
                    {
                        Product product = catalogManager.CreateProduct(productType.ClrType);

                        product.ApplicationName = "/Catalog";
                        product.Title = productImportModel.Title;
                        if (string.IsNullOrWhiteSpace(productImportModel.Url))
                        {
                            product.UrlName = Regex.Replace(productImportModel.Title.ToLower(), @"\s+", "-");
                        }
                        else
                        {
                            product.UrlName = productImportModel.Url;
                        }

                        product.Description = productImportModel.Description;
                        product.Price = productImportModel.Price;
                        product.Weight = Convert.ToDouble(productImportModel.Weight);
                        product.Sku = productImportModel.Sku;
                        product.IsActive = productImportModel.IsActive;

                        catalogManager.RecompileItemUrls<Product>(product);

                        var props = TypeDescriptor.GetProperties(product);
                        foreach (var singleProp in props)
                        {
                            if (singleProp.GetType() == typeof(MetafieldPropertyDescriptor))
                            {
                                MetafieldPropertyDescriptor customField = singleProp as MetafieldPropertyDescriptor;
                                if (customField != null)
                                {
                                    foreach (var customFieldData in productImportModel.CustomFieldData)
                                    {
                                        if (customField.Name == customFieldData.PropertyName)
                                        {
                                            customField.SetValue(product, customFieldData.PropertyValue);
                                        }
                                    }
                                }
                            }
                        }
                        catalogManager.SaveChanges();
                        
                        List<ProductImage> productImages = ImagesImporter.ImportImagesAndGetProductImages(productImportModel, config);
                        product.Images.AddRange(productImages);

                        catalogManager.SaveChanges();

                        ContentLinkGenerator.GenerateContentLinksForProductImages(product);

                        List<ProductFile> productFiles = DocumentsAndFilesImporter.ImportDocumentsAndGetProductDocuments(productImportModel, config);
                        product.DocumentsAndFiles.AddRange(productFiles);

                        catalogManager.SaveChanges();

                        ContentLinkGenerator.GenerateContentLinksForProductDocuments(product);

                        DepartmentsImporter.ImportDepartments(product, productImportModel.Departments, config);

                        TagsImporter.ImportTags(product, productImportModel.Tags, config);

                        numberOfSuccessfulRecords++;
                    }
                    else
                    {
                        throw new ArgumentException("Cannot find product type " + productImportModel.ProductTypeTitle);
                    }

                }
                catch (Exception ex)
                {
                    numberOfFailedRecords++;

                    importErrors.Add(new ImportError { ErrorMessage = ex.Message, ErrorRow = productImportModel.CorrespondingRowData });

                    continue;
                }

            }
            ImportStatistic statisticsOfImport = new ImportStatistic
                                                        {
                                                            TotalNumberOfRecordsProcessed = numberOfRecordsProcessed,
                                                            NumberOfSuccessfulRecords = numberOfSuccessfulRecords,
                                                            NumberOfFailedRecords = numberOfFailedRecords,
                                                            Errors = importErrors
                                                        };
            return statisticsOfImport;
        }

        internal static ImportStatistic SaveProductVariations(List<ProductVariationImportModel> data, UploadConfig config)
        {
            CatalogManager catalogManager = CatalogManager.GetManager();

            List<ImportError> importErrors = new List<ImportError>();

            int numberOfRecordsProcessed = 0;
            int numberOfSuccessfulRecords = 0;
            int numberOfFailedRecords = 0;

            foreach (ProductVariationImportModel productVariationImportModel in data)
            {

                try
                {
                    numberOfRecordsProcessed++;

                    Product parentProduct = catalogManager.GetProduct(productVariationImportModel.ProductNameSku);
                    if (parentProduct != null)
                    {
                        ProductAttribute productAttribute = catalogManager.GetProductAttributeByName(productVariationImportModel.AttributeName);
                        if (productAttribute != null)
                        {
                            ProductAttributeValue attributeValue = catalogManager.GetProductAttributeValues().Where(pav => pav.Title == productVariationImportModel.ValueName).FirstOrDefault();
                            if (attributeValue != null)
                            {
                                ProductVariation productVariation = catalogManager.CreateProductVariation();

                                //Set the properties
                                productVariation.AdditionalPrice = productVariationImportModel.AdditionalPrice;
                                productVariation.Parent = parentProduct;
                                productVariation.Sku = productVariationImportModel.Sku;
                                productVariation.IsActive = productVariationImportModel.IsActive;

                                //Set the Variant property
                                AttributeValuePair attributeValuePair = new AttributeValuePair();
                                attributeValuePair.AttributeValueId = attributeValue.Id;
                                attributeValuePair.AttributeId = attributeValue.Parent.Id;

                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                string attributeValuePairJson = serializer.Serialize(attributeValuePair);

                                productVariation.Variant = attributeValuePairJson;

                                //Create the product variation detail
                                ProductVariationDetail detail = catalogManager.CreateProductVariationDetail();
                                detail.ProductAttributeParent = attributeValue.Parent;
                                detail.ProductAttributeValueParent = attributeValue;
                                detail.ProductVariationParent = productVariation;
                                detail.ProductVariationDetailParentId = Guid.Empty;

                                parentProduct.ProductVariations.Add(productVariation);

                                catalogManager.SaveChanges();


                                numberOfSuccessfulRecords++;
                            }
                            else
                            {
                                throw new ArgumentException("Cannot find attribute value with title  " + productVariationImportModel.ValueName);
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Cannot find attribute with title  " + productVariationImportModel.AttributeName);
                        }
                        
                        
                    }
                    else
                    {
                        throw new ArgumentException("Cannot find product with Sku " + productVariationImportModel.ProductNameSku);
                    }

                }
                catch (Exception ex)
                {
                    numberOfFailedRecords++;

                    importErrors.Add(new ImportError { ErrorMessage = ex.Message, ErrorRow = productVariationImportModel.CorrespondingRowData });

                    continue;
                }

            }
            ImportStatistic statisticsOfImport = new ImportStatistic
            {
                TotalNumberOfRecordsProcessed = numberOfRecordsProcessed,
                NumberOfSuccessfulRecords = numberOfSuccessfulRecords,
                NumberOfFailedRecords = numberOfFailedRecords,
                Errors = importErrors
            };
            return statisticsOfImport;
        }
  
        

        

    }
}
