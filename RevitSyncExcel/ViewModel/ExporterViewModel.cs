using Autodesk.Revit.DB;
using RevitSyncExcel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitSyncExcel.ViewModel
{
    public class ExporterViewModel : BaseViewModel
    {
        public Document Doc;
        private ExporterModel _ExporterModel;
        public ExporterModel ExporterModel { get { return _ExporterModel; } set { _ExporterModel = value; OnPropertyChanged(); } }


        public ExporterViewModel(Document doc)
        {
            Doc = doc;
            ExporterModel = new ExporterModel(Doc);
        }
    }
}
