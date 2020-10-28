using System;

using GameBarToDo.ViewModels;
using Microsoft.Gaming.XboxGameBar;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GameBarToDo.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        private XboxGameBarWidget widget = null;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            widget = e.Parameter as XboxGameBarWidget;

            //Hook up events for when the ui is updated.
            if (widget != null)
            {
                widget.SettingsClicked += Widget_SettingsClicked;

                // subscribe for RequestedOpacityChanged events
                widget.RequestedOpacityChanged += Widget_RequestedOpacityChanged;
            }
        }

        private async void Widget_RequestedOpacityChanged(XboxGameBarWidget sender, object args)
        {
            await ContentArea.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // adjust the opacity of your background as appropriate
                ContentArea.Opacity = widget.RequestedOpacity;
            });
        }

        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            await widget.ActivateSettingsAsync();
        }
    }
}
