using System.Linq;

namespace ELEMENTS.Elements
{
    public class TabList<T> : HorizontalGroup<T> where T : TabList<T>
    {
        public TabList(params IElement[] tabItems) : base(tabItems)
        {
            VisualElement.AddToClassList("tab-list");
            var firstTab = tabItems.First();
            var lastTab = tabItems.Last();
            ((TabItem)firstTab).First();
            ((TabItem)lastTab).Last();
        }
    }

    public class TabList : TabList<TabList>
    {
        public TabList(params IElement[] tabItems) : base(tabItems)
        {
        }
    }
}