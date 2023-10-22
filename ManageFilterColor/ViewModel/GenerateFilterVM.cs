using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ManageFilterColor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManageFilterColor.ViewModel
{

    public class GenerateFilterVM : BaseViewModel
    {

        public ICommand ClickCommand { get; set; }
        public GenerateFilterVM()
        {
            ClickCommand = new RelayCommand<MainWindow>(p => true, p =>
            {
                System.Windows.Forms.MessageBox.Show("Test");
            });
        }
    }
}
