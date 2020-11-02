using GameBarToDo.Helpers;
using System;

namespace GameBarToDo.Models
{
    public class TaskModel : Observable
    {
        private int _id;
        private int _list_id;
        private string _item_name;
        private bool _is_complete;
        private DateTime _created_date;

        public int id
        {
            get { return _id; }
            set
            {
                Set(ref _id, value);
            }
        }

        public int list_id
        {
            get { return _list_id; }
            set
            {
                Set(ref _list_id, value);
            }
        }

        public string item_name
        {
            get { return _item_name; }
            set
            {
                Set(ref _item_name, value);
            }
        }

        public bool is_complete
        {
            get { return _is_complete; }
            set
            {
                Set(ref _is_complete, value);
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
