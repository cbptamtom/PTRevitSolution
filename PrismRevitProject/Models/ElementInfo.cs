using Prism.Mvvm;


namespace PrismRevitProject.Models
{

    /// <summary>
    ///  This class is the object of Element has some infomation including here properties
    /// </summary>
    public class ElementInfo : BindableBase
    {

        public string Category { get; set; }
        public string ElementName { get; set; }

        private string _aliasValue;
        public string AliasValue
        {
            get { return _aliasValue; }
            set { SetProperty(ref _aliasValue, value); }
        }

        private string _unitQuantityValue;
        public string UnitQuantityValue
        {
            get { return _unitQuantityValue; }
            set { SetProperty(ref _unitQuantityValue, value); }
        }

    }

}
