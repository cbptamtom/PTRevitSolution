using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismRevitProject.Services
{
    public class TemporaryDocumentService
    {
        private static Document _document;

        public static Document Document
        {
            get { return _document; }
            set { _document = value; }
        }
    }
}
