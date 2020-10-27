using GameBarToDo.Helpers;
using GameBarToDo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBarToDo.ViewModels
{
    public class ListItemsViewModel : Observable
    {
        private ObservableCollection<ListItemModel> _listItems;
        private SQLiteHelper db = new SQLiteHelper();
        private string _listHeader;
        private ListModel _selectedList;
        private string _newListName;
        private string _newListItemName;

        public ListItemsViewModel()
        {
            //db.EraseAllData();
            //db.LoadDummyData();
            //GetListItems();
        }

        private void GetListItems()
        {
            ListItems = db.GetListItems(SelectedList);
        }

        public ObservableCollection<ListItemModel> ListItems
        {
            get { return _listItems; }
            set
            {
                Set(ref _listItems, value);
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
