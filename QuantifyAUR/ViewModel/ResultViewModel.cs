using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using QuantifyAUR.Library.Orther;
using QuantifyAUR.Model;
using QuantifyAUR.View;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace QuantifyAUR
{
    public class ResultViewModel : BaseViewModel
    {
        private ObservableCollection<ResultItem> _results;

        public ObservableCollection<ResultItem> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                OnPropertyChanged(nameof(Results));
            }
        }
        private ResultView _resultControl;

        public ResultView ResultControl
        {
            get { return _resultControl; }
            set
            {
                _resultControl = value;
                OnPropertyChanged(nameof(ResultControl));
            }
        }

        private QuantifyModel _quantifyModel;

        public QuantifyModel QuantifyModel
        {
            get { return _quantifyModel; }
            set
            {
                _quantifyModel = value;
                OnPropertyChanged(nameof(QuantifyModel));
            }
        }

        public ResultViewModel(Autodesk.Revit.DB.Document doc)
        {
            Results = new ObservableCollection<ResultItem>();
            ResultControl = new ResultView(this);
            QuantifyModel = new QuantifyModel(doc);
        }

    }
}
