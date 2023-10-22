using AUR_REAL_REBAR_FOUNDATION.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AUR_REAL_REBAR_FOUNDATION.ViewModels
{
    public class MainViewModel : BindableBase
    {



        public ObservableCollection<MenuItemViewModel> MenuItems { get; }


        private UserControl currentView;
        public UserControl CurrentView
        {
            get { return currentView; }
            set { SetProperty(ref currentView, value); }
        }

        private View1ViewModel _view1ViewModel;
        public View1ViewModel View1ViewModel
        {
            get { return _view1ViewModel; }
            set { SetProperty(ref _view1ViewModel, value); }
        }

        private View2ViewModel _View2ViewModel;
        public View2ViewModel View2ViewModel
        {
            get { return _View2ViewModel; }
            set { SetProperty(ref _View2ViewModel, value); }
        }



        public ICommand TestCommand { get; private set; }
        public ICommand ShowView1Command { get; set; }
        public ICommand ShowView2Command { get; set; }

        public MainViewModel()
        {

            _view1ViewModel = new View1ViewModel();
            _View2ViewModel = new View2ViewModel();
            CurrentView = new SettingView();
            TestCommand = new DelegateCommand(() => MessageBox.Show("Test"));


            ShowView1Command = new DelegateCommand(() =>
            {
                _view1ViewModel.IsActive = true;
                _View2ViewModel.IsActive = false;
                CurrentView = new SettingView();
            });

            ShowView2Command = new DelegateCommand(() =>
            {
                _view1ViewModel.IsActive = false;
                _View2ViewModel.IsActive = true;
                CurrentView = new GeometryView();
            });


        }


    }

}
