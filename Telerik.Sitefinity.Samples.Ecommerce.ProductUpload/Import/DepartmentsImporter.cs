using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.Sitefinity.Ecommerce.Catalog.Model;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;

namespace Telerik.Sitefinity.Samples.Ecommerce.ProductUpload.Import
{
    internal class DepartmentsImporter
    {
        internal static void ImportDepartments(Product product, List<string> departmentList, UploadConfig config)
        {
            TaxonomyManager taxonomyManager = TaxonomyManager.GetManager();
            CatalogManager catalogManager = CatalogManager.GetManager();

            HierarchicalTaxonomy departmentTaxonomy = taxonomyManager.GetTaxonomies<HierarchicalTaxonomy>().Where(t => t.Name == "Departments").SingleOrDefault();

            foreach (var departmentString in departmentList)
            {
                Taxon department = GetDepartmentIfItExsistsOrCreateOneIfItDoesnt(departmentString, departmentTaxonomy, taxonomyManager);
                
                SetDepartmentProperties(department, departmentString);

                taxonomyManager.SaveChanges();

                if (product.Organizer.TaxonExists("Department", department.Id) == true)
                {
                    continue;        // Product already linked to department
                }

                product.Organizer.AddTaxa("Department", department.Id);

                catalogManager.SaveChanges();

            }
        }
  
        private static Taxon GetDepartmentIfItExsistsOrCreateOneIfItDoesnt(string departmentName, HierarchicalTaxonomy departments, TaxonomyManager taxonomyManager)
        {
            Taxon department = departments.Taxa.Where(t => t.Title.ToLower() == departmentName.ToLower()).FirstOrDefault();

            if (department == null)
            {
                department = taxonomyManager.CreateTaxon<HierarchicalTaxon>();
                
                SetDepartmentProperties(department, departmentName);

                department.Taxonomy = departments;

                taxonomyManager.SaveChanges();
            }

            return department;
        }
  
        
        private static void SetDepartmentProperties(Taxon department, string departmentName)
        {
            department.Name = Regex.Replace(departmentName.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
            department.Title = departmentName;

            department.Description = departmentName;
            department.UrlName = department.Name;
        }
    }
}
