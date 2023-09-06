using NWCExporter.Model;
using NWCExporter.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace NWCExporter.ViewModel
{
    public class ExportOptionsViewModel : BaseViewModel
    {
        private ExportOptionModel _ExportOptionModel;
        public ExportOptionModel ExportOptionModel { get { return _ExportOptionModel; } set { _ExportOptionModel = value; OnPropertyChanged(); } }

        public ICommand DefaultCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ExportOptionsViewModel()
        {
            ExportOptionModel = new ExportOptionModel();
            DefaultCommand = new RelayCommand<ExportOptionsWindow>((p) => { return true; }, (p) =>
            {
                ExportOptionModel.DefaultSettingValueAttribute();
            });
            CancelCommand = new RelayCommand<ExportOptionsWindow>((p) => { return true; }, (p) =>
            {
                p.Close();
            });


        }
    }
}
