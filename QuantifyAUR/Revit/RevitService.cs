using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Office.Interop.Excel;
using QuantifyAUR.Library.Unit;
using QuantifyAUR.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Parameter = Autodesk.Revit.DB.Parameter;

namespace QuantifyAUR.Revit
{
    public class RevitService : BaseViewModel
    {
        private Document _document;
        public RevitService(Document document)
        {
            _document = document;
        }
        public ProjectInformationData GetProjectInformationData()
        {
            ProjectInformationData projectData = new ProjectInformationData();
            Element ProjElem = new FilteredElementCollector(_document).OfClass(typeof(ProjectInfo)).FirstOrDefault();
            if (ProjElem != null)
            {
                Parameter projectNameParam = ProjElem.get_Parameter(BuiltInParameter.PROJECT_NAME);
                Parameter projectNumberParam = ProjElem.get_Parameter(BuiltInParameter.PROJECT_NUMBER);
                Parameter projectAddressParam = ProjElem.get_Parameter(BuiltInParameter.PROJECT_ADDRESS);
                if (projectNameParam != null)
                    projectData.ProjectName = projectNameParam.AsString();

                if (projectNumberParam != null)
                    projectData.ProjectNumber = projectNumberParam.AsString();

                if (projectAddressParam != null)
                    projectData.ProjectAddress = projectAddressParam.AsString();

            }

            return projectData;
        }
        public double CalculateTotalArea()
        {
            double TotalAreaFloors = new FilteredElementCollector(_document).OfClass(typeof(Autodesk.Revit.DB.Floor))
                .ToElements()
                .Select(floor =>
                {
                    Autodesk.Revit.DB.Parameter areaParameter = floor.LookupParameter("Area");
                    if (areaParameter != null && areaParameter.HasValue)
                    {
                        return areaParameter.AsDouble();
                    }
                    return 0.0;
                }).Sum();
            return TotalAreaFloors = ChangeUnit.SquareFeetToSquareMeter(TotalAreaFloors);
        }
        public List<Category> GetCategoriesHasVolume()
        {
            Categories allCategories = _document.Settings.Categories;
            List<Category> categoriesWithVolume = allCategories.Cast<Category>()
                .Where(category =>
                {
                    return !category.CategoryType.Equals(CategoryType.Annotation) &&
                        new FilteredElementCollector(_document)
                            .OfCategoryId(category.Id)
                            .Where(element =>
                                   element.LookupParameter("Volume") != null &&
                                   element.LookupParameter("Alias") != null &&
                                   element.LookupParameter("Unit Quantity") != null).Any();
                })
                .ToList();

            return categoriesWithVolume;
        }
        public List<CategoriesData> PopulateCategoriesObject(List<Category> categoriesWithVolume)
        {
            List<CategoriesData> categoriesData = new List<CategoriesData>();
            foreach (Category cat in categoriesWithVolume)
            {
                CategoriesData data = new CategoriesData { Name = cat.Name, Id = cat.Id, Selected = false };
                categoriesData.Add(data);
            }
            categoriesData = categoriesData.OrderBy(c => c.Name).ToList();

            return categoriesData;
        }




        public void CreateSharedParameterAndBindToCategoriesIfNotExist(string parameterName, ForgeTypeId parameterType, string groupName, List<Category> categories)
        {
            ExternalDefinition existingDefinition = GetExternalDefinitions(parameterName).FirstOrDefault();

            if (existingDefinition != null)
            {
                // Nếu tham số đã tồn tại, thì không cần tạo lại và bạn có thể binding nó với các loại phần tử
                BindSharedParameterToCategories(existingDefinition, categories);
            }
            else
            {
                // Nếu tham số chưa tồn tại, thì tạo tham số mới và sau đó binding nó với các loại phần tử
                using (Transaction transaction = new Transaction(_document, "Create Shared Parameter"))
                {
                    transaction.Start();

                    DefinitionFile sharedParamFile = _document.Application.OpenSharedParameterFile();

                    // Lấy hoặc tạo nhóm (group) trong tệp tham số chia sẻ
                    DefinitionGroup group = sharedParamFile.Groups.Create(groupName);

                    // Tạo đối tượng ExternalDefinitionCreationOptions để định nghĩa tham số
                    ExternalDefinitionCreationOptions options = new ExternalDefinitionCreationOptions(parameterName, parameterType);

                    // Tạo định nghĩa tham số mới trong nhóm và ép kiểu thành ExternalDefinition
                    ExternalDefinition definition = group.Definitions.Create(options) as ExternalDefinition;

                    // Binding tham số với các loại phần tử
                    BindSharedParameterToCategories(definition, categories);

                    transaction.Commit();
                }
            }
        }

        private void BindSharedParameterToCategories(ExternalDefinition definition, List<Category> categories)
        {
            using (Transaction transaction = new Transaction(_document, "Bind Shared Parameter to Categories"))
            {
                transaction.Start();
                // Liên kết định nghĩa tham số với các loại phần tử (categories)
                CategorySet categorySet = new CategorySet();

                // Thêm các loại phần tử vào CategorySet
                foreach (Category cate in categories)
                {
                    categorySet.Insert(cate);
                }

                BindingMap bindingMap = _document.ParameterBindings;

                // Tạo một đối tượng InstanceBinding để liên kết định nghĩa tham số với CategorySet
                InstanceBinding binding = new InstanceBinding(categorySet);

                // Thêm định nghĩa vào CategorySet
                bindingMap.Insert(definition, binding, BuiltInParameterGroup.PG_TEXT);
                transaction.Commit();
            }
        }







        public List<ExternalDefinition> GetExternalDefinitions(string sharedParameterName)
        {
            // Get the shared parameter definition from the document
            DefinitionFile sharedParamFile = _document.Application.OpenSharedParameterFile();

            if (sharedParamFile == null)
            {
                // Handle the case where no shared parameter file is found
                return new List<ExternalDefinition>();
            }

            // Find the shared parameter group by name
            DefinitionGroup group = sharedParamFile.Groups.FirstOrDefault(g => g.Name == sharedParameterName);

            if (group == null)
            {
                // Handle the case where the group with the specified name is not found
                return new List<ExternalDefinition>();
            }

            // Get the external definitions from the group
            List<ExternalDefinition> externalDefinitions = group.Definitions
                .Cast<ExternalDefinition>()
                .ToList();

            return externalDefinitions;
        }

    }
}
