using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
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
        private string _newTaskName;
        private TaskModel _selectedTask;
        private XboxGameBarWidget _widget;
        public RelayCommand BackCommand { get; private set; }
        public static RelayCommand<TaskModel> ItemCheckedCommand { get; private set; }
        public static RelayCommand<TaskModel> DeleteTaskCommand { get; private set; }
        public RelayCommand<string> NewTaskCommand { get; set; }

        public TaskViewModel()
        {
            BackCommand = new RelayCommand(GoBack);
            ItemCheckedCommand = new RelayCommand<TaskModel>(UpdateTask);
            DeleteTaskCommand = new RelayCommand<TaskModel>(DeleteTask);
            NewTaskCommand = new RelayCommand<string>(AddNewTask);
        }

        private void DeleteTask(TaskModel task)
        {
            db.DeleteTask(task);
            Tasks.Remove(task);
        }

        private void UpdateTask(TaskModel task)
        {
            db.UpdateTask(task);

            foreach (TaskModel _task in Tasks)
            {
                if (_task.id == task.id)
                {
                    _task.is_complete = task.is_complete;
                }
            }
        }

        private void GetTasks()
        {
            Tasks = db.GetTasks(SelectedList);
        }

        public void GoBack()
        {
            this.rootFrame.Navigate(typeof(MainPage), Widget);
        }
        public XboxGameBarWidget Widget
        {
            get { return _widget; }
            set { Set(ref _widget, value); }
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
                    //Check if Task has note
                    NoteModel note = db.GetNote(SelectedTask);
                    //If it doesn't have a note, create a blank one
                    if (note.id < 1)
                    {
                        note = db.AddNewNoteToItemTable("", SelectedTask.id);
                    }
                    //Navigate to Note page
                    List<object> list = new List<object>
                    {
                        note.note,
                        note.item_ID,
                        SelectedTask.item_name,
<<<<<<< HEAD
                        SelectedList,
                        Widget

=======
                        SelectedList
>>>>>>> characterLimit
                    };
                    this.rootFrame.Navigate(typeof(NoteView), list);
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
                Set(ref _newTaskName, value);
            }
        }

        public string ListHeader
        {
            get { return _listHeader; }
            set { Set(ref _listHeader, value); }
        }

        private void AddNewTask(string value)
        {
            NewTaskName = "";
            if (value != null && value.Length > 0)
            {
                db.AddNewTask(value, SelectedList.id);
                Tasks.Add(db.GetSpecificTask(value));
            }
        }
    }
}
