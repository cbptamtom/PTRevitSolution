using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using QuantifyAUR.Library.Filter;
using QuantifyAUR.Library.Orther;
using QuantifyAUR.Library.Unit;
using QuantifyAUR.Revit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuantifyAUR.Command
{
    [Transaction(TransactionMode.Manual)]
    public class QuantifyCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            RevitService revitSv = new RevitService(doc);
            BuiltInCategory[] categoriesToInclude = new BuiltInCategory[]
            {
                BuiltInCategory.OST_StructuralFraming,
                BuiltInCategory.OST_StructuralColumns,
                BuiltInCategory.OST_Floors,
                BuiltInCategory.OST_StructuralFoundation,
                BuiltInCategory.OST_Walls
            };

            List<Category> cates = categoriesToInclude
                .Select(bic => Category.GetCategory(doc, bic))
                .Where(category => category != null)
                .ToList();

            List<Element> elementsToInclude = categoriesToInclude
                .SelectMany(bic => new FilteredElementCollector(doc)
                .OfCategory(bic)
                .WhereElementIsNotElementType())
                .ToList();

            if (elementsToInclude.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Model có con mẹ gì đâu mà run.");
                return Result.Cancelled;
            }

            Element e = elementsToInclude.FirstOrDefault();

            bool hasAlias = false;
            bool hasUnitQuantity = false;

            if (e != null)
            {
                ParameterSet pSet = e.Parameters;
                List<Parameter> parameters = pSet.Cast<Parameter>().ToList();
                hasAlias = parameters.Any(p => p.Definition.Name == "Alias");
                hasUnitQuantity = parameters.Any(p => p.Definition.Name == "Unit Quantity");
            }

            bool showQuantifyWindow = hasAlias && hasUnitQuantity;

            using (TransactionGroup transGr = new TransactionGroup(doc))
            {
                transGr.Start("RAPI00TransGr");



                if (!hasAlias || !hasUnitQuantity)
                {
                    try
                    {
                        if (!hasAlias)
                        {
                            revitSv.CreateSharedParameterAndBindToCategoriesIfNotExist("Alias", SpecTypeId.String.Text, "Alias", cates);

                        }

                        if (!hasUnitQuantity)
                        {
                            revitSv.CreateSharedParameterAndBindToCategoriesIfNotExist("Unit Quantity", SpecTypeId.String.Text, "Unit Quantity", cates);
                        }

                        message = "Đã tạo shared parameters. Chạy lại phần mềm để tiếp tục tính toán.";
                        TaskDialog.Show("Thông báo", message);
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        System.Windows.Forms.MessageBox.Show(message);
                        return Result.Failed;
                    }
                }
                bool check = e.LookupParameter("Alias").AsString() == null || e.LookupParameter("Unit Quantity").AsString() == null;
                if (check)
                {
                    TaskDialog.Show("Thông báo", "Cap nhat gia trị cho Alias, Unit Quantity");
                }
                if (showQuantifyWindow && e.LookupParameter("Alias").AsString() != null && e.LookupParameter("Unit Quantity").AsString() != null)
                {
                    QuantifyViewModel viewModel = new QuantifyViewModel(uidoc, doc);
                    QuantifyWindow window = new QuantifyWindow(viewModel);
                    window.ShowDialog();
                }
                transGr.Assimilate();
            }

            return Result.Succeeded;
        }
    }
}
