using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
namespace QuantifyAUR.Excel
{
    public class ExcelService : BaseViewModel
    {
        private QuantifyViewModel _viewmodel;
        public QuantifyViewModel QViewModel { get { return _viewmodel; } set { _viewmodel = value; OnPropertyChanged(); } }


        private int _startRowDefault = 11;
        private int _startRow = 11;
        public int StartRow
        {
            get { return _startRow; }
            set
            {
                if (int.TryParse(value.ToString(), out int parsedValue) && parsedValue > 0 && parsedValue < 11)
                {
                    _startRow = parsedValue;
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("Start Row value must be under 11");
                    _startRow = _startRowDefault;
                    OnPropertyChanged();

                }
            }
        }

        private string _aliasColumn = "F";
        public string AliasColumn
        {
            get { return _aliasColumn; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _aliasColumn = value;
                }
                OnPropertyChanged();
            }
        }

        private string _outputColumn = "I";
        public string OutputColumn
        {
            get { return _outputColumn; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _outputColumn = value;
                }; OnPropertyChanged();
            }
        }

        public void UpdateExcelFile(string filePath, Dictionary<string, double> dataToUpdate, double totalAreaFloor, string projectName)
        {

            try
            {
                CloseExcelProcesses();
                // Load existing Excel file
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var workbook = package.Workbook;
                    var worksheet = workbook.Worksheets["Inputs"]; // Replace with your worksheet name
                    if (worksheet != null)
                    {

                        for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                        {
                            var cellH = worksheet.Cells[i, ConvertColumnToIndex(AliasColumn)];
                            var cellI = worksheet.Cells[i, ConvertColumnToIndex(OutputColumn)];

                            if (cellH != null && cellI != null)
                            {
                                string alias = cellH.Text;
                                if (dataToUpdate.ContainsKey(alias))
                                {
                                    double unitQtyValue = dataToUpdate[alias];
                                    cellI.Value = unitQtyValue;
                                    cellI.Style.Font.Bold = true;
                                    cellI.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                                }
                            }
                        }
                        worksheet.Cells[4, 7].Value = totalAreaFloor;
                        worksheet.Cells[3, 7].Value = projectName;


                        worksheet.Cells["I8:I" + worksheet.Dimension.End.Row].AutoFilter = true;
                        // To be update





                        package.Save();
                        OpenExcelFile(filePath);
                    }
                    else
                    {
                        MessageBox.Show("Worksheet 'Inputs' not found in the Excel file.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating Excel file: {ex.Message}");
            }

        }
        public void OpenExcelFile(string filePath)
        {
            try
            {
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Excel file: {ex.Message}");
            }
        }

        public void CloseExcelProcesses()
        {
            try
            {
                var processes = Process.GetProcessesByName("EXCEL");
                foreach (var process in processes)
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing Excel processes: {ex.Message}");
            }
        }
        public int ConvertColumnToIndex(string column)
        { // Use for EPPlus lib
            int columnIndex;
            if (int.TryParse(column, out columnIndex))
            {
                return columnIndex;
            }
            else
            {
                char[] letters = column.ToUpper().ToCharArray();
                columnIndex = 0;
                for (int i = 0; i < letters.Length; i++)
                {
                    columnIndex = columnIndex * 26 + (letters[i] - 'A' + 1);
                }
                return columnIndex;
            }
            // If start index is 0, user return columnIndex--
        }




    }
}
