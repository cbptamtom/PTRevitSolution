#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using QuantifyAUR.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

#endregion

namespace QuantifyAUR
{
    public class QuantifyViewModel : BaseViewModel
    {
        public UIDocument UiDoc;
        public Document Doc;

        private QuantifyModel _quantifyModel;
        public QuantifyModel QuantifyModel { get { return _quantifyModel; } set { _quantifyModel = value; } }
        public ICommand BrowserCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public QuantifyViewModel(UIDocument uiDoc, Document doc)
        {
            UiDoc = uiDoc;
            Doc = doc;
            QuantifyModel = new QuantifyModel(Doc);
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
                    MessageBox.Show($"Selected file: {selectedFilePath}");
                }
            });
            ExportCommand = new RelayCommand<QuantifyWindow>((p) => { return true; }, (p) =>
            {
                QuantifyModel.GetSelectedElements();
                QuantifyModel.GetAliasValues();
                QuantifyModel.GetUnitQuantityValues();
                QuantifyModel.CombineData(QuantifyModel.Elements, QuantifyModel.AliasValues, QuantifyModel.UnitQuantity);




                //StringBuilder message = new StringBuilder();
                //message.AppendLine("Combined Data:");
                //foreach (var data in QuantifyModel.CombinedData)
                //{
                //    message.AppendLine($"Element: {data.Item1}, Alias: {data.Item2}, Unit Quantity: {data.Item3}");
                //}
                //MessageBox.Show(message.ToString());

                /////////  /////////  /////////  /////////
                //Có Tupe gồm 3 phần tử <Element,Alias,Unity> 
                /////////  /////////  /////////  /////////

            });
            CancelCommand = new RelayCommand<QuantifyWindow>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }






    }
}
