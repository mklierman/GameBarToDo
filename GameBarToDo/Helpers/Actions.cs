using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace GameBarToDo.Helpers
{
    internal class Actions
    {
    }

    /// <summary>
    /// Used for R-Click actions
    /// </summary>
    public class OpenFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);

            return null;
        }
    }
}
