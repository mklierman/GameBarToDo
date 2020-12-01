using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameBarToDo.ViewModels
{
    public class MainViewModel : Observable
    {
        private string _newListName;
        private ListModel _selectedList;
        private ObservableCollection<ListModel> _userLists;
        private XboxGameBarWidget _widget;
        private SQLiteHelper db = new SQLiteHelper();
        private Frame rootFrame = Window.Current.Content as Frame;

        public MainViewModel()
        {
            DeleteListCommand = new RelayCommand<ListModel>(DeleteList);
            NewListCommand = new RelayCommand<string>(AddNewList);
            LoadUserLists();
        }

        public static RelayCommand<ListModel> DeleteListCommand { get; private set; }
        public RelayCommand<string> NewListCommand { get; set; }

        /// <summary>
        /// The name of a new List
        /// </summary>
        public string NewListName
        {
            get { return _newListName; }
            set
            {
                Set(ref _newListName, value);
            }
        }

        /// <summary>
        /// The ListModel object for the item the user clicked on
        /// </summary>
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

        /// <summary>
        /// The collection of all the ListModel objects
        /// </summary>
        public ObservableCollection<ListModel> UserLists
        {
            get { return _userLists; }
            set
            {
                Set(ref _userLists, value);
            }
        }

        /// <summary>
        /// The Xbox Game Bar Widget to be passed around pages
        /// </summary>
        public XboxGameBarWidget Widget
        {
            get { return _widget; }
            set { Set(ref _widget, value); }
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
                else if (value.Trim().Length > 0) //Prevent only spaces from being used as a name
                {
                    db.AddNewListToTable(value);
                    UserLists.Add(db.GetSpecificList(value));
                }
            }
        }

        /// <summary>
        /// Delete a List
        /// </summary>
        /// <param name="list">The List to be deleted</param>
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
                // The user clicked the CloseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }

        /// <summary>
        /// Get all the lists
        /// </summary>
        private void LoadUserLists()
        {
            UserLists = db.GetUserLists();
        }
    }
}
