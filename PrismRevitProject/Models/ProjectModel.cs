using Prism.Mvvm;

namespace PrismRevitProject.Models
{
    public class ProjectModel : BindableBase
    {
        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }
        private string _projectID;
        public string ProjectID
        {
            get { return _projectID; }
            set { SetProperty(ref _projectID, value); }
        }
        private string _projectAddress;
        public string ProjectAddress
        {
            get { return _projectAddress; }
            set { SetProperty(ref _projectAddress, value); }
        }
        private bool _hasInittialParameter;
        public bool HasInittialParameter
        {
            get { return _hasInittialParameter; }
            set { SetProperty(ref _hasInittialParameter, value); }
        }
        private bool _hasNoInittialParameter;
        public bool HasNoInittialParameter
        {
            get { return !_hasInittialParameter; }
            set { SetProperty(ref _hasNoInittialParameter, value); }
        }

        private string _homeNotify;
        public string HomeNotify
        {
            get { return _homeNotify; }
            set { SetProperty(ref _homeNotify, value); }
        }

        public ProjectModel()
        {
            ProjectName = string.IsNullOrWhiteSpace(DocumentService.GetProjectName()) ? "Sample Project" : DocumentService.GetProjectName();
            ProjectID = string.IsNullOrWhiteSpace(DocumentService.GetProjectNumber()) ? "ID123456" : DocumentService.GetProjectNumber();
            ProjectAddress = string.IsNullOrWhiteSpace(DocumentService.GetProjectAddress()) ? "VietNam" : DocumentService.GetProjectAddress();
            HasInittialParameter = DocumentService.CheckAllElementsHaveAliasAndUnitQuantity();
            HomeNotify = HasInittialParameter ? "Alias and Quantity parameters are ready!" : "Hit button to initialize Parameters!";
        }

    }
}
