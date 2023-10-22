using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AUR_REAL_REBAR_FOUNDATION.ViewModels
{
    public class View2ViewModel : BindableBase
    {
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { SetProperty(ref isActive, value); }
        }
        public ICommand GeometryCommand { get; private set; }

        public View2ViewModel()
        {
            GeometryCommand = new DelegateCommand(() => MessageBox.Show("Geometry"));
        }
    }
}
