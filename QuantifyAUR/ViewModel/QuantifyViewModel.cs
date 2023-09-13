#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace QuantifyAUR
{
    public class QuantifyViewModel : BaseViewModel
    {
        public UIDocument UiDoc;
        public Document Doc;
        private Categories _AllCategories;

        public Categories AllCategories
        {
            get { return _AllCategories; }
            set { _AllCategories = value; }
        }


        public QuantifyViewModel(UIDocument uiDoc, Document doc)
        {
            UiDoc = uiDoc;
            Doc = doc;



            GetAllCategories();
            MessageBox.Show(AllCategories + "");

        }

        private void GetAllCategories()
        {
            AllCategories = Doc.Settings.Categories;
        }
    }
}
