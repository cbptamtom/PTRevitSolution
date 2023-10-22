using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AmazingNavigation.MVVM.ViewModel;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;





namespace AmazingNavigation.Command
{
    [Transaction(TransactionMode.Manual)]
    public class AmazingNavigationCommand : IExternalCommand
    {
        private ServiceProvider _serviceProvider;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (TransactionGroup transGr = new TransactionGroup(doc))
            {
                IServiceCollection services = new ServiceCollection();
                services.AddSingleton<MainWindow>(provider => new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainViewModel>()
                });
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<SettingViewModel>();
                _serviceProvider = services.BuildServiceProvider();

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }



            return Result.Succeeded;
        }


    }
}
