using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameBarToDo.ViewModels
{
    public class TaskViewModel : Observable
    {
        Frame rootFrame = Window.Current.Content as Frame;
        private ObservableCollection<TaskModel> _listItems;
        private SQLiteHelper db = new SQLiteHelper();
        private string _listHeader;
        private ListModel _selectedList;
        private string _newListName;
        private string _newListItemName;
        private TaskModel _selectedItem;
        public RelayCommand BackCommand { get; private set; }
        public static RelayCommand<TaskModel> ItemCheckedCommand { get; private set; }

        public TaskViewModel()
        {
            BackCommand = new RelayCommand(GoBack);
            ItemCheckedCommand = new RelayCommand<TaskModel>(UpdateListItem);
            //db.EraseAllData();
            //db.LoadDummyData();
            //GetListItems();

        }

        private void UpdateListItem(TaskModel listItem)
        {
            db.UpdateListItem(listItem);
        }

        private void GetListItems()
        {
            ListItems = db.GetListItems(SelectedList);
        }

        public void GoBack()
        {
            this.rootFrame.Navigate(typeof(MainPage));
        }

        public ObservableCollection<TaskModel> ListItems
        {
            get { return _listItems; }
            set
            {
                Set(ref _listItems, value);
            }
        }

        public TaskModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                Set(ref _selectedItem, value);
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
                    GetListItems();
                    ListHeader = value.list_name;
                }
            }
        }

        public string NewListItemName
        {
            get { return _newListItemName; }
            set
            {
                if (value.Length > 0 && value.IsLastCharReturn())
                {
                    value = value.Remove(value.Length - 1, 1);
                    Set(ref _newListItemName, value);
                    AddNewListItem();
                }
                else
                {
                    Set(ref _newListItemName, value);
                }
            }
        }

        public string ListHeader
        {
            get { return _listHeader; }
            set { Set(ref _listHeader, value); }
        }

        private void AddNewListItem()
        {
            db.AddNewItemToListItemTable(NewListItemName, SelectedList.id);
            //GetListItems();
            ListItems.Add(db.GetSpecificListItem(NewListItemName));
            NewListItemName = "";
        }

        ///// <summary>
        ///// Adds the user defined list
        ///// </summary>
        ///// <param name="value"></param>
        //private void Addnewlistitem(string value)
        //{
        //    //need to pass an id as well
        //    db.AddNewItemToListItemTable(value);
        //    ListItems.Add(db.GetListItems(value));
        //    NewListItemName = "";
        //}
    }
}
