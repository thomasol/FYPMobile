using System;

namespace FinalYearProject.Mobile.ClickListeners
{
    public interface IExpandCollapseListener
    {
        void OnRecyclerViewItemExpanded(int position);

        void OnRecyclerViewItemCollapsed(int position);
    }
}
