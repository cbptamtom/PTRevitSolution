#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using QuantifyAUR.Excel;
using QuantifyAUR.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

#endregion

namespace QuantifyAUR
{
    public class QuantifyViewModel : BaseViewModel
    {
        public UIDocument UiDoc;
        public Document Doc;

        private string _ToggleResultName;
        public string ToggleResultName { get { return _ToggleResultName; } set { _ToggleResultName = value; OnPropertyChanged(); } }

        private bool _isResultControlVisible;
        public bool IsResultControlVisible { get { return _isResultControlVisible; } set { _isResultControlVisible = value; OnPropertyChanged(); ToggleResultName = value ? "Hide Result" : "Show Result"; } }



        private ResultViewModel _resultViewModel;
        public ResultViewModel ResultViewModel { get { return _resultViewModel; } set { _resultViewModel = value; OnPropertyChanged(); } }

        private QuantifyModel _quantifyModel;
        public QuantifyModel QuantifyModel { get { return _quantifyModel; } set { _quantifyModel = value; OnPropertyChanged(); } }
        private ExcelService _excelService;
        public ExcelService ExcelService { get { return _excelService; } set { _excelService = value; OnPropertyChanged(); } }


        private bool _isApply = false;
        public bool IsApply
        {
            get { return !string.IsNullOrEmpty(PathString) && QuantifyModel.CategoriesData.Any(cat => cat.Selected); }
            set
            {
                _isApply = value;
                OnPropertyChanged();
            }
        }
        private bool _isAnyCategorySelected = false;

        public bool IsAnyCategorySelected
        {
            get { return _isAnyCategorySelected; }
            set
            {
                _isAnyCategorySelected = value;
                OnPropertyChanged();
            }
        }
        private string _PathString;
        public string PathString
        {
            get { return _PathString; }
            set
            {
                _PathString = value;
                OnPropertyChanged("PathString");
            }
        }
        public ICommand BrowserCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CheckAllCommand { get; set; }
        public ICommand CheckNoneCommand { get; private set; }
        public ICommand ToggleResultCommand { get; set; }


        public QuantifyViewModel(UIDocument uiDoc, Document doc)
        {
            UiDoc = uiDoc;
            Doc = doc;
            ExcelService = new ExcelService();
            QuantifyModel = new QuantifyModel(Doc);
            ResultViewModel = new ResultViewModel(Doc);
            ToggleResultName = "Show Result";
            BrowserCommand = new RelayCommand<QuantifyWindow>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel Files|*.xls;*.xlsx",
                    Title = "Select Excel File"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    PathString = selectedFilePath;
                }

            });
            CheckAllCommand = new RelayCommand<QuantifyWindow>((p) => { return true; }, (p) =>
            {
                QuantifyModel.CategoriesData.ForEach(cat => { cat.Selected = true; });
            });
            CheckNoneCommand = new RelayCommand<QuantifyWindow>((p) => { return true; }, (p) =>
            {
                QuantifyModel.CategoriesData.ForEach(cat => { cat.Selected = false; });
            });
            ToggleResultCommand = new RelayCommand<QuantifyWindow>((p) => { return true; }, (p) =>
            {
                IsResultControlVisible = !IsResultControlVisible;

            });
            ExportCommand = new RelayCommand<QuantifyWindow>((p) => { return IsApply; }, (p) =>
            {

                if (string.IsNullOrEmpty(PathString))
                {
                    MessageBox.Show("Please provide the Excel file path.");
                    return;
                }
                if (!QuantifyModel.CategoriesData.Any(category => category.Selected))
                {
                    MessageBox.Show("Please select at least one category for calculation.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExcelService.AliasColumn))
                {
                    MessageBox.Show("Please provide a value for Alias Column.");
                    return;
                }
                if (
                  string.IsNullOrWhiteSpace(ExcelService.OutputColumn))
                {
                    MessageBox.Show("Please provide a value for Output Column.");
                    return;
                }

                if (ExcelService.StartRow == 0)
                {
                    MessageBox.Show("Please provide a valid Start Row.");
                    return;
                }

                QuantifyModel.GetSelectedElements();
                QuantifyModel.GetAliasValues();
                QuantifyModel.GetUnitQuantityValues();
                QuantifyModel.CombineData(QuantifyModel.Elements, QuantifyModel.AliasValues, QuantifyModel.UnitQuantity);
                QuantifyModel.SumUnitQtyValuesByAliases(QuantifyModel.CombinedData);
                ExcelService.UpdateExcelFile(PathString, QuantifyModel.SumByAliasesDictionary, QuantifyModel.TotalAreaFloors, QuantifyModel.RevitName);


                //StringBuilder message = new StringBuilder();
                //message.AppendLine("Combined Data:");
                //foreach (var data in QuantifyModel.CombinedData)
                //{
                //    message.AppendLine($"Element: {data.Item1.Name}, Alias: {data.Item2}, Unit Quantity: {data.Item3}");
                //}

                //message.AppendLine("\nSum By Aliases:");
                //foreach (var kvp in QuantifyModel.SumByAliasesDictionary)
                //{
                //    message.AppendLine($"Alias: {kvp.Key}, Sum Unit Quantity: {kvp.Value}");
                //}
                //MessageBox.Show(message.ToString());

                /////////  /////////  /////////  /////////
                //Có Tupe gồm 3 phần tử <Element,Alias,Unity> 
                /////////  /////////  /////////  /////////
                ShowResultControl();
                IsResultControlVisible = true;
            });
            CancelCommand = new RelayCommand<QuantifyWindow>((p) => { return true; }, (p) =>
            {
                p.Close();
            });



            foreach (var category in QuantifyModel.CategoriesData)
            {
                category.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Selected")
                    {
                        IsAnyCategorySelected = QuantifyModel.CategoriesData.Any(cat => cat.Selected);
                        IsApply = IsAnyCategorySelected && !string.IsNullOrEmpty(PathString);
                    }
                };
            }
        }

        private void ShowResultControl()
        {
            // Tạo một instance của ResultViewModel với dữ liệu tính toán
            List<ResultItem> results = new List<ResultItem>();
            foreach (var kvp in QuantifyModel.SumByAliasesDictionary)
            {
                results.Add(new ResultItem
                {
                    Alias = kvp.Key,
                    SumUnitQuantity = kvp.Value,
                });
            }
            ResultViewModel.Results = new ObservableCollection<ResultItem>(results);
        }
    }
}
