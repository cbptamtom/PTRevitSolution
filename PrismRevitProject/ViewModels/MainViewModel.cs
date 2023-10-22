using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using PrismRevitProject.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace PrismRevitProject.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand HomeCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand DataAnalysisCommand { get; set; }
        public ICommand GeometryCommand { get; set; }
        public ICommand CloseWindow { get; set; }

        private void Home(object obj) => CurrentView = new HomeViewModel();
        private void Setting(object obj)
        {
            CurrentView = new SettingViewModel();
        }
        private void DataAnalysis(object obj) => CurrentView = new DataAnalysisViewModel();
        private void Geometry(object obj) => CurrentView = new GeometryViewModel();
        private void Close(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
        public MainViewModel()
        {

            HomeCommand = new RelayCommand(Home);
            SettingCommand = new RelayCommand(Setting);
            DataAnalysisCommand = new RelayCommand(DataAnalysis);
            GeometryCommand = new RelayCommand(Geometry);
            CurrentView = new HomeViewModel();
            CloseWindow = new RelayCommand(Close);

        }
    }
}
