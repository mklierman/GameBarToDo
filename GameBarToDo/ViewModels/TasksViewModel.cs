using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using System.Linq;

namespace GameBarToDo.ViewModels
{
    public class TasksViewModel : Observable
    {
        private string _listHeader;
        private string _newTaskName;
        private string oldListHeader;
        private string listHeaderPlaceholder;
        private ListModel _selectedList;
        private TaskModel _selectedTask;
        private ObservableCollection<TaskModel> _tasks;
        private XboxGameBarWidget _widget;
        private readonly SQLiteHelper db = new SQLiteHelper();
        private readonly Frame rootFrame = Window.Current.Content as Frame;

        public TasksViewModel()
        {
            //Initialize the Relay Commands
            BackCommand = new RelayCommand(GoBack);
            NewTaskCommand = new RelayCommand<string>(AddNewTask);
            DeleteTaskCommand = new RelayCommand<TaskModel>(DeleteTask);
            TaskCheckedCommand = new RelayCommand<TaskModel>(UpdateTask);
        }

        public RelayCommand BackCommand { get; private set; }
        public RelayCommand<string> NewTaskCommand { get; set; }
        public static RelayCommand<TaskModel> DeleteTaskCommand { get; private set; }
        public static RelayCommand<TaskModel> TaskCheckedCommand { get; private set; }

        /// <summary>
        /// The header displayed at the top of the ListItemsView page
        /// </summary>
        public string ListHeader
        {
            get
            {
                oldListHeader = _listHeader;
                return _listHeader;
            }
            set
            {
                if (_listHeader != null && _listHeader != value)
                {
                    if (db.RenameList(value, SelectedList.id))
                    {
                        Set(ref _listHeader, value);
                    }
                    else
                    {
                        listHeaderPlaceholder = Guid.NewGuid().ToString();
                        Set(ref listHeaderPlaceholder, oldListHeader);
                        ShowRenameListError(value);
                    }
                }
                else
                {
                    Set(ref _listHeader, value);
                }
            }
        }

        private async void ShowRenameListError(string value)
        {
            ContentDialog listAlreadyExistsDialog = new ContentDialog
            {
                Content = string.Format("A list by the name {0} already exists.", value),
                CloseButtonText = "Ok"
            };
            ContentDialogResult result = await listAlreadyExistsDialog.ShowAsync();
        }

        /// <summary>
        /// The name for a new Task
        /// </summary>
        public string NewTaskName
        {
            get => _newTaskName;
            set => Set(ref _newTaskName, value);
        }

        /// <summary>
        /// The ListModel object for the currently listed tasks
        /// </summary>
        public ListModel SelectedList
        {
            get => _selectedList;
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
            get => _selectedTask;
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
                        note = db.AddNewNoteToTable("", SelectedTask.id);
                    }

                    //Navigate to Note page
                    List<object> list = new List<object>
                    {
                        note.note_text,
                        note.task_id,
                        SelectedTask.task_name,
                        SelectedList,
                        Widget
                    };
                    rootFrame.Navigate(typeof(NoteView), list);
                }
            }
        }

        /// <summary>
        /// The collection of Tasks to list for the user
        /// </summary>
        public ObservableCollection<TaskModel> Tasks
        {
            get => _tasks;
            set => Set(ref _tasks, value);
        }

        /// <summary>
        /// Game Bar widget to be passed around as we move pages
        /// </summary>
        public XboxGameBarWidget Widget
        {
            get => _widget;
            set => Set(ref _widget, value);
        }

        /// <summary>
        /// Returns to the Main list page
        /// </summary>
        public void GoBack()
        {
            rootFrame.Navigate(typeof(ListsView), Widget);
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

            //Tasks.Remove(task);

            //int incompleteIndex = Tasks.IndexOf(Tasks.FirstOrDefault(t => t.is_complete == false && task.created_date < t.created_date));
            //int earliestCompleteIndex = Tasks.IndexOf(Tasks.FirstOrDefault(t => t.is_complete == true));

            //if (task.is_complete)
            //{
            //    if (earliestCompleteIndex == -1)
            //        earliestCompleteIndex = Tasks.Count;

            //    Tasks.Insert(earliestCompleteIndex, task);
            //}
            //else
            //{
            //    if (incompleteIndex == -1 || incompleteIndex == 0)
            //        incompleteIndex = Tasks.IndexOf(Tasks.FirstOrDefault(t => t.is_complete == false && t.created_date > task.created_date)) - 1;

            //    if (incompleteIndex == -1)
            //        incompleteIndex = 0;
            //    else if (incompleteIndex == -2)
            //        incompleteIndex = Tasks.Count;
            //    Tasks.Insert(incompleteIndex, task);
            //}

            db.UpdateTask(task);
        }
    }
}

