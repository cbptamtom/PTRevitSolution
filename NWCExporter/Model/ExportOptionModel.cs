using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCExporter.Model
{
    public class ExportOptionModel : BaseViewModel
    {
        private bool _IsConvertParts = false;
        public bool IsConvertParts { get { return _IsConvertParts; } set { _IsConvertParts = value; OnPropertyChanged(); } }

        private bool _IsConvertElemId = true;
        public bool IsConvertElemId { get { return _IsConvertElemId; } set { _IsConvertElemId = value; OnPropertyChanged(); } }

        private bool _IsConvertElemParam;
        public bool IsConvertElemParam { get { return _IsConvertElemParam; } set { _IsConvertElemParam = value; OnPropertyChanged(); } }

        private bool _IsConvertElemProp = false;
        public bool IsConvertElemProp { get { return _IsConvertElemProp; } set { _IsConvertElemProp = value; OnPropertyChanged(); } }

        private List<string> _ConvertLinkedFile;
        public List<string> ConvertLinkedFile { get { if (_ConvertLinkedFile == null) { _ConvertLinkedFile = new List<string>() { "All", "Elements", "None" }; } return _ConvertLinkedFile; } set { _ConvertLinkedFile = value; OnPropertyChanged(); } }

        private string _SelectedConvertLinkedFile;
        public string SelectedConvertLinkedFile { get { return _SelectedConvertLinkedFile; } set { _SelectedConvertLinkedFile = value; OnPropertyChanged(); } }

        private bool _IsConvertRoom = true;
        public bool IsConvertRoom { get { return _IsConvertRoom; } set { _IsConvertRoom = value; OnPropertyChanged(); } }

        private bool _IsConvertURL = true;
        public bool IsConvertURL { get { return _IsConvertURL; } set { _IsConvertURL = value; OnPropertyChanged(); } }

        private List<string> _Coordinates;
        public List<string> Coordinates { get { if (_Coordinates == null) { _Coordinates = new List<string>() { "Internal", "Shared" }; } return _Coordinates; } set { _Coordinates = value; OnPropertyChanged(); } }

        private string _SelectedCoordinates;
        public string SelectedCoordinates { get { return _SelectedCoordinates; } set { _SelectedCoordinates = value; OnPropertyChanged(); } }

        private bool _IsDivideLevel = false;
        public bool IsDivideLevel { get { return _IsDivideLevel; } set { _IsDivideLevel = value; OnPropertyChanged(); } }

        private List<string> _Export;
        public List<string> Export { get { if (_Export == null) { _Export = new List<string>() { "Model", "SelectedElements", "Active View" }; } return _Export; } set { _Export = value; OnPropertyChanged(); } }


        private string _SelectedExport;
        public string SelectedExport { get { return _SelectedExport; } set { _SelectedExport = value; OnPropertyChanged(); } }

        private bool _RoomGeometry = true;
        public bool RoomGeometry { get { return _RoomGeometry; } set { _RoomGeometry = value; OnPropertyChanged(); } }

        private bool _MissingMaterial = true;
        public bool MissingMaterial { get { return _MissingMaterial; } set { _MissingMaterial = value; OnPropertyChanged(); } }
        //SelectedCoordinates

        public ExportOptionModel()
        {
            SelectedConvertLinkedFile = ConvertLinkedFile[0];
            SelectedCoordinates = Coordinates[0];
            SelectedExport = Export[0];
        }

        public void DefaultSettingValueAttribute()
        {
            IsConvertParts = false;
            IsConvertElemId = true;
            IsConvertElemParam = false;
            IsConvertElemProp = false;
            IsConvertRoom = true;
            IsConvertURL = true;
            IsDivideLevel = false;
            RoomGeometry = true;
            MissingMaterial = true;
            SelectedConvertLinkedFile = ConvertLinkedFile[0];
            SelectedCoordinates = Coordinates[0];
            SelectedExport = Export[2];
        }
    }
}
