using Prism.Mvvm;
using PrismRevitProject.Models;

namespace PrismRevitProject.ViewModels
{
    public class DataAnalysisViewModel : BindableBase
    {
        private ProjectModel _projectModel;
        public ProjectModel ProjectModel
        {
            get { return _projectModel; }
            set { SetProperty(ref _projectModel, value); }
        }

        public DataAnalysisViewModel()
        {

            ProjectModel = new ProjectModel();
        }
    }
}
