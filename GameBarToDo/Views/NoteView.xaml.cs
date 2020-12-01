using GameBarToDo.Models;
using GameBarToDo.ViewModels;
using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GameBarToDo.Views
{
    public sealed partial class NoteView : Page
    {
        public NoteViewModel ViewModel { get; } = new NoteViewModel();
        private XboxGameBarWidget widget = null;

        public NoteView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                List<object> lists = (List<object>)e.Parameter;
                ViewModel.NoteText = lists[0].ToString();
                ViewModel.TaskID = Convert.ToInt32(lists[1]);
                ViewModel.TaskHeader = lists[2].ToString();
                ViewModel.SelectedList = (ListModel)lists[3];
                widget = (XboxGameBarWidget)lists[4];
                ViewModel.Widget = widget;
            }

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
            //Need to adjust Disabled visual stype
            await NoteTextBox.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (sender.GameBarDisplayMode == XboxGameBarDisplayMode.PinnedOnly && sender.Pinned)
                {
                    NoteTextBox.IsEnabled = false;
                }
                else
                {
                    NoteTextBox.IsEnabled = true;
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
    }
}
