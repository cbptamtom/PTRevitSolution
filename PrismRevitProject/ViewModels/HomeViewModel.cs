using Autodesk.Revit.DB;
using Prism.Commands;
using Prism.Mvvm;
using PrismRevitProject.Models;
using System.Windows.Controls;
using System;
using Autodesk.Revit.Creation;
using Unity.Injection;
using System.Linq;

namespace PrismRevitProject.ViewModels
{



    public class HomeViewModel : BindableBase
    {

        private ProjectModel _projectModel;
        public ProjectModel ProjectModel
        {
            get { return _projectModel; }
            set { SetProperty(ref _projectModel, value); }
        }
        BuiltInCategory[] categoriesToInclude = new BuiltInCategory[]{
                BuiltInCategory.OST_StructuralFraming,
                BuiltInCategory.OST_StructuralColumns,
                BuiltInCategory.OST_Floors,
                BuiltInCategory.OST_StructuralFoundation,
                BuiltInCategory.OST_Walls};
        //InitialValueCommand

        public DelegateCommand InitialValueCommand { get; set; }
        public HomeViewModel()
        {
            ProjectModel = new ProjectModel();





            InitialValueCommand = new DelegateCommand(() =>
            {
                try
                {
                    Autodesk.Revit.DB.Document document = DIContainer.Instance.Resolve<Autodesk.Revit.DB.Document>();

                    DefinitionGroup group = DocumentService.CreateSharedParameterGroup(document, "Carbon");

                    ExternalDefinition alias = DocumentService.AddExternalSharedParameterToGroup(document, group, "Alias", SpecTypeId.String.Text);
                    ExternalDefinition unitQuantity = DocumentService.AddExternalSharedParameterToGroup(document, group, "Unit Quantity", SpecTypeId.String.Text);
                    DocumentService.BindSharedParameterToCategories(document, alias, DocumentService.GetCategoriesByBuiltInCategories(categoriesToInclude));
                    DocumentService.BindSharedParameterToCategories(document, unitQuantity, DocumentService.GetCategoriesByBuiltInCategories(categoriesToInclude));
                }
                catch (Exception ex)
                {

                    System.Windows.Forms.MessageBox.Show(ex + "Error in InitialValueCommand");
                }











            });
        }
    }
}
