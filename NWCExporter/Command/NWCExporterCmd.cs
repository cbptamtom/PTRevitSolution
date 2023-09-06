
#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NWCExporter.Library.Filter;
using NWCExporter.Library.Orther;
using NWCExporter.Library.Unit;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms.Automation;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion


namespace NWCExporter
{
    [Transaction(TransactionMode.Manual)]
    public class NWCExporterCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // code

            //System.Windows.Forms.MessageBox.Show("Test");

            if (doc == null || string.IsNullOrEmpty(doc.PathName))
            {
                MessageBox.Show("Project must be saved", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return Result.Cancelled;
            }

            if (OptionalFunctionalityUtils.IsNavisworksExporterAvailable() == false)
            {
                if (MessageBox.Show("You need to install Autodesk Navisworks Export Utility to use this feature. Do you want to download it?", "Missing Utility", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("http://www.autodesk.com/products/navisworks/autodesk-navisworks-nwc-export-utility");
                }
                return Result.Failed;
            }


            using (TransactionGroup transGr = new TransactionGroup(doc))
            {
                transGr.Start("NWCExporterTransGr");
                NWCExporterViewModel viewModel = new NWCExporterViewModel(uidoc, doc, uiapp);
                NWCExporterWindow window = new NWCExporterWindow(viewModel);
                if (window.ShowDialog() == false) return Result.Cancelled;
                transGr.Assimilate();
            }

            return Result.Succeeded;
        }
    }
}
