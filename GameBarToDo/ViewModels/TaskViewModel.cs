using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameBarToDo.ViewModels
{
    public class TaskViewModel : Observable
    {
        private Frame rootFrame = Window.Current.Content as Frame;
        private ObservableCollection<TaskModel> _tasks;
        private SQLiteHelper db = new SQLiteHelper();
        private string _listHeader;
        private ListModel _selectedList;
        private string _newListName;
        private string _newTaskName;
        private TaskModel _selectedTask;
        public RelayCommand BackCommand { get; private set; }
        public static RelayCommand<TaskModel> ItemCheckedCommand { get; private set; }
        public static RelayCommand<TaskModel> DeleteTaskCommand { get; private set; }

        public TaskViewModel()
        {
            BackCommand = new RelayCommand(GoBack);
            ItemCheckedCommand = new RelayCommand<TaskModel>(UpdateTask);
            DeleteTaskCommand = new RelayCommand<TaskModel>(DeleteTask);
        }

        private void DeleteTask(TaskModel task)
        {
            db.DeleteTask(task);
            Tasks.Remove(task);
        }

        private void UpdateTask(TaskModel task)
        {
            db.UpdateTask(task);
        }

        private void GetTasks()
        {
            Tasks = db.GetTasks(SelectedList);
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

        public TaskModel SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                Set(ref _selectedTask, value);
                if (value != null)
                {
                    //Navigate to Note page
                    this.rootFrame.Navigate(typeof(NoteView), SelectedTask);
                }
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
            db.AddNewTask(NewTaskName, SelectedList.id);
            Tasks.Add(db.GetSpecificTask(NewTaskName));
            NewTaskName = "";
        }
    }
}
