using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;

namespace RibbonManager
{
    public class RibbonSettings
    {
        public string ContentsFolder { get; private set; }
        public string ResourcesFolder { get; private set; }
        public string HelpFolder { get; private set; }
        public string ImageFolder { get; private set; }
        public string SettingFolder { get; private set; }
        public string DllFolder { get; private set; }
        public BitmapImage IconWindow { get; private set; }
        public string HelperPath { get; private set; }

        public RibbonSettings(ControlledApplication a = null)
        {
            ContentsFolder = Path.Combine(@"C:\ProgramData\Autodesk\ApplicationPlugins\REALTool.bundle\Contents");
            ResourcesFolder = Path.Combine(ContentsFolder, "Resources");
            HelpFolder = Path.Combine(ResourcesFolder, "Help");
            ImageFolder = Path.Combine(ResourcesFolder, "Image");
            SettingFolder = Path.Combine(ResourcesFolder, "Setting");

            if (a != null)
            {
                string versionNumber = a.VersionNumber;
                DllFolder = Path.Combine(ContentsFolder, versionNumber, "dll");
                MessageBox.Show(DllFolder + "");
            }

            string iconWindowPath = Path.Combine(ImageFolder, "About.ico");
            IconWindow = new BitmapImage(new Uri(iconWindowPath, UriKind.Relative));
        }
    }
}
