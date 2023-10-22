using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace PrismRevitProject.Services
{
    public static class ExcelService
    {
        public static string GetExcelFilePath()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.DefaultExt = ".xlsx";
                saveFileDialog.Title = "Save Excel File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
                return null;
            }
        }
        public static void ExportToExcel(DataTable data, string filePath)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");

                // Đặt tiêu đề cho các cột
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = data.Columns[i].ColumnName;
                }

                // Đổ dữ liệu vào bảng Excel
                for (int row = 0; row < data.Rows.Count; row++)
                {
                    for (int col = 0; col < data.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = data.Rows[row][col];
                    }
                }

                package.Save();
            }
        }
    }
}
