using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using PrismRevitProject.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Unity.Injection;
using Xamarin.Forms;

public static class DocumentService
{
    private static Autodesk.Revit.DB.Document _document;
    private static string _projectName;
    private static string _projectNumber;
    private static string _projectAddress;


    public static Autodesk.Revit.DB.Document GetDocument()
    {
        return _document;
    }

    public static void SetDocument(Autodesk.Revit.DB.Document document)
    {
        _document = document;
        if (_document != null)
        {
            Autodesk.Revit.DB.ProjectInfo projectInfo = _document.ProjectInformation;
            _projectName = projectInfo.Name;
            _projectNumber = projectInfo.Number;
            _projectAddress = projectInfo.Address;
        }
    }

    public static string GetProjectName()
    {
        return _projectName ?? "No document or project information available.";
    }

    public static string GetProjectNumber()
    {
        return _projectNumber ?? "No document or project information available.";
    }

    public static string GetProjectAddress()
    {
        return _projectAddress ?? "No document or project information available.";
    }


    public static bool CheckAllElementsHaveAliasAndUnitQuantity()
    {
        List<BuiltInCategory> categoriesToCheck = new List<BuiltInCategory>
        {
        BuiltInCategory.OST_Floors,
        BuiltInCategory.OST_StructuralFraming,
        BuiltInCategory.OST_StructuralColumns,
        BuiltInCategory.OST_Walls,
        BuiltInCategory.OST_StructuralFoundation
        };

        return categoriesToCheck.All(category =>
                    new FilteredElementCollector(_document)
                    .OfCategory(category).WhereElementIsNotElementType()
                    .Cast<Autodesk.Revit.DB.Element>()
        .All(e => e.Parameters.Cast<Parameter>().Any(p => p.Definition.Name == "Alias") &&
                                    e.Parameters.Cast<Parameter>().Any(p => p.Definition.Name == "Unit Quantity"))
                                            );
    }
    public static List<Autodesk.Revit.DB.Element> GetElementsWithAliasAndUnitQuantity()
    {
        List<BuiltInCategory> categoriesToCheck = new List<BuiltInCategory>
                                                {
                                                BuiltInCategory.OST_Floors,
                                                BuiltInCategory.OST_StructuralFraming,
                                                BuiltInCategory.OST_StructuralColumns,
                                                BuiltInCategory.OST_Walls,
                                                BuiltInCategory.OST_StructuralFoundation
                                                };

        List<Autodesk.Revit.DB.Element> elementsWithAliasAndUnitQuantity = new List<Autodesk.Revit.DB.Element>();

        foreach (BuiltInCategory category in categoriesToCheck)
        {
            // Tạo một FilteredElementCollector để lấy tất cả phần tử trong category và view
            FilteredElementCollector collector = new FilteredElementCollector(_document);
            collector.OfCategory(category).WhereElementIsNotElementType();

            // Sử dụng LINQ để lọc các phần tử có Alias và Unit Quantity
            var filteredElements = collector.Where(element =>
                                                                        {
                                                                            var aliasParameter = element.LookupParameter("Alias");
                                                                            var unitQuantityParameter = element.LookupParameter("Unit Quantity");

                                                                            return aliasParameter != null && unitQuantityParameter != null;
                                                                        }).ToList();

            elementsWithAliasAndUnitQuantity.AddRange(filteredElements);
        }

        return elementsWithAliasAndUnitQuantity;
    }

    public static void SetParameterFromElementInfo(Document document, ObservableCollection<ElementInfo> elementInfoList)
    {
        using (Transaction transaction = new Transaction(document, "Map ElementInfo Data to Elements"))
        {
            transaction.Start();

            List<Autodesk.Revit.DB.Element> allElements = new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .ToList();

            foreach (ElementInfo elementInfo in elementInfoList)
            {
                foreach (Autodesk.Revit.DB.Element element in allElements)
                {
                    if (element.Name == elementInfo.ElementName)
                    {
                        Parameter aliasParameter = element.LookupParameter("Alias");
                        Parameter unitQuantityParameter = element.LookupParameter("Unit Quantity");

                        if (aliasParameter != null && unitQuantityParameter != null)
                        {
                            aliasParameter.Set(elementInfo.AliasValue);
                            unitQuantityParameter.Set(elementInfo.UnitQuantityValue);
                        }
                    }
                }
            }

            transaction.Commit();
        }
    }

    public static DefinitionGroup CreateSharedParameterGroup(Document document, string groupName)
    {
        using (Transaction transaction = new Transaction(document, "Create Shared Parameter Group"))
        {
            transaction.Start();
            DefinitionFile sharedParamFile = _document.Application.OpenSharedParameterFile();
            DefinitionGroups groups = sharedParamFile.Groups;
            foreach (var item in groups)
            {
                if (item.Name == groupName)
                {
                    transaction.RollBack();
                    System.Windows.Forms.MessageBox.Show("Parameter group already exists.");
                    return null;
                }
            }
            DefinitionGroup group = sharedParamFile.Groups.Create(groupName);
            transaction.Commit();
            return group;
        }
    }
    public static ExternalDefinition AddExternalSharedParameterToGroup(Document document, DefinitionGroup group, string parameterName, ForgeTypeId parameterType)
    {
        using (Transaction transaction = new Transaction(document, "Create Shared Parameter Group"))
        {
            transaction.Start();
            ExternalDefinitionCreationOptions options = new ExternalDefinitionCreationOptions(parameterName, parameterType);
            ExternalDefinition definition = group.Definitions.Create(options) as ExternalDefinition;
            transaction.Commit();
            return definition;
        }
    }
    public static void BindSharedParameterToCategories(Document document, ExternalDefinition definition, List<Category> categories)
    {
        CategorySet categorySet = new CategorySet();
        foreach (Category cate in categories)
        {
            categorySet.Insert(cate);
        }
        BindingMap bindingMap = _document.ParameterBindings;
        using (Transaction transaction = new Transaction(document, "Create Shared Parameter Group"))
        {
            transaction.Start();
            InstanceBinding binding = new InstanceBinding(categorySet);
            bindingMap.Insert(definition, binding, BuiltInParameterGroup.PG_TEXT);
            transaction.Commit();
        }
    }
    public static List<Category> GetCategoriesByBuiltInCategories(BuiltInCategory[] categoriesToInclude)
    {
        return categoriesToInclude
            .Select(bic => Category.GetCategory(_document, bic))
            .Where(category => category != null)
            .ToList();
    }




}
