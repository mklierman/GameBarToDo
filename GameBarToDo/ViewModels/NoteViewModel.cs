using GameBarToDo.Helpers;
using GameBarToDo.Models;
using GameBarToDo.Views;
using System;
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
        private TaskModel _selectedTask;

        public RelayCommand BackCommand { get; private set; }

        public NoteViewModel()
        {
            BackCommand = new RelayCommand(GoBack);
            GetNote();
        }

        public void GoBack()
        {
            //Creating a new ListItemsView page, need to fix how it goes back.
            ListModel listModel = db.GetListByID(SelectedTask.list_id);
            this.rootFrame.Navigate(typeof(ListItemsView), listModel);
        }

        private void GetNote()
        {
            if (Note == null && SelectedTask != null)
            {
                Note = db.GetNote(SelectedTask);
            }
        }

        public NoteModel Note
        {
            get { return _note; }
            set
            {
                Set(ref _note, value);
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
                    GetNote();
                    TaskHeader = value.item_name;
                }
            }
        }

        public string NoteText
        {
            get { return _noteText; }
            set
            {
                if (value.Length > 0)
                {

                    Set(ref _noteText, value);
                    Note.note = value;
                    UpsertNote();
                }
                else
                {
                    Set(ref _noteText, value);
                }
            }
        }

        public string TaskHeader
        {
            get { return _taskHeader; }
            set { Set(ref _taskHeader, value); }
        }

        private void CreateNewNote()
        {
            NoteModel nm = db.AddNewNoteToItemTable(NoteText, SelectedTask.id);
            Note = nm;
        }

        private void UpsertNote()
        {
            db.UpsertNote(Note);
        }

    }
}
