using GameBarToDo.Helpers;
using System;

namespace GameBarToDo.Models
{
    public class TaskModel : Observable
    {
        private int _id;
        private int _list_id;
        private string _task_name;
        private bool _is_complete;
        private DateTime _created_date;
        private DateTime _last_updated;

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

        public string task_name
        {
            get => _task_name;
            set => Set(ref _task_name, value);
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
        public DateTime last_updated
        {
            get => _last_updated;
            set => Set(ref _last_updated, value);
        }
    }
}
