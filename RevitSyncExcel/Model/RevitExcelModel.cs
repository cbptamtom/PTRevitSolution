using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitSyncExcel.Model
{
    public class RevitExcelModel : BaseViewModel
    {

        private ExporterModel _ExporterModel;
        public ExporterModel ExporterModel { get { return _ExporterModel; } set { _ExporterModel = value; OnPropertyChanged(); } }



        public RevitExcelModel(Document doc)
        {
            ExporterModel = new ExporterModel(doc);
        }
    }
}
