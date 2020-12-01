using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using Microsoft.Gaming.XboxGameBar;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameBarToDo.ViewModels
{
    public class NoteViewModel : Observable
    {
        private NoteModel _note;
        private string _noteText;
        private ListModel _selectedList;
        private string _taskHeader;
        private int _taskID;
        private XboxGameBarWidget _widget;
        private readonly SQLiteHelper db = new SQLiteHelper();
        private readonly Frame rootFrame = Window.Current.Content as Frame;

        public NoteViewModel()
        {
            BackCommand = new RelayCommand(GoBack);
        }

        public RelayCommand BackCommand { get; private set; }

        /// <summary>
        /// The NoteModel object for the page
        /// </summary>
        public NoteModel Note
        {
            get => _note;
            set => Set(ref _note, value);
        }

        /// <summary>
        /// The text the user types for the Note
        /// </summary>
        public string NoteText
        {
            get => _noteText;
            set
            {
                Set(ref _noteText, value);
                UpdateNote();
            }
        }

        /// <summary>
        /// The ListModel object associated with the current note
        /// </summary>
        public ListModel SelectedList
        {
            get => _selectedList;
            set => Set(ref _selectedList, value);
        }

        /// <summary>
        /// The Task Name for the node used as the header
        /// </summary>
        public string TaskHeader
        {
            get => _taskHeader;
            set => Set(ref _taskHeader, value);
        }

        /// <summary>
        /// The ID for the Task associated with the current note
        /// </summary>
        public int TaskID
        {
            get => _taskID;
            set => Set(ref _taskID, value);
        }

        /// <summary>
        /// The Xbox Game Bar Widget to be passed around pages
        /// </summary>
        public XboxGameBarWidget Widget
        {
            get => _widget;
            set => Set(ref _widget, value);
        }

        /// <summary>
        /// Returns to the list of Tasks for the current List
        /// </summary>
        public void GoBack()
        {
            List<object> list = new List<object>
                    {
                        SelectedList,
                        Widget
                    };
            rootFrame.Navigate(typeof(ListItemsView), list);
        }
        /// <summary>
        /// Updates the note with the current Note text
        /// </summary>
        private void UpdateNote()
        {
            db.UpdateNote(NoteText, TaskID);
        }
    }
}
