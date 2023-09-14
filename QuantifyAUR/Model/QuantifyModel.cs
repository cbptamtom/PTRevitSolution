using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuantifyAUR.Model
{
    public class QuantifyModel : BaseViewModel
    {

        private List<Category> _categoriesWithVolume;
        public List<Category> CategoriesWithVolume { get { return _categoriesWithVolume; } set { _categoriesWithVolume = value; OnPropertyChanged(); } }


        private List<CategoriesData> _categoriesData;
        public List<CategoriesData> CategoriesData { get { return _categoriesData; } set { _categoriesData = value; OnPropertyChanged(nameof(CategoriesData)); } }

        private List<Element> _elements;
        public List<Element> Elements { get { return _elements; } set { _elements = value; OnPropertyChanged(); } }

        private List<string> _alias;
        public List<string> AliasValues { get { return _alias; } set { _alias = value; OnPropertyChanged(); } }

        private List<string> _unitQuantity;
        public List<string> UnitQuantity { get { return _unitQuantity; } set { _unitQuantity = value; OnPropertyChanged(); } }

        private List<Tuple<Element, string, string>> _combinedData;
        public List<Tuple<Element, string, string>> CombinedData { get { return _combinedData; } set { _combinedData = value; OnPropertyChanged(); } }

        public Document Doc;
        public QuantifyModel(Document doc)
        {
            Doc = doc;
            GetCategoriesHasVolume(Doc);
            PopulateCategoriesObject(CategoriesWithVolume);
        }



        private void PopulateCategoriesObject(List<Category> categoriesWithVolume)
        {
            CategoriesData = new List<CategoriesData>();
            foreach (Category cat in categoriesWithVolume)
            {
                CategoriesData data = new CategoriesData { Name = cat.Name, Id = cat.Id, Selected = false };
                CategoriesData.Add(data);
            }
            CategoriesData = CategoriesData.OrderBy(c => c.Name).ToList();
        }

        private void GetCategoriesHasVolume(Document doc)
        {
            Categories allCategories = doc.Settings.Categories;
            CategoriesWithVolume = allCategories.Cast<Category>()
                .Where(category =>
                {
                    return !category.CategoryType.Equals(CategoryType.Annotation) &&
                        new FilteredElementCollector(doc)
                            .OfCategoryId(category.Id)
                             .Where(element =>
                                    element.LookupParameter("Volume") != null &&
                                    element.LookupParameter("Alias") != null &&
                                    element.LookupParameter("Unit Quantity") != null).Any();
                }).ToList();
        }

        public List<Element> GetSelectedElements()
        {
            Elements = new List<Element>();

            foreach (CategoriesData categoryData in CategoriesData)
            {
                if (categoryData.Selected)
                {
                    List<Element> elements = new FilteredElementCollector(Doc)
                        .OfCategoryId(categoryData.Id)
                        .WhereElementIsNotElementType()
                        .ToList();
                    Elements.AddRange(elements);
                }
            }
            return Elements;
        }

        public void GetAliasValues()
        {
            AliasValues = new List<string>();
            foreach (Element elem in Elements)
            {
                Parameter aliasParam = elem.LookupParameter("Alias");
                if (aliasParam != null)
                {
                    string alias = aliasParam.AsString();
                    AliasValues.Add(alias);
                }
            }
        }

        public void GetUnitQuantityValues()
        {
            UnitQuantity = new List<string>(); // Sử dụng danh sách kiểu double để lưu giá trị số
            foreach (Element elem in Elements)
            {
                Parameter unitQuantityParam = elem.LookupParameter("Unit Quantity");
                if (unitQuantityParam != null)
                {
                    string unitQuantityValue = unitQuantityParam.AsString();
                    UnitQuantity.Add(unitQuantityValue);
                }
            }
        }

        public void CombineData(List<Element> elements, List<string> aliasValues, List<string> unitQuantity)
        {
            CombinedData = new List<Tuple<Element, string, string>>();

            for (int i = 0; i < elements.Count; i++)
            {
                Element element = elements[i];
                string[] aliases = aliasValues[i].Split(';');
                string[] unitQuantities = unitQuantity[i].Split(';');

                int minLength = Math.Min(aliases.Length, unitQuantities.Length);

                for (int j = 0; j < minLength; j++)
                {
                    CombinedData.Add(new Tuple<Element, string, string>(element, aliases[j], unitQuantities[j]));
                }
            }

        }


    }

}
