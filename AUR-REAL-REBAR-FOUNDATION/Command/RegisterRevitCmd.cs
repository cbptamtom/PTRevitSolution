using AUR_REAL_REBAR_FOUNDATION.ViewModels;
using AUR_REAL_REBAR_FOUNDATION.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AUR_REAL_REBAR_FOUNDATION.Command
{
    [Transaction(TransactionMode.Manual)]
    public class PrismRevitCmd : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MainViewModel mainViewModel = new MainViewModel();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();




            return Result.Succeeded;
        }
    }
}
