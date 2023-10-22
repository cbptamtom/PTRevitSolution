using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RibbonManager
{
    public class RibbonManager : IExternalApplication
    {
        private UIControlledApplication _application;

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "REAL Tools";
            bool tabCreated = false;

            RibbonPanel firstPanel = GetOrCreateRibbonPanel(application, tabName, "REAL Tools", ref tabCreated);
            CreatePushButton(firstPanel, "Carbon01909", "Carbon\nCalculator", "QuantifyAUR.dll", "QuantifyAUR.QuantifyCmd", "building.png", "Another Tooltip");
            CreatePushButton(firstPanel, "NWC Exporter", " Export\nNWC", "NWCExporter.dll", "NWCExporter.NWCExporterCmd", "nwcexporter.png", "Export your Revit 3D views in batch to Navisworks");
            RibbonPanel secondPanel = GetOrCreateRibbonPanel(application, tabName, "REAL Details", ref tabCreated);
            CreatePushButton(secondPanel, "NWC Exporter", " Reinforcement\nFoundation", "NWCExporter.dll", "NWCExporter.NWCExporterCmd", "smile.png", "Export your Revit 3D views in batch to Navisworks");

            return Result.Succeeded;


        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


        private PushButton CreatePushButton(RibbonPanel panel, string name, string text, string dllName, string className, string imageName, string tooltip)
        {
            string assemblieFolder = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location);
            string commandPath = Path.Combine(assemblieFolder, dllName);
            PushButton pushButton = panel.AddItem(new PushButtonData(name, text, commandPath, className)) as PushButton;

            var buttonImage = Path.Combine(assemblieFolder, @"Resources\" + imageName);

            if (!File.Exists(buttonImage))
                buttonImage = Path.Combine(Directory.GetParent(assemblieFolder).FullName, @"Resources\" + imageName);

            pushButton.LargeImage = new BitmapImage(new Uri(buttonImage));
            pushButton.ToolTip = tooltip;
            pushButton.ToolTipImage = new BitmapImage(new Uri(buttonImage));

            return pushButton;
        }

        private RibbonPanel GetOrCreateRibbonPanel(UIControlledApplication application, string tabName, string panelName, ref bool tabCreated)
        {
            // Kiểm tra nếu tab đã được tạo
            if (!tabCreated)
            {
                application.CreateRibbonTab(tabName);
                tabCreated = true;
            }

            List<RibbonPanel> panels = application.GetRibbonPanels(tabName);
            RibbonPanel ribbonPanel = panels.FirstOrDefault(p => p.Name == panelName);

            if (ribbonPanel == null)
            {
                ribbonPanel = application.CreateRibbonPanel(tabName, panelName);
            }

            return ribbonPanel;
        }

    }
}
