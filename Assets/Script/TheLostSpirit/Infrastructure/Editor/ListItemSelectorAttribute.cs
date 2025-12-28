using System;

namespace Script.TheLostSpirit.Infrastructure.Editor
{
    public class ListItemSelectorAttribute : Attribute
    {
        public string SetSelectedMethod;

        public ListItemSelectorAttribute(string setSelectedMethod) {
            SetSelectedMethod = setSelectedMethod;
        }
    }
}