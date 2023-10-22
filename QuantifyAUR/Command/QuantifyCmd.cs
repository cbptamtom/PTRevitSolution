using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using QuantifyAUR.Library.Filter;
using QuantifyAUR.Library.Orther;
using QuantifyAUR.Library.Unit;
using QuantifyAUR.Revit;
using QuantifyAUR.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace QuantifyAUR
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


            List<Category> cates = new List<Category>();
            List<Element> elementsToInclude = new List<Element>();
            RevitService revitSv = new RevitService(doc);

            BuiltInCategory[] categoriesToInclude = new BuiltInCategory[]
            {
                BuiltInCategory.OST_StructuralFraming,
                BuiltInCategory.OST_StructuralColumns,
                BuiltInCategory.OST_Floors,
                BuiltInCategory.OST_StructuralFoundation,
                BuiltInCategory.OST_Walls
            };

            cates = revitSv.GetCategoriesByBuiltInCategories(categoriesToInclude);
            elementsToInclude = revitSv.GetElementsByCategories(categoriesToInclude.ToList());

            if (elementsToInclude.Count == 0)
            {
                MessageBox.Show("There are no elements to work with.");
                return Result.Cancelled;
            }
            Element fiste = elementsToInclude.FirstOrDefault();

            bool hasAlias = false;
            bool hasUnitQuantity = false;

            if (fiste != null)
            {
                ParameterSet pSet = fiste.Parameters;
                List<Parameter> parameters = pSet.Cast<Parameter>().ToList();
                hasAlias = parameters.Any(p => p.Definition.Name == "Alias");
                hasUnitQuantity = parameters.Any(p => p.Definition.Name == "Unit Quantity");
            }

            bool showQuantifyWindow = hasAlias && hasUnitQuantity;

            using (TransactionGroup transGr = new TransactionGroup(doc))
            {
                transGr.Start("Calculation");
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

                        message = "Parameters have been created";
                        TaskDialog.Show("Thông báo", message);

                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        System.Windows.Forms.MessageBox.Show(message);
                        return Result.Failed;
                    }
                }

                else
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
