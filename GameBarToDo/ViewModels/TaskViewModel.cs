using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<TaskModel> _tasks;
        private SQLiteHelper db = new SQLiteHelper();
        private string _listHeader;
        private TaskModel _selectedTask;
        private string _newListName;
        private string _newTaskName;
        public RelayCommand BackCommand { get; private set; }

        public TaskViewModel()
        {
            BackCommand = new RelayCommand(GoBack);
            //db.EraseAllData();
            //db.LoadDummyData();
            //GetListItems();

        }

        private void GetTasks()
        {
            Tasks = db.GetListItems(SelectedList);
        }

        public void GoBack()
        {
            this.rootFrame.Navigate(typeof(MainPage));
        }

        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set
            {
                Set(ref _tasks, value);
            }
        }

        public TaskModel SelectedList
        {
            get { return _selectedTask; }
            set
            {
                Set(ref _selectedTask, value);
                if (value != null)
                {
                    GetTasks();
                    ListHeader = value.list_name;
                }
            }
        }

        public string NewTaskName
        {
            get { return _newTaskName; }
            set
            {
                if (value.Length > 0 && value.IsLastCharReturn())
                {
                    value = value.Remove(value.Length - 1, 1);
                    Set(ref _newTaskName, value);
                    AddNewTask();
                }
                else
                {
                    Set(ref _newTaskName, value);
                }
            }
        }

        public string ListHeader
        {
            get { return _listHeader; }
            set { Set(ref _listHeader, value); }
        }

        private void AddNewTask()
        {
            db.AddNewItemToListItemTable(NewTaskName, SelectedList.id);
            //GetListItems();
            Tasks.Add(db.GetSpecificListItem(NewTaskName));
            NewTaskName = "";
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
