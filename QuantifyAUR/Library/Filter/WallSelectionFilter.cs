﻿#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

#endregion

namespace QuantifyAUR.Library.Filter
{
    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is Wall;

        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
