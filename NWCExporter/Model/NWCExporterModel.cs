using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NWCExporter.Model
{
    public class NWCExporterModel : BaseViewModel
    {

        public UIApplication Application { get; private set; }
        private string _PathString;
        public string PathString { get { return _PathString; } set { _PathString = value; OnPropertyChanged(); } }
        private UserControl _OptionsUserControl;
        public UserControl OptionsUserControl { get { return _OptionsUserControl; } set { _OptionsUserControl = value; OnPropertyChanged(nameof(OptionsUserControl)); } }

        private List<View3DData> _View3Ds = new List<View3DData>();
        public List<View3DData> View3Ds { get { return _View3Ds; } set { _View3Ds = value; OnPropertyChanged(nameof(View3Ds)); } }

        private List<View3D> _Views;
        public List<View3D> Views { get { return _Views; } set { _Views = value; OnPropertyChanged(nameof(Views)); } }


        private List<string> _RevitFilePaths;
        public List<string> RevitFilePaths
        {
            get { return _RevitFilePaths; }
            set { _RevitFilePaths = value; OnPropertyChanged(); }
        }

        private string _PrefixValue;
        public string PrefixValue { get { return _PrefixValue; } set { _PrefixValue = value; OnPropertyChanged(); } }

        private string _SuffixValue;
        public string SuffixValue { get { return _SuffixValue; } set { _SuffixValue = value; OnPropertyChanged(); } }




        public NWCExporterModel(UIApplication application)
        {
            Application = application;
        }

        public void PopulateView3DList(Document document)
        {
            Views = new FilteredElementCollector(document).OfClass(typeof(View3D)).Cast<View3D>().Where(v => !v.IsTemplate).ToList();

            foreach (View3D view in Views)
            {
                View3DData data = new View3DData { Name = view.Name, Id = view.Id, Selected = false };
                View3Ds.Add(data);
            }
            View3Ds = View3Ds.OrderBy(v => v.Name).ToList();

        }

        public List<string> GetRevitFilesInDirectory(string directoryPath)
        {
            // Lấy danh sách tất cả các tệp trong thư mục
            string[] allFiles = Directory.GetFiles(directoryPath);

            // Lọc ra các tệp .rvt
            List<string> revitFiles = allFiles.Where(file => file.EndsWith(".rvt", StringComparison.OrdinalIgnoreCase)).ToList();

            return revitFiles;
        }
        public Document OpenDocumentFile(string modelPath)
        {
            return Application.Application.OpenDocumentFile(modelPath);
        }
        public string NormalizeFileName(string fileName)
        {
            return fileName.Replace("{", "").Replace("}", "");
        }
        public NavisworksExportOptions GetNavisworksExportOptions(ExportOptionModel exportOptions)
        {
            NavisworksExportOptions options = new NavisworksExportOptions();
            //param với link bị nhằm tên biến. Khi có time hãy cấu hình lại 
            options.ExportParts = exportOptions.IsConvertParts;
            options.ExportElementIds = exportOptions.IsConvertElemId;
            options.Parameters = GetNavisworksParameters(exportOptions.SelectedConvertLinkedFile);
            options.ConvertElementProperties = exportOptions.IsConvertElemProp;
            options.ExportLinks = exportOptions.IsConvertElemParam;
            options.ExportRoomAsAttribute = exportOptions.IsConvertRoom;
            options.ExportUrls = exportOptions.IsConvertURL;
            options.Coordinates = GetCoordinatesValue(exportOptions.SelectedCoordinates);
            options.DivideFileIntoLevels = exportOptions.IsDivideLevel;
            options.ExportScope = GetExportScopeValue(exportOptions.SelectedExport);
            options.ExportRoomGeometry = exportOptions.RoomGeometry;
            options.FindMissingMaterials = exportOptions.MissingMaterial;
            return options;
        }
        private NavisworksParameters GetNavisworksParameters(string selectedOption)
        {
            switch (selectedOption)
            {
                case "All":
                    return NavisworksParameters.All;
                case "Elements":
                    return NavisworksParameters.Elements;
                case "None":
                    return NavisworksParameters.None;
                default:
                    return NavisworksParameters.All;
            }
        }
        private NavisworksCoordinates GetCoordinatesValue(string selectedCoordinates)
        {
            switch (selectedCoordinates)
            {
                case "Internal":
                    return NavisworksCoordinates.Internal;
                case "Shared":
                    return NavisworksCoordinates.Shared;
                default:
                    return NavisworksCoordinates.Internal;
            }
        }
        private NavisworksExportScope GetExportScopeValue(string selectedExport)
        {
            switch (selectedExport)
            {
                case "Model":
                    return NavisworksExportScope.Model;
                case "SelectedElements":
                    return NavisworksExportScope.SelectedElements;
                case "Active View":
                    return NavisworksExportScope.View;
                default:
                    return NavisworksExportScope.View;
            }
        }

        public View3D CreateNewViewInRevit(Document doc, View3DData viewData)
        {
            // Tạo một giao dịch Revit
            using (Transaction transaction = new Transaction(doc, "Create New View"))
            {
                transaction.Start();

                // Tạo một ViewFamilyType (loại của view bạn muốn tạo, ví dụ: ViewType.ThreeD)
                FilteredElementCollector viewFamilyCollector = new FilteredElementCollector(doc);
                var viewFamily = viewFamilyCollector.OfClass(typeof(ViewFamilyType))
                    .Cast<ViewFamilyType>()
                    .FirstOrDefault(vft => vft.ViewFamily == ViewFamily.ThreeDimensional);

                if (viewFamily != null)
                {
                    // Tạo một View3D mới và thiết lập tên của nó
                    View3D newView = View3D.CreateIsometric(doc, viewFamily.Id);
                    newView.Name = viewData.Name;

                    // Commit giao dịch để lưu view mới
                    transaction.Commit();

                    return newView;
                }
                else
                {
                    // Nếu không tìm thấy loại view phù hợp, hãy thông báo lỗi hoặc xử lý tương ứng
                    return null;
                }
            }
        }


        //public bool IsViewInDocument(Document document, View3DData view)
        //{
        //    // Kiểm tra xem view có tồn tại trong tài liệu không
        //    return document.GetElement(view.Id) != null;
        //}

        //public void DuplicateViewIfNotExists(Document document, View3DData view)
        //{
        //    if (!IsViewInDocument(document, view))
        //    {
        //        // Sao chép view từ view mẫu (hoặc view mặc định) và đặt tên theo tên view đã chọn
        //        // Sau đó, gán Id của view mới cho view đã chọn
        //    }
        //}






    }
    public class View3DData : BaseViewModel
    {
        private bool _selected;

        public string Name { get; set; }
        public ElementId Id { get; set; }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged(nameof(Selected));
                }
            }
        }
    }
}
