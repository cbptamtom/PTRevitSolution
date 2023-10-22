using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace CarbonAnalysis
{
    public partial class APIWindow
    {

        public APIWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }



    }
}
