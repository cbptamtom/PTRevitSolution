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
using OfficeOpenXml.Style;
using Autodesk.Revit.DB;
using System.Text;

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
                    if (parsedValue <= 0)
                    {
                        MessageBox.Show("Start Row value must be greater than 0");
                    }
                    else if (parsedValue >= 11)
                    {
                        MessageBox.Show("Start Row value must be less than 11");
                    }
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
                                if (string.IsNullOrEmpty(cellI.FormulaR1C1))
                                {
                                    if (dataToUpdate.ContainsKey(alias))
                                    {
                                        double unitQtyValue = dataToUpdate[alias];
                                        cellI.Value = unitQtyValue;
                                        cellI.Style.Font.Bold = true;
                                        cellI.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                                    }
                                }
                            }
                        }
                        worksheet.Cells[4, 7].Value = totalAreaFloor;
                        worksheet.Cells[3, 7].Value = projectName;
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
        public void ExportElementsToExcel(List<Element> elementList)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Choose the Excel file location";
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.FileName = "ExportElementParameter.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    FileInfo newFile = new FileInfo(filePath);

                    // Check if the file already exists
                    if (newFile.Exists)
                    {
                        // If the file exists, delete it
                        CloseExcelProcesses();
                        newFile.Delete();
                    }

                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        // Create a dictionary to store elements by Category
                        Dictionary<string, List<Element>> elementsByCategory = new Dictionary<string, List<Element>>();
                        HashSet<string> addedNames = new HashSet<string>(); // To track added names
                                                                            // Group elements by Category
                        foreach (Element element in elementList)
                        {
                            string category = element.Category.Name;
                            string name = element.Name;

                            // Check if the name has already been added to this category
                            if (!addedNames.Contains(name))
                            {
                                if (!elementsByCategory.ContainsKey(category))
                                {
                                    elementsByCategory[category] = new List<Element>();
                                }

                                elementsByCategory[category].Add(element);
                                addedNames.Add(name); // Add the name to the set
                            }
                        }
                        foreach (var categoryGroup in elementsByCategory)
                        {
                            string categoryName = categoryGroup.Key;
                            List<Element> elementsInCategory = categoryGroup.Value;

                            // Create a new sheet for each category
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(categoryName);
                            worksheet.Cells[1, 1].Value = "Element Name";
                            worksheet.Cells[1, 2].Value = "Alias";



                            worksheet.Cells[1, 1, 1, 3].Style.Font.Bold = true;
                            worksheet.Cells[1, 1, 1, 3].Style.Font.Size = 14;
                            worksheet.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            // Start from row 2 (since the first row contains column names)
                            int row = 2;

                            // Add the list of names to the current sheet
                            foreach (Element element in elementsInCategory)
                            {
                                // Lấy giá trị của Alias và Unit Quantity từ các tham số tương ứng trên phần tử
                                Parameter aliasParameter = element.LookupParameter("Alias");
                                Parameter unitQuantityParameter = element.LookupParameter("Unit Quantity");

                                // Kiểm tra nếu tham số tồn tại và có giá trị
                                if (aliasParameter != null && unitQuantityParameter != null
                                    )
                                {
                                    // Lấy giá trị của Alias và Unit Quantity
                                    string aliasValue = aliasParameter.AsString();
                                    string unitQuantityValue = unitQuantityParameter.AsString();

                                    // Đưa giá trị vào tệp Excel
                                    worksheet.Cells[row, 1].Value = element.Name;
                                    worksheet.Cells[row, 2].Value = aliasValue;
                                    worksheet.Cells[row, 3].Value = unitQuantityValue;
                                }
                                else
                                {
                                    // Xử lý khi Alias hoặc Unit Quantity không tồn tại hoặc không có giá trị
                                    // Ở đây, bạn có thể gán giá trị mặc định hoặc thực hiện xử lý khác tùy ý
                                    worksheet.Cells[row, 1].Value = element.Name;
                                    worksheet.Cells[row, 2].Value = "N/A"; // Giá trị mặc định cho Alias
                                    worksheet.Cells[row, 3].Value = "N/A"; // Giá trị mặc định cho Unit Quantity
                                }

                                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                row++;
                                worksheet.Cells.AutoFitColumns();
                            }
                        }

                        // Save the Excel file
                        package.Save();
                        DialogResult result = MessageBox.Show("The Excel file has been created. Do you want to open it?", "Information", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            // Open the Excel file
                            System.Diagnostics.Process.Start(filePath);
                        }
                    }
                }
            }
        }


        public List<ExcelData> ReadExcelData()
        {
            List<ExcelData> excelDataList = new List<ExcelData>();
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Choose the Excel file to open";
                    openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        FileInfo fileInfo = new FileInfo(filePath);

                        using (ExcelPackage package = new ExcelPackage(fileInfo))
                        {
                            foreach (var worksheet in package.Workbook.Worksheets)
                            {
                                int rowCount = worksheet.Dimension.Rows;
                                int columnCount = worksheet.Dimension.Columns;

                                // Duyệt qua từng dòng trong tệp Excel
                                for (int row = 2; row <= rowCount; row++) // Bắt đầu từ hàng thứ 2 vì hàng đầu tiên thường là tiêu đề
                                {
                                    // Đọc dữ liệu từ các cột (trong trường hợp này, chúng ta giả sử có 3 cột)
                                    string elementName = worksheet.Cells[row, 1].Value?.ToString();
                                    string alias = worksheet.Cells[row, 2].Value?.ToString();
                                    string unitQuantity = worksheet.Cells[row, 3].Value?.ToString();

                                    // Xử lý dữ liệu ở đây (ví dụ: thêm vào danh sách hoặc thực hiện các thao tác khác)
                                    // Trong trường hợp này, chúng ta có thể tạo một đối tượng ExcelData và thêm vào danh sách
                                    ExcelData excelData = new ExcelData
                                    {
                                        ElementName = elementName,
                                        Alias = alias,
                                        UnitQuantity = unitQuantity
                                    };

                                    // Thêm excelData vào danh sách hoặc thực hiện các thao tác khác
                                    excelDataList.Add(excelData);
                                }
                            }

                            // Sau khi đọc xong dữ liệu từ tất cả các sheets, bạn có thể thực hiện các thao tác tiếp theo tùy theo nhu cầu của bạn.
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return excelDataList;
        }



        public class ExcelData
        {
            public string ElementName { get; set; }
            public string Alias { get; set; }
            public string UnitQuantity { get; set; }

        }



    }
}
