using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace MyRevitPrismApp.Services
{
    public class RevitService
    {
        private Document document; // Document của dự án Revit

        public RevitService(Document doc)
        {
            document = doc;
        }

        public List<Element> GetStructuralColumnsFromRevit()
        {
            List<Element> columns = new List<Element>();

            // Sử dụng FilteredElementCollector để lấy danh sách các phần tử Structural Columns
            FilteredElementCollector collector = new FilteredElementCollector(document);
            collector.OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_StructuralColumns); // Lọc theo BuiltInCategory cho Structural Columns

            foreach (Element column in collector)
            {
                columns.Add(column);
            }

            return columns;
        }

        public List<Element> GetStructuralFramingFromRevit()
        {
            List<Element> framing = new List<Element>();

            // Sử dụng FilteredElementCollector để lấy danh sách các phần tử Structural Framing
            FilteredElementCollector collector = new FilteredElementCollector(document);
            collector.OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_StructuralFraming); // Lọc theo BuiltInCategory cho Structural Framing

            foreach (Element framingElement in collector)
            {
                framing.Add(framingElement);
            }

            return framing;
        }
    }
}
