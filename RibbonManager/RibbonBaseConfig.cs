using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace RibbonManager
{
    public class RibbonBaseConfig
    {
        private RibbonSettings ribbonSettings;
        private string imageFolder;
        private string dllFolder;

        public RibbonBaseConfig(ControlledApplication app)
        {
            ribbonSettings = new RibbonSettings(app);
            imageFolder = ribbonSettings.ImageFolder;
            dllFolder = ribbonSettings.DllFolder;
        }

        public PushButtonData CreatePushButton(
            string name, string displayName,
            string dllName, string fullClassName,
            string image, string tooltip,
            string helperPath = null,
            string longDescription = null,
            string tooltipImage = null,
            string linkYoutube = null)
        {
            PushButtonData pushButton = new PushButtonData(name, displayName,
                Path.Combine(dllFolder, dllName), fullClassName);

            pushButton.LargeImage = CreateBitmapImage(image);
            pushButton.ToolTip = tooltip;

            if (!string.IsNullOrEmpty(tooltipImage))
            {
                Uri tooltipUri = new Uri(Path.Combine(imageFolder, tooltipImage),
                    UriKind.Absolute);
                pushButton.ToolTipImage = new BitmapImage(tooltipUri);
            }

            if (!string.IsNullOrEmpty(helperPath))
            {
                ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.ChmFile,
                    helperPath);
                pushButton.SetContextualHelp(contextHelp);
            }

            if (!string.IsNullOrEmpty(linkYoutube))
            {
                ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url,
                    linkYoutube);
                pushButton.SetContextualHelp(contextHelp);
            }

            if (longDescription != null)
            {
                pushButton.LongDescription = longDescription;
            }

            return pushButton;
        }

        public PulldownButtonData CreatePulldownButton(
            string name, string displayName,
            string tooltip, string image,
            string helperPath = null,
            string linkYoutube = null,
            string tooltipImage = null)
        {
            PulldownButtonData pulldownButtonData
                = new PulldownButtonData(name, displayName);

            pulldownButtonData.ToolTip = tooltip;

            if (!string.IsNullOrEmpty(helperPath))
            {
                ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.ChmFile,
                    helperPath);
                pulldownButtonData.SetContextualHelp(contextHelp);
            }

            if (!string.IsNullOrEmpty(linkYoutube))
            {
                ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url,
                    linkYoutube);
                pulldownButtonData.SetContextualHelp(contextHelp);
            }

            if (!string.IsNullOrEmpty(image))
            {
                pulldownButtonData.Image = CreateBitmapImage(image);
            }

            if (!string.IsNullOrEmpty(tooltipImage))
            {
                pulldownButtonData.ToolTipImage = CreateBitmapImage(tooltipImage);
            }

            return pulldownButtonData;
        }

        public PulldownButton AddPulldownButton(
            RibbonPanel ribbonPanel,
            string name, string displayName,
            string tooltip, string largeImage,
            string tooltipImage = null,
            string helperPath = null,
            string linkYoutube = null)
        {
            PulldownButtonData pulldownButtonData
                = new PulldownButtonData(name, displayName);
            pulldownButtonData.ToolTip = tooltip;

            if (!string.IsNullOrEmpty(helperPath))
            {
                ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.ChmFile,
                    helperPath);
                pulldownButtonData.SetContextualHelp(contextHelp);
            }

            if (!string.IsNullOrEmpty(linkYoutube))
            {
                ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url,
                    linkYoutube);
                pulldownButtonData.SetContextualHelp(contextHelp);
            }

            if (!string.IsNullOrEmpty(largeImage))
            {
                pulldownButtonData.LargeImage = CreateBitmapImage(largeImage);
            }

            if (!string.IsNullOrEmpty(tooltipImage))
            {
                pulldownButtonData.ToolTipImage = CreateBitmapImage(tooltipImage);
            }

            return ribbonPanel.AddItem(pulldownButtonData) as PulldownButton;
        }

        public SplitButton AddSplitButton(
            RibbonPanel ribbonPanel, string name,
            string displayName, string tooltip,
            string helperPath = null,
            string linkYoutube = null)
        {
            SplitButtonData splitButtonData = new SplitButtonData(name, displayName);
            splitButtonData.ToolTip = tooltip;

            if (!string.IsNullOrEmpty(helperPath))
            {
                ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.ChmFile,
                        helperPath);
                splitButtonData.SetContextualHelp(contextHelp);
            }

            if (!string.IsNullOrEmpty(linkYoutube))
            {
                ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url,
                    linkYoutube);
                splitButtonData.SetContextualHelp(contextHelp);
            }

            return ribbonPanel.AddItem(splitButtonData) as SplitButton;
        }

        public BitmapImage CreateBitmapImage(string image)
        {
            string pathImage = Path.Combine(imageFolder, image);
            Uri iconUri = new Uri(pathImage, UriKind.Absolute);
            return new BitmapImage(iconUri);
        }
    }
}
