using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Ecommerce.Catalog.Model;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog;
using Telerik.Sitefinity.Taxonomies.Model;
using System.Text.RegularExpressions;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Import
{
    internal class TagsImporter
    {
        internal static void ImportTags(Product product, List<string> tagsList, UploadConfig config)
        {
            TaxonomyManager taxonomyManager = TaxonomyManager.GetManager();
            CatalogManager catalogManager = CatalogManager.GetManager();

            FlatTaxonomy tagsTaxonomy = taxonomyManager.GetTaxonomies<FlatTaxonomy>().Where(t => t.Name == "Tags").SingleOrDefault();

            foreach (var tagString in tagsList)
            {
                Taxon tag = GetTagIfItExsistsOrCreateOneIfItDoesnt(tagString, tagsTaxonomy, taxonomyManager);

                SetTagProperties(tag, tagString);

                taxonomyManager.SaveChanges();

                if (product.Organizer.TaxonExists("Tags", tag.Id) == true)
                {
                    continue;        // Product already linked to Tag
                }

                product.Organizer.AddTaxa("Tags", tag.Id);

                catalogManager.SaveChanges();

            }
        }

        private static Taxon GetTagIfItExsistsOrCreateOneIfItDoesnt(string tagName, FlatTaxonomy tags, TaxonomyManager taxonomyManager)
        {
            Taxon tag = tags.Taxa.Where(t => t.Title.ToLower() == tagName.ToLower()).FirstOrDefault();

            if (tag == null)
            {
                tag = taxonomyManager.CreateTaxon<FlatTaxon>();

                SetTagProperties(tag, tagName);

                tag.Taxonomy = tags;

                taxonomyManager.SaveChanges();
            }

            return tag;
        }


        private static void SetTagProperties(Taxon tag, string tagName)
        {
            tag.Name = Regex.Replace(tagName.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
            tag.Title = tagName;

            tag.Description = tagName;
            tag.UrlName = tag.Name;
        }
    }
}
