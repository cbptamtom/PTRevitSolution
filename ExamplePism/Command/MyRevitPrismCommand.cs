using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows;
using MyRevitPrismApp.ViewModels;
using MyRevitPrismApp;

namespace ExamplePism.Command
{
    [Transaction(TransactionMode.Manual)]
    public class MyRevitPrismCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
                Document doc = uidoc.Document;


                // Tạo và hiển thị MainWindow
                MainWindow mainWindow = new MainWindow();
                // Tạo ViewModel cho MainWindow
                MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(doc);
                // Gán ViewModel cho MainWindow
                mainWindow.DataContext = mainWindowViewModel;
                // Hiển thị MainWindow
                mainWindow.Show();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
