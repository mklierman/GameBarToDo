using GameBarToDo.Models;
using GameBarToDo.ViewModels;
using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GameBarToDo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
            widget = e.Parameter as XboxGameBarWidget;

            //Hook up events for when the ui is updated.
            if (widget != null)
            {
                widget.SettingsClicked += Widget_SettingsClicked;

                // subscribe for RequestedOpacityChanged events
                widget.RequestedOpacityChanged += Widget_RequestedOpacityChanged;
            }

            //if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            if (e.Parameter != null)
            {
                //ViewModel.Note = (Models.NoteModel)e.Parameter;
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
                BackgroundGrid.Opacity = widget.RequestedOpacity;
            }
            base.OnNavigatedTo(e);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

