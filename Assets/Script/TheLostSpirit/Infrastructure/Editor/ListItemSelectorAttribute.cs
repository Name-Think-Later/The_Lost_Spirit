using System;

namespace TheLostSpirit.Infrastructure.Editor
{
    public class ListItemSelectorAttribute : Attribute
    {
        public string SetSelectedMethod;

        public ListItemSelectorAttribute(string setSelectedMethod) {
            this.SetSelectedMethod = setSelectedMethod;
        }
    }
}