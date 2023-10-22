using Autodesk.Revit.DB;
using PrismRevitProject.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace PrismRevitProject
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MouseDown_Border(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }
        }


    }
}
