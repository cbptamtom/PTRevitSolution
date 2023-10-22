﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace QuantifyAUR.Button
{
    [Transaction(TransactionMode.Manual)]
    public class Button : IExternalApplication
    {
        private const string tabName = "REAL Tool";
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel toolsPanel = GetOrCreateRibbonPanel(application);
            string assemblieFolder = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location);
            string commandPath = Path.Combine(assemblieFolder, "QuantifyAUR.dll");
            PushButton pushButton = toolsPanel.AddItem(new PushButtonData(
                                    "NWC Exporter",
                                    "Carbon calculator",
                                    commandPath,
                                    "QuantifyAUR.QuantifyCmd")) as PushButton;
            var buttonImage = Path.Combine(assemblieFolder, @"Resources\NWC.png");

            if (!File.Exists(buttonImage))
                buttonImage = Path.Combine(Directory.GetParent(assemblieFolder).FullName, @"Resources\NWC.png");

            pushButton.LargeImage = new BitmapImage(new Uri(buttonImage));
            pushButton.ToolTip = "Export your Revit 3D views in batch to Navisworks";
            pushButton.ToolTipImage = new BitmapImage(new Uri(buttonImage));
            return Result.Succeeded;
        }
        private RibbonPanel GetOrCreateRibbonPanel(UIControlledApplication application)
        {
            application.CreateRibbonTab(tabName);
            var ribbonPanel = application.CreateRibbonPanel(tabName, "REAL Export");
            return ribbonPanel;
        }
    }
}
