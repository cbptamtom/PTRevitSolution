using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace NWCExporter
{
    public partial class NWCExporterWindow
    {
        private NWCExporterViewModel _viewModel;

        public NWCExporterWindow(NWCExporterViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            this.DataContext = viewModel;
        }

        private void lbViews3D_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {

        }
    }
}
