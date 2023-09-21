using Autodesk.Revit.DB;

namespace QuantifyAUR.Revit
{
    public class ProjectInformationData
    {
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectAddress { get; set; }

        public ProjectInformationData()
        {
            ProjectName = "";
            ProjectNumber = "";
            ProjectAddress = "";
        }
    }
}
