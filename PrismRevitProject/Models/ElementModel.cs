using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismRevitProject.Models
{
    public class ElementModel
    {
        public ObservableCollection<ElementInfo> ElementInfo { get; set; }
        public List<ElementGroup> ElementGroups { get; set; }

        public ElementModel()
        {
            ElementInfo = new ObservableCollection<ElementInfo>();
            var elementsWithAliasAndUnitQuantity = DocumentService.GetElementsWithAliasAndUnitQuantity();

            foreach (var element in elementsWithAliasAndUnitQuantity)
            {
                Parameter aliasParameter = element.GetParameters("Alias").FirstOrDefault();
                string aliasValue = aliasParameter != null ? aliasParameter.AsString() : "";
                Parameter unitQuantityParameter = element.GetParameters("Unit Quantity").FirstOrDefault();
                string unitQuantityValue = unitQuantityParameter != null ? unitQuantityParameter.AsString() : "";

                ElementInfo.Add(new ElementInfo
                {
                    Category = element.Category.Name,
                    ElementName = element.Name,
                    AliasValue = aliasValue,
                    UnitQuantityValue = unitQuantityValue
                });
            }
            var groupedElements = ElementInfo.GroupBy(e => e.Category);

            ElementGroups = groupedElements.Select(g => new ElementGroup
            {
                Category = g.Key,
                Elements = g.ToList()
            }).ToList();
        }

        public class ElementGroup
        {
            public string Category { get; set; }
            public List<ElementInfo> Elements { get; set; }
        }



    }
}
