using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using QuantifyAUR.Library.Unit;
using QuantifyAUR.Revit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static QuantifyAUR.Excel.ExcelService;

namespace QuantifyAUR.Model
{
    public class QuantifyModel : BaseViewModel
    {
        private RevitService _revitService;
        public RevitService RevitService { get { return _revitService; } set { _revitService = value; OnPropertyChanged(); } }
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
        private List<Tuple<Element, string, double>> _combinedData;
        public List<Tuple<Element, string, double>> CombinedData { get { return _combinedData; } set { _combinedData = value; OnPropertyChanged(); } }
        private Dictionary<string, double> _sumByAliasesDictionary;
        public Dictionary<string, double> SumByAliasesDictionary { get { return _sumByAliasesDictionary; } set { _sumByAliasesDictionary = value; OnPropertyChanged(); } }
        private double _totalAreaFloors;

        public double TotalAreaFloors { get { return _totalAreaFloors; } set { _totalAreaFloors = value; OnPropertyChanged(); } }
        public string TotalAreaFloorsString
        {
            get { return $"{Math.Round(_totalAreaFloors, 2)} m2"; }
        }
        private string _revitName;
        public string RevitName { get { return _revitName; } set { _revitName = value; OnPropertyChanged(); } }

        private List<Element> _InstanceElemInProject;
        public List<Element> InstanceElemInProject { get { return _InstanceElemInProject; } set { _InstanceElemInProject = value; OnPropertyChanged(); } }

        private List<string> _InstanceElemNameInProject;
        public List<string> InstanceElemNameInProject { get { return _InstanceElemNameInProject; } set { _InstanceElemNameInProject = value; OnPropertyChanged(); } }



        public Document Doc;
        public QuantifyModel(Document doc)
        {
            Doc = doc;
            RevitService = new RevitService(Doc);
            TotalAreaFloors = RevitService.CalculateTotalArea();
            CategoriesWithVolume = RevitService.GetCategoriesHasVolume();
            CategoriesData = RevitService.PopulateCategoriesObject(CategoriesWithVolume);
            RevitName = RevitService.GetProjectInformationData().ProjectName;
            InstanceElemInProject = RevitService.GetElementsByCategories(RevitService.categoriesToInclude.ToList());

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
            UnitQuantity = new List<string>();
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
            CombinedData = new List<Tuple<Element, string, double>>();
            for (int i = 0; i < elements.Count; i++)
            {
                Element element = elements[i];
                char[] separators = new char[] { ';', '/' };
                string[] aliases = aliasValues[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string[] unitQuantities = unitQuantity[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                int minLength = Math.Min(aliases.Length, unitQuantities.Length);
                double volume = GetVolumeForElement(element);
                for (int j = 0; j < minLength; j++)
                {
                    if (double.TryParse(unitQuantities[j], out double unitQtyValue))
                    {
                        unitQtyValue *= volume;
                        CombinedData.Add(new Tuple<Element, string, double>(element, aliases[j], unitQtyValue));
                    }
                    else
                    {
                        MessageBox.Show($"Wrong Unit Quantity for {element.Name}, value: {unitQuantities[j]}");
                        return;
                    }
                }
            }
        }

        public Dictionary<string, double> SumUnitQtyValuesByAliases(List<Tuple<Element, string, double>> combinedData)
        {
            SumByAliasesDictionary = new Dictionary<string, double>();

            foreach (var data in combinedData)
            {
                string alias = data.Item2;
                double unitQtyValue = data.Item3;

                if (SumByAliasesDictionary.ContainsKey(alias))
                {
                    // If the alias already exists in the Dictionary, add the new value to the existing value.
                    SumByAliasesDictionary[alias] += unitQtyValue;
                }
                else
                {
                    // If the alias does not exist in the Dictionary, add a new alias with the current value.
                    SumByAliasesDictionary[alias] = unitQtyValue;
                }
            }
            return SumByAliasesDictionary;
        }
        private double GetVolumeForElement(Element element)
        {
            Parameter volumeParam = element.LookupParameter("Volume");
            if (volumeParam != null && volumeParam.HasValue)
            {
                return ChangeUnit.CubicFeetToCubicMeter(volumeParam.AsDouble());
            }
            else
            {
                return 0.0;
            }
        }


    }

}
