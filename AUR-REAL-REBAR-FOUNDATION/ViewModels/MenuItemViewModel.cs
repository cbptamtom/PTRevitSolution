using System.Windows.Input;

namespace AUR_REAL_REBAR_FOUNDATION.ViewModels
{
    public class MenuItemViewModel
    {
        public string Name { get; }
        public string Icon { get; }
        public ICommand Command { get; }

        public MenuItemViewModel(string name, string icon, ICommand command)
        {
            Name = name;
            Icon = icon;
            Command = command;
        }
    }
}
