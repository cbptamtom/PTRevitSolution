using AUR_Foundation_Reinforcement.Models;
using AUR_Foundation_Reinforcement.Services;
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
    public class FrammingViewModel : BindableBase
    {
        private ObservableCollection<FrammingModel> framingList;

        public ObservableCollection<FrammingModel> FramingList
        {
            get { return framingList; }
            set { SetProperty(ref framingList, value); }
        }

        public ICommand ShowFrammingsCommand { get; private set; }

        public FrammingViewModel(Autodesk.Revit.DB.Document document)
        {
            RevitService revitService = new RevitService(document);
            List<Element> revitFraming = revitService.GetStructuralFramingFromRevit();

            // Chuyển đổi danh sách Element thành danh sách FramingModel (nếu có)
            List<FrammingModel> framing = revitFraming.Select(element => new FrammingModel
            {
                Name = element.Name,
                Id = element.Id.ToString()
            }).ToList();

            FramingList = new ObservableCollection<FrammingModel>(framing);
            ShowFrammingsCommand = new DelegateCommand(OnShowFramming);
        }

        private void OnShowFramming()
        {
            MessageBox.Show("OnShowFramming");
        }
    }
}
