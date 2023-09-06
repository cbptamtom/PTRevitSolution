#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NWCExporter.Model;
using System.Windows.Input;
using System.Windows.Forms;
using NWCExporter.View;
using NWCExporter.ViewModel;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Windows.Threading;
using System;
#endregion

namespace NWCExporter
{
    public class NWCExporterViewModel : BaseViewModel
    {
        public UIDocument UiDoc;
        public Document Doc;
        public UIApplication Application;
        private NWCExporterModel _NWCExporterModel;
        public NWCExporterModel NWCExporterModel { get { return _NWCExporterModel; } set { _NWCExporterModel = value; OnPropertyChanged(); } }

        private ExportOptionsViewModel _ExportOptionsViewModel;

        public ExportOptionsViewModel ExportOptionsViewModel { get { return _ExportOptionsViewModel; } set { _ExportOptionsViewModel = value; OnPropertyChanged(); } }



        private List<Document> _RevitDocuments;
        public List<Document> RevitDocuments
        {
            get { return _RevitDocuments; }
            set { _RevitDocuments = value; OnPropertyChanged(); }
        }

        #region Icommand
        public ICommand BrowserCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ShowExportOptions { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand CheckAllCommand { get; set; }
        public ICommand CheckNoneCommand { get; private set; }

        #endregion
        public NWCExporterViewModel(UIDocument uiDoc, Document doc, UIApplication application)
        {
            Application = application;
            UiDoc = uiDoc;
            Doc = doc;
            NWCExporterModel = new NWCExporterModel(application);
            NWCExporterModel.PopulateView3DList(Doc);
            ExportOptionsViewModel = new ExportOptionsViewModel();
            BrowserCommand = new RelayCommand<NWCExporterWindow>((p) => { return true; }, (p) =>
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                if (fd.ShowDialog() == DialogResult.OK)
                    NWCExporterModel.PathString = fd.SelectedPath;
                //List<string> revitFiles = NWCExporterModel.GetRevitFilesInDirectory(fd.SelectedPath);

                //foreach (string revitFilePath in revitFiles)
                //{
                //    // Sử dụng phương thức OpenDocumentFile từ Model để mở tệp Revit
                //    Document doca = NWCExporterModel.OpenDocumentFile(revitFilePath);
                //    if (doca != null)
                //    {
                //        MessageBox.Show(doca.ProjectInformation.Name);
                //        // Xuất tệp Revit thành tệp NWC ở đây, sử dụng các tùy chọn xuất NWC
                //        // Sau khi xuất, bạn có thể đóng tài liệu này nếu cần
                //    }
                //}


            });
            CancelCommand = new RelayCommand<NWCExporterWindow>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
            ShowExportOptions = new RelayCommand<NWCExporterWindow>((p) => { return true; }, (p) =>
            {
                ExportOptionsWindow optionsWindow = new ExportOptionsWindow(ExportOptionsViewModel);
                optionsWindow.ShowDialog();
            });
            CheckAllCommand = new RelayCommand<NWCExporterWindow>((p) => { return true; }, (p) =>
            {
                NWCExporterModel.View3Ds.ForEach(v => { v.Selected = true; });
            });
            CheckNoneCommand = new RelayCommand<NWCExporterWindow>((p) => { return true; }, (p) =>
            {
                NWCExporterModel.View3Ds.ForEach(v => { v.Selected = false; });
            });
            ExportCommand = new RelayCommand<NWCExporterWindow>((p) => { return true; }, (p) =>
            {



                #region old
                //var selectedView3D = NWCExporterModel.View3Ds.Where(v => v.Selected).ToList();
                if (string.IsNullOrWhiteSpace(NWCExporterModel.PathString))
                {
                    MessageBox.Show("Please fill Folder Path value");
                    return;
                }

                if (!Directory.Exists(NWCExporterModel.PathString))
                {
                    MessageBox.Show("Path does not exist.");
                    return;
                }

                if (!NWCExporterModel.View3Ds?.Any(view => view.Selected) ?? true)
                {
                    System.Windows.Forms.MessageBox.Show("Please select at least one view.");
                    return;
                }


                List<string> revitFiles = NWCExporterModel.GetRevitFilesInDirectory(NWCExporterModel.PathString);
                NavisworksExportOptions option = NWCExporterModel.GetNavisworksExportOptions(ExportOptionsViewModel.ExportOptionModel);


                foreach (string revitFilePath in revitFiles)
                {
                    // Sử dụng phương thức OpenDocumentFile từ Model để mở tệp Revit
                    Document doca = NWCExporterModel.OpenDocumentFile(revitFilePath);
                    if (doca != null)
                    {
                        foreach (View3DData view in NWCExporterModel.View3Ds)
                        {
                            if (view.Selected)
                            {
                                View3D revitView = doca.GetElement(view.Id) as View3D;
                                if (revitView != null)
                                {
                                    NavisworksExportOptions options = option;
                                    options.ViewId = view.Id;
                                    option.ExportScope = NavisworksExportScope.View;
                                    string exportFilePath = NWCExporterModel.NormalizeFileName($"{NWCExporterModel.PrefixValue}-{Path.GetFileNameWithoutExtension(revitFilePath)}-{view.Name}-{NWCExporterModel.SuffixValue}.nwc");
                                    try
                                    {
                                        doca.Export(NWCExporterModel.PathString, exportFilePath, options);
                                    }
                                    catch (System.Exception)
                                    {

                                        throw;
                                    }
                                }
                                else
                                {
                                    View3D sourceView = new FilteredElementCollector(doca).OfClass(typeof(View3D)).Cast<View3D>().FirstOrDefault(v => v.Name.Equals("{3D}"));
                                    if (sourceView != null)
                                    {
                                        NavisworksExportOptions options1 = option;
                                        options1.ViewId = sourceView.Id;
                                        options1.ExportScope = NavisworksExportScope.View;
                                        string exportFilePath = NWCExporterModel.NormalizeFileName($"{NWCExporterModel.PrefixValue}-{Path.GetFileNameWithoutExtension(revitFilePath)}-{sourceView.Name}-{NWCExporterModel.SuffixValue}.nwc");
                                        try
                                        {
                                            doca.Export(NWCExporterModel.PathString, exportFilePath, options1);

                                        }
                                        catch (System.Exception)
                                        {

                                            throw;
                                        }
                                    }

                                }

                            }
                        }

                    }
                }
                #endregion

            });



        }
    }

}
