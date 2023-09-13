using Autodesk.Revit.DB;

public class ScheduleObject : RevitSyncExcel.BaseViewModel
{
    private bool _selected;

    public string Name { get; set; }
    public ElementId Id { get; set; }

    public bool Selected
    {
        get { return _selected; }
        set
        {
            if (_selected != value)
            {
                _selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }
    }
}