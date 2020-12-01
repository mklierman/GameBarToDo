using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using Microsoft.Gaming.XboxGameBar;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameBarToDo.ViewModels
{
    public class TaskViewModel : Observable
    {
        private string _listHeader;
        private string _newTaskName;
        private ListModel _selectedList;
        private TaskModel _selectedTask;
        private ObservableCollection<TaskModel> _tasks;
        private XboxGameBarWidget _widget;
        private SQLiteHelper db = new SQLiteHelper();
        private Frame rootFrame = Window.Current.Content as Frame;

        public TaskViewModel()
        {
            //Initialize the Relay Commands
            BackCommand        = new RelayCommand(GoBack);
            NewTaskCommand     = new RelayCommand<string>(AddNewTask);
            DeleteTaskCommand  = new RelayCommand<TaskModel>(DeleteTask);
            ItemCheckedCommand = new RelayCommand<TaskModel>(UpdateTask);
        }

        public RelayCommand BackCommand { get; private set; }
        public RelayCommand<string> NewTaskCommand { get; set; }
        public static RelayCommand<TaskModel> DeleteTaskCommand { get; private set; }
        public static RelayCommand<TaskModel> ItemCheckedCommand { get; private set; }

        /// <summary>
        /// The header displayed at the top of the ListItemsView page
        /// </summary>
        public string ListHeader
        {
            get { return _listHeader; }
            set { Set(ref _listHeader, value); }
        }


        /// <summary>
        /// The name for a new Task
        /// </summary>
        public string NewTaskName
        {
            get { return _newTaskName; }
            set
            {
                Set(ref _newTaskName, value);
            }
        }

        /// <summary>
        /// The ListModel object for the currently listed tasks
        /// </summary>
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

        /// <summary>
        /// The Task object that a user clicks on
        /// </summary>
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
                        SelectedList,
                        Widget
                    };
                    this.rootFrame.Navigate(typeof(NoteView), list);
                }
            }
        }

        /// <summary>
        /// The collection of Tasks to list for the user
        /// </summary>
        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set
            {
                Set(ref _tasks, value);
            }
        }

        /// <summary>
        /// Game Bar widget to be passed around as we move pages
        /// </summary>
        public XboxGameBarWidget Widget
        {
            get { return _widget; }
            set { Set(ref _widget, value); }
        }

        /// <summary>
        /// Returns to the Main list page
        /// </summary>
        public void GoBack()
        {
            this.rootFrame.Navigate(typeof(MainPage), Widget);
        }

        private void AddNewTask(string value)
        {
            NewTaskName = "";
            if (value != null && value.Trim().Length > 0)
            {
                db.AddNewTask(value, SelectedList.id);
                Tasks.Add(db.GetSpecificTask(value));
            }
        }

        private void DeleteTask(TaskModel task)
        {
            db.DeleteTask(task);
            Tasks.Remove(task);
        }

        private void GetTasks()
        {
            Tasks = db.GetTasks(SelectedList);
        }

        private void UpdateTask(TaskModel task)
        {
            db.RenameTask(task);

            foreach (TaskModel _task in Tasks)
            {
                if (_task.id == task.id)
                {
                    _task.is_complete = task.is_complete;
                }
            }
        }
    }
}
