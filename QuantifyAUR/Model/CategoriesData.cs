using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantifyAUR.Model
{

    // This is the object including Name, Id and Selected 

    public class CategoriesData : BaseViewModel
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
