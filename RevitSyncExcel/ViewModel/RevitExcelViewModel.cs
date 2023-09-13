#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitSyncExcel.Model;
using RevitSyncExcel.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace RevitSyncExcel
{
    public class RevitExcelViewModel : BaseViewModel
    {
        public UIDocument UiDoc;
        public Document Doc;

        private RevitExcelModel _RevitExcelModel;
        public RevitExcelModel RevitExcelModel { get { return _RevitExcelModel; } set { _RevitExcelModel = value; OnPropertyChanged(); } }

        public RevitExcelViewModel(UIDocument uiDoc, Document doc)
        {
            UiDoc = uiDoc;
            Doc = doc;

            RevitExcelModel = new RevitExcelModel(Doc);

        }

    }
}
