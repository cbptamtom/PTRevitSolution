using Autodesk.Revit.DB;
using MyRevitPrismApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyRevitPrismApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        private ColumnViewModel columnViewModel;
        private FrammingViewModel framingViewModel;
        private UserControl currentView;
        public ColumnViewModel ColumnViewModel
        {
            get { return columnViewModel; }
            set { SetProperty(ref columnViewModel, value); }
        }

        public FrammingViewModel FramingViewModel
        {
            get { return framingViewModel; }
            set { SetProperty(ref framingViewModel, value); }
        }
        public UserControl CurrentView
        {
            get { return currentView; }
            set { SetProperty(ref currentView, value); }
        }

        //private BindableBase currentViewModel;
        //public BindableBase CurrentViewModel
        //{
        //    get { return currentViewModel; }
        //    set { SetProperty(ref currentViewModel, value); }
        //}

        public ICommand ShowColumnViewCommand { get; set; }
        public ICommand ShowFramingViewCommand { get; set; }
        public MainWindowViewModel(Document document)
        {
            columnViewModel = new ColumnViewModel(document);
            framingViewModel = new FrammingViewModel(document);
            CurrentView = new ColumnView();

            ShowColumnViewCommand = new DelegateCommand(ShowColumnView);
            ShowFramingViewCommand = new DelegateCommand(ShowFramingView);
        }
        private void ShowColumnView()
        {
            CurrentView = new ColumnView();
        }

        private void ShowFramingView()
        {
            CurrentView = new FramingView();
        }
    }
}
