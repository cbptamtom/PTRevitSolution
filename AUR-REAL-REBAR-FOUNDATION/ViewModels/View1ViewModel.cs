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
    public class View1ViewModel : BindableBase
    {
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { SetProperty(ref isActive, value); }
        }
        public ICommand SettingCommand { get; private set; }
        public View1ViewModel()
        {
            SettingCommand = new DelegateCommand(() => MessageBox.Show("Setting"));
        }
    }
}
