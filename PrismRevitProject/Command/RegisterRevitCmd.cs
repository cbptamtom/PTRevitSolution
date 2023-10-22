using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using PrismRevitProject.Services;
using PrismRevitProject.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace PrismRevitProject.Command
{
    [Transaction(TransactionMode.Manual)]
    public class PrismRevitCmd : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            TransactionGroup transGr = new TransactionGroup(doc);
            using (transGr)
            {
                transGr.Start("Do something");
                DIContainer.Instance.Register(doc);
                DIContainer.Instance.Register(transGr);

                DocumentService.SetDocument(doc);
                MainViewModel viewModel = new MainViewModel();
                MainWindow mainWindow = new MainWindow();
                mainWindow.DataContext = viewModel;
                if (mainWindow.ShowDialog() == false)
                {
                    return Result.Succeeded;

                }
                transGr.Assimilate();
                return Result.Succeeded;

            }
        }
    }
}
