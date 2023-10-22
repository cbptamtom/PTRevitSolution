using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace PrismRevitProject.ViewModels
{
    public class GeometryViewModel : BindableBase
    {
        public ICommand GeometryCommand { get; private set; }

        public GeometryViewModel()
        {
            GeometryCommand = new DelegateCommand(() => MessageBox.Show("Geometry"));
        }
    }
}
