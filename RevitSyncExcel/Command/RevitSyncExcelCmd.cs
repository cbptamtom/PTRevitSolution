
#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitSyncExcel.Library.Filter;
using RevitSyncExcel.Library.Orther;
using RevitSyncExcel.Library.Unit;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion


namespace RevitSyncExcel.Command
{
    [Transaction(TransactionMode.Manual)]
    public class RevitSyncExcelCmd : IExternalCommand
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
