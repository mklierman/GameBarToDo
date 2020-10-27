using GameBarToDo.ViewModels;
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
        public ListItemsViewModel ViewModel { get; } = new ListItemsViewModel();
        public ListItemsView()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            if(e.Parameter != null)
            {
                ViewModel.SelectedList = (Models.ListModel)e.Parameter;
            }
            else
            {

            }
            base.OnNavigatedTo(e);
        }
    }
}
