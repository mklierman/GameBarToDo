using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GameBarToDo.Helpers;
using GameBarToDo.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameBarToDo.ViewModels
{
    public class MainViewModel : Observable
    {
        Frame rootFrame = Window.Current.Content as Frame;
        private ObservableCollection<string> _userLists;
        private ObservableCollection<string> _listItems;
        private SQLiteHelper db = new SQLiteHelper();
        private string _listHeader;
        private string _selectedList;
        private string _newListName;
        private string _newListItemName;
        public ICommand NewListCommand { get; set; }
        public ICommand NewListItemCommand { get; set; }
        public MainViewModel()
        {
            db.EraseAllData();
            db.LoadDummyData();
            UserLists = db.GetUserLists();
        }

        public ObservableCollection<string> UserLists
        {
            get { return _userLists; }
            set
            {
                Set(ref _userLists, value);
            }
        }

        public string SelectedList
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
