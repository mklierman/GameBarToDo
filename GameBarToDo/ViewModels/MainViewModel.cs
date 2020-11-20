using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using Microsoft.Gaming.XboxGameBar;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

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
        private XboxGameBarWidget _widget;
        public RelayCommand<string> NewListCommand { get; set; }
        public static RelayCommand<ListModel> DeleteListCommand { get; private set; }
        public MainViewModel()
        {
            //db.EraseAllData();
            //db.LoadDummyData();
            DeleteListCommand = new RelayCommand<ListModel>(DeleteList);
            NewListCommand = new RelayCommand<string>(AddNewList);
            LoadUserLists();
        }

        private async void DeleteList(ListModel list)
        {
            ContentDialog deleteConfirmationDialog = new ContentDialog
            {
                Title = String.Format("Delete {0} List", list.list_name),
                Content = String.Format("Are you sure you want to delete the {0} list? This cannot be undone.", list.list_name),
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {

                db.DeleteList(list);
                UserLists.Remove(list);
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }

        public XboxGameBarWidget Widget
        {
            get { return _widget; }
            set { Set(ref _widget, value); }
        }

        private void LoadUserLists()
        {
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
                    List<object> list = new List<object>
                    {
                        SelectedList,
                        Widget
                    };
                    this.rootFrame.Navigate(typeof(ListItemsView), list);
                }
            }
        }

        public string NewListName
        {
            get { return _newListName; }
            set
            {
                if (value.Length > 0 && value.IsLastCharReturn())
                {
                    value = value.RemoveReturnChars();
                    Set(ref _newListName, value);
                    AddNewList(value);
                }
                else
                {
                    Set(ref _newListName, value);
                }
            }
        }

        /// <summary>
        /// Adds the user defined list
        /// </summary>
        /// <param name="value"></param>
        private async void AddNewList(string value)
        {
            if (value != null && value.Length > 0)
            {
                NewListName = "";
                if (UserLists.Any(str => str.list_name.Equals(value)))
                {
                    ContentDialog listAlreadyExistsDialog = new ContentDialog
                    {

                        Content = String.Format("A list by the name {0} already exists.", value),
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await listAlreadyExistsDialog.ShowAsync();
                }
                else
                {
                    db.AddNewListToTable(value);
                    UserLists.Add(db.GetSpecificList(value));
                }
            }
        }


    }
}
