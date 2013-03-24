using AgileEAP.Core.Plugins;

namespace AgileEAP.MVC
{
    public interface IMenuProvider
    {
        void BuildMenuItem(MenuItemBuilder menuItemBuilder);
        int Priority { get; }
    }
}
