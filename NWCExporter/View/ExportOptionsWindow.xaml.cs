using Autodesk.Revit.DB;
using NWCExporter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NWCExporter.View
{
    /// <summary>
    /// Interaction logic for ExportOptionsWindow.xaml
    /// </summary>
    public partial class ExportOptionsWindow : Window
    {
        private ExportOptionsViewModel _viewModel;
        public ExportOptionsWindow(ExportOptionsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            this.DataContext = viewModel;
        }
    }
}
