
#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IFCExporter.Library.Filter;
using IFCExporter.Library.Orther;
using IFCExporter.Library.Unit;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion


namespace IFCExporter.Command
{
    [Transaction(TransactionMode.Manual)]
    public class ExamCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // code



            using (TransactionGroup transGr = new TransactionGroup(doc))
            {
                transGr.Start("RAPI00TransGr");

                RAPI00ViewModel viewModel = new RAPI00ViewModel(uidoc, doc);
                APIWindow window = new APIWindow(viewModel);
                if (window.ShowDialog() == false) return Result.Cancelled;

                transGr.Assimilate();
            }

            return Result.Succeeded;
        }
    }
}
