using GameBarToDo.ViewModels;
using Microsoft.Gaming.XboxGameBar;
using System;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Activation;

namespace GameBarToDo.Views
{
    public sealed partial class ListsView : Page
    {
        private XboxGameBarWidget widget = null;

        public ListsView()
        {
            InitializeComponent();
        }

        public ListsViewModel ViewModel { get; } = new ListsViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            widget = e.Parameter as XboxGameBarWidget;
            ViewModel.Widget = widget;

            //Hook up events for when the ui is updated.
            if (widget != null)
            {
                widget.SettingsClicked += Widget_SettingsClicked;
                widget.RequestedOpacityChanged += Widget_RequestedOpacityChanged;
                widget.GameBarDisplayModeChanged += Widget_GameBarDisplayModeChanged;

                Widget_RequestedOpacityChanged(widget, null);
                Widget_GameBarDisplayModeChanged(widget, null);
            }
        }

        private void TextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.NewListCommand.Execute(ViewModel.NewListName);
            }
        }

        private async void Widget_GameBarDisplayModeChanged(XboxGameBarWidget sender, object args)
        {
            await NewListGrid.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //If the widget is pinned and the overlay isn't on, hide the New List grid.
                if (sender.GameBarDisplayMode == XboxGameBarDisplayMode.PinnedOnly && sender.Pinned)
                {
                    NewListGrid.Height = 0;
                }
                else
                {
                    NewListGrid.Height = 40;
                }
            });
        }

        private async void Widget_RequestedOpacityChanged(XboxGameBarWidget sender, object args)
        {
            await BackgroundGrid.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // adjust the opacity of your background as appropriate
                widget = sender;
                BackgroundGrid.Opacity = widget.RequestedOpacity;
            });
        }

        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            //Changed to sender.Activate() from widget.Activate()
            await widget.ActivateSettingsAsync();
        }
    }
}
