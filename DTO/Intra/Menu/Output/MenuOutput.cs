using DTO.Intra.Menu.Enum;
using DTO.Intra.User.Enums;
using System.Collections.Generic;

namespace DTO.Intra.Menu.Output
{
    public class MenuOutput
    {
        public MenuOutput(string name, IconData iconData, string route)
        {
            Name = name;
            IconData = iconData;
            Route = route;
        }

        public MenuOutput(string name, IconData iconData, List<MenuOutput> childrens)
        {
            Name = name;
            IconData = iconData;
            Children = childrens;
        }

        public string Name { get; set; }
        public string Route { get; set; }
        public IconData IconData { get; set; }
        public List<MenuOutput> Children { get; set; }
    }

    public class IconData
    {
        public IconData(string icon, IconTypeEnum type)
        {
            Icon = icon;
            Type = type;
        }
        public string Icon { get; set; }
        public IconTypeEnum Type { get; set; }
    }
}
