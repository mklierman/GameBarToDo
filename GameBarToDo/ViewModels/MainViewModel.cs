using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameBarToDo.ViewModels
{
    public class MainViewModel : Observable
    {
        Frame rootFrame = Window.Current.Content as Frame;
        private ObservableCollection<ListModel> _userLists;
        private ObservableCollection<ListModel> _listItems;
        private SQLiteHelper db = new SQLiteHelper();
        private string _listHeader;
        private ListModel _selectedList;
        private string _newListName;
        private string _newListItemName;
        public ICommand NewListCommand { get; set; }
        public ICommand NewListItemCommand { get; set; }
        public MainViewModel()
        {
            //db.EraseAllData();
            //db.LoadDummyData();
            UserLists = db.GetUserLists();
        }

        public ObservableCollection<ListModel> UserLists
        {
            get { return _userLists; }
            set
            {
                Set(ref _userLists, value);
            }
        }

        public ListModel SelectedList
        {
            get { return _selectedList; }
            set
            {
                Set(ref _selectedList, value);
                if (value != null)
                {
                    //Navigate to ListItem page
                    this.rootFrame.Navigate(typeof(ListItemsView), SelectedList);
                }
            }
        }
    }
}
