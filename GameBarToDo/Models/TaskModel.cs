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
            get => _id;
            set => Set(ref _id, value);
        }

        public int list_id
        {
            get => _list_id;
            set => Set(ref _list_id, value);
        }

        public string item_name
        {
            get => _item_name;
            set => Set(ref _item_name, value);
        }

        public bool is_complete
        {
            get => _is_complete;
            set => Set(ref _is_complete, value);
        }

        public DateTime created_date
        {
            get => _created_date;
            set => Set(ref _created_date, value);
        }
    }
}
