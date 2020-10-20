using GameBarToDo.Helpers;
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
        private ObservableCollection<string> _listItems;
        private SQLiteHelper db = new SQLiteHelper();
        private string _listHeader;
        private string _selectedList;
        private string _newListName;
        private string _newListItemName;

        public ObservableCollection<string> ListItems
        {
            get { return _listItems; }
            set
            {
                Set(ref _listItems, value);
            }
        }

        public string ListHeader
        {
            get { return _listHeader; }
            set { Set(ref _listHeader, value); }
        }

        public string NewListItemName
        {
            get { return _newListItemName; }
            set
            {
                if (value.IsLastCharReturn())
                {
                    value = value.Remove(value.Length - 1, 1);
                    Set(ref _newListItemName, value);
                    //AddNewListItem();
                }
                else
                {
                    Set(ref _newListItemName, value);
                }
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
                    GetListItems();
                    ListHeader = value;
                }
            }
        }

        private void GetListItems()
        {
            ListItems = db.GetListItems(SelectedList);
        }

        private void AddNewListItem()
        {
            db.AddNewItemToListItemTableByListName(NewListItemName, SelectedList);
            ListItems.Add(NewListItemName);
            NewListItemName = "";
        }
    }
}
