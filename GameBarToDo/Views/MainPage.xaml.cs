using System;

using GameBarToDo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GameBarToDo.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
