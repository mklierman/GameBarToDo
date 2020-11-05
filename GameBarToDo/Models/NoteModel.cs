using System;

namespace GameBarToDo.Helpers
{
    public class NoteModel : Observable
    {
        private int _id;
        private int _item_ID;
        private string _note;
        private DateTime _created_date;

        public int id
        {
            get { return _id; }
            set
            {
                Set(ref _id, value);
            }
        }

        public int item_ID
        {
            get { return _item_ID; }
            set
            {
                Set(ref _item_ID, value);
            }
        }
        public string note
        {
            get { return _note; }
            set
            {
                Set(ref _note, value);
            }
        }

        public DateTime created_date
        {
            get { return _created_date; }
            set
            {
                Set(ref _created_date, value);
            }
        }
    }
}
