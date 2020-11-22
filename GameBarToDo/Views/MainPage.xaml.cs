using System;
using System.Diagnostics;
using GameBarToDo.ViewModels;
using Microsoft.Gaming.XboxGameBar;
using Microsoft.Xaml.Interactivity;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
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

        private async void Widget_GameBarDisplayModeChanged(XboxGameBarWidget sender, object args)
        {
            await NewListGrid.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
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
            await widget.ActivateSettingsAsync();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.NewListCommand.Execute(ViewModel.NewListName);
            }
            TextBox textBox = (TextBox)sender;
        }
    }
}
