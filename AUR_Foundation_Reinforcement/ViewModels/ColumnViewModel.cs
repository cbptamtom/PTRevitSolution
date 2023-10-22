using AUR_Foundation_Reinforcement.Models;
using AUR_Foundation_Reinforcement.Services;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AUR_Foundation_Reinforcement.ViewModels
{
    public class ColumnViewModel : BindableBase
    {
        private ObservableCollection<ColumnModel> columnList;

        public ObservableCollection<ColumnModel> ColumnList
        {
            get { return columnList; }
            set { SetProperty(ref columnList, value); }
        }

        public ICommand ShowColumnsCommand { get; private set; }

        public ColumnViewModel(Autodesk.Revit.DB.Document document)
        {
            RevitService revitService = new RevitService(document);
            List<Element> revitColumns = revitService.GetStructuralColumnsFromRevit();

            // Chuyển đổi danh sách Element thành danh sách ColumnModel
            List<ColumnModel> columns = revitColumns.Select(element => new ColumnModel
            {
                Name = element.Name,
                Id = element.Id.ToString() // Chuyển đổi Id từ Element thành chuỗi
            }).ToList();

            ColumnList = new ObservableCollection<ColumnModel>(columns);
            ShowColumnsCommand = new DelegateCommand(OnShowColumns);
        }


        private void OnShowColumns()
        {
            MessageBox.Show("OnShowColumns");
            // Triển khai mã để lấy danh sách Columns từ RevitService và cập nhật ObservableCollection Columns
        }
    }
}
