using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace QuantifyAUR
{
    public partial class QuantifyWindow
    {
        private QuantifyViewModel _viewModel;

        public QuantifyWindow(QuantifyViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            this.DataContext = viewModel;
        }



    }
}
