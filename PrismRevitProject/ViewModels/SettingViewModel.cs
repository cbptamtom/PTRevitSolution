using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ImTools;
using OfficeOpenXml;
using Prism.Commands;
using Prism.Mvvm;
using PrismRevitProject.Command;
using PrismRevitProject.Models;
using PrismRevitProject.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;



namespace PrismRevitProject.ViewModels
{
    public class SettingViewModel : BindableBase
    {

        private TransactionGroup _transactionGroup;
        public Application App;
        public UIDocument UiDoc;
        public Document Doc;

        private ElementModel _elementModel;
        public ElementModel ElementModel
        {
            get { return _elementModel; }
            set { SetProperty(ref _elementModel, value); }
        }



        private ObservableCollection<ElementInfo> updatedData;
        public ObservableCollection<ElementInfo> UpdatedData
        {
            get { return updatedData; }
            set { SetProperty(ref updatedData, value); }
        }
        //Command
        public DelegateCommand UpdateDataCommand { get; set; }

        public SettingViewModel()
        {
            //App = app;
            //UiDoc = uiDoc;
            //Doc = doc;
            _transactionGroup = DIContainer.Instance.Resolve<TransactionGroup>();
            Doc = DIContainer.Instance.Resolve<Document>();
            ElementModel = new ElementModel();

            // In here because in this SettingView. Just need show type not show all elements
            ElementModel.ElementInfo = new ObservableCollection<ElementInfo>(
                ElementModel.ElementInfo
                        .GroupBy(e => e.ElementName)
                        .Select(g => g.First())
                        .ToList());
            var collectionView = CollectionViewSource.GetDefaultView(ElementModel.ElementInfo);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            //Command
            UpdateDataCommand = new DelegateCommand(() =>
            {
                try
                {
                    if (Doc != null)
                    {
                        if (!_transactionGroup.HasStarted())
                        {
                            _transactionGroup.Start("a");
                        }

                        if (_transactionGroup.HasStarted())
                        {
                            DocumentService.SetParameterFromElementInfo(Doc, ElementModel.ElementInfo);
                            MessageBox.Show("Update Success!");
                            _transactionGroup.Commit();
                        }
                        else
                        {
                            MessageBox.Show("Transaction has not started.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Document is not available.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            });


        }
        //public void CreateGrid(Document document)
        //{
        //    using (Transaction transaction = new Transaction(document, "Create Grid"))
        //    {
        //        transaction.Start();
        //        try
        //        {
        //            XYZ start = new XYZ(0, 0, 0);
        //            XYZ end = new XYZ(30, 30, 0);

        //            Line geomLine = Line.CreateBound(start, end);

        //            Autodesk.Revit.DB.Grid lineGrid = Autodesk.Revit.DB.Grid.Create(Doc, geomLine);

        //            if (lineGrid != null)
        //            {
        //                lineGrid.Name = "A";
        //                transaction.Commit();
        //                MessageBox.Show("Grid created with name: " + lineGrid.Name);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Failed to create a new grid.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.RollBack();
        //            MessageBox.Show("Error: " + ex.Message);
        //        }
        //    }

        //}

    }



}

