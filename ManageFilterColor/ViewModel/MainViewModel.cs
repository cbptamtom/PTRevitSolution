#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ManageFilterColor.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace ManageFilterColor
{
    public class MainViewModel : BaseViewModel
    {
        public UIDocument UiDoc;
        public Document Doc;
        private GenerateFilterVM _GenerateFilterVM;
        public GenerateFilterVM GenerateFilterVM { get { return _GenerateFilterVM; } set { _GenerateFilterVM = value; OnPropertyChanged(); } }


        public MainViewModel(UIDocument uiDoc, Document doc)
        {
            UiDoc = uiDoc;
            Doc = doc;
            GenerateFilterVM = new GenerateFilterVM();
        }

    }
}
