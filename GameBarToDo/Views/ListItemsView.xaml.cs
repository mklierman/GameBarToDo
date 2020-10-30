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
    public sealed partial class ListItemsView : Page
    {
        public TaskViewModel ViewModel { get; } = new TaskViewModel();
        private XboxGameBarWidget widget = null;
        public ListItemsView()
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
                ViewModel.SelectedList = (Models.TaskModel)e.Parameter;
            }
            else
            {

            }
            base.OnNavigatedTo(e);
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
