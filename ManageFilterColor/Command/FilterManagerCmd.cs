﻿
#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ManageFilterColor.Library.Filter;
using ManageFilterColor.Library.Orther;
using ManageFilterColor.Library.Unit;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion


namespace ManageFilterColor.Command
{
    [Transaction(TransactionMode.Manual)]
    public class FilterManagerCmd : IExternalCommand
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

                MainViewModel viewModel = new MainViewModel(uidoc, doc);
                MainWindow window = new MainWindow(viewModel);
                if (window.ShowDialog() == false) return Result.Cancelled;

                transGr.Assimilate();
            }

            return Result.Succeeded;
        }
    }
}
