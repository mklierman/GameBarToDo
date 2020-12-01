using GameBarToDo.ViewModels;
using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace GameBarToDo.Views
{
    public sealed partial class TasksView : Page
    {
        public TasksViewModel ViewModel { get; } = new TasksViewModel();
        private XboxGameBarWidget widget = null;

        public TasksView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                List<object> lists = (List<object>)e.Parameter;
                ViewModel.SelectedList = (Models.ListModel)lists[0];
                widget = (XboxGameBarWidget)lists[1];
                ViewModel.Widget = widget;
            }

            //Hook up events for when the ui is updated.
            if (widget != null)
            {
                widget.SettingsClicked += Widget_SettingsClicked;
                widget.RequestedOpacityChanged += Widget_RequestedOpacityChanged;
                widget.GameBarDisplayModeChanged += Widget_GameBarDisplayModeChanged;

                Widget_RequestedOpacityChanged(widget, null);
                Widget_GameBarDisplayModeChanged(widget, null);
            }

            base.OnNavigatedTo(e);
        }

        private async void Widget_GameBarDisplayModeChanged(XboxGameBarWidget sender, object args)
        {
            await NewTaskGrid.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //If the widget is pinned and the overlay isn't on, hide the New Task grid.
                if (sender.GameBarDisplayMode == XboxGameBarDisplayMode.PinnedOnly && sender.Pinned)
                {
                    NewTaskGrid.Height = 0;
                }
                else
                {
                    NewTaskGrid.Height = 40;
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

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.NewTaskCommand.Execute(ViewModel.NewTaskName);
            }
        }
    }
}
