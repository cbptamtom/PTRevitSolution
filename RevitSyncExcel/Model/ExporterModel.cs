using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitSyncExcel.Model
{
    public class ExporterModel : BaseViewModel
    {



        private List<ScheduleObject> _ScheduleObjects = new List<ScheduleObject>();
        public List<ScheduleObject> ScheduleObjects { get { return _ScheduleObjects; } set { _ScheduleObjects = value; OnPropertyChanged(); } }

        public ExporterModel(Document document)
        {
            GetViewSchedules(document);
        }

        private void GetViewSchedules(Document document)
        {
            var schedulesList = new FilteredElementCollector(document).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>().ToList();

            foreach (ViewSchedule schedule in schedulesList)
            {
                ScheduleObject data = new ScheduleObject { Name = schedule.Name, Id = schedule.Id, Selected = false };
                ScheduleObjects.Add(data);
            }

            ScheduleObjects = ScheduleObjects.OrderBy(v => v.Name).ToList();
        }
    }
}
