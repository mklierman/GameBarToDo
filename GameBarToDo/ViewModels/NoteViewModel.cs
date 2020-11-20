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
    public class NoteViewModel : Observable
    {
        private Frame rootFrame = Window.Current.Content as Frame;
        private NoteModel _note;
        private SQLiteHelper db = new SQLiteHelper();
        private string _taskHeader;
        private string _noteText;
        private int _taskID;
        private XboxGameBarWidget _widget;
        private ListModel _selectedList;

        public RelayCommand BackCommand { get; private set; }

        public NoteViewModel()
        {
            BackCommand = new RelayCommand(GoBack);
        }

        public void GoBack()
        {
            List<object> list = new List<object>
                    {
                        SelectedList,
                        Widget
                    };
            this.rootFrame.Navigate(typeof(ListItemsView), list);
        }
        public XboxGameBarWidget Widget
        {
            get { return _widget; }
            set { Set(ref _widget, value); }
        }

        public NoteModel Note
        {
            get { return _note; }
            set
            {
                Set(ref _note, value);
            }
        }

        public ListModel SelectedList
        {
            get { return _selectedList; }
            set
            {
                Set(ref _selectedList, value);
            }
        }

        public string NoteText
        {
            get { return _noteText; }
            set
            {
                Set(ref _noteText, value);
                UpdateNote();
            }
        }

        public int TaskID
        {
            get { return _taskID; }
            set
            {
                Set(ref _taskID, value);
            }
        }

        public string TaskHeader
        {
            get { return _taskHeader; }
            set { Set(ref _taskHeader, value); }
        }

        private void UpdateNote()
        {
            db.UpdateNote(NoteText ,TaskID);
        }

    }
}
