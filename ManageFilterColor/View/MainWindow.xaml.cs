using MahApps.Metro.Controls;
using ManageFilterColor.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ManageFilterColor
{
    public partial class MainWindow : MetroWindow
    {

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }



    }
}
