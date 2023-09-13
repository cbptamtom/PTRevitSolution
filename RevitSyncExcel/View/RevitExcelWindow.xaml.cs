using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace RevitSyncExcel
{
    public partial class RevitExcelWindow
    {
        private RevitExcelViewModel _viewModel;

        public RevitExcelWindow(RevitExcelViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            this.DataContext = viewModel;
        }
    }
}
