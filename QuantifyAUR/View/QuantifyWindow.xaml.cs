using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace QuantifyAUR
{
    public partial class APIWindow
    {
        private QuantifyViewModel _viewModel;

        public APIWindow(QuantifyViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            this.DataContext = viewModel;
        }



    }
}
