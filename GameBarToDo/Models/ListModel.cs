using System;

namespace GameBarToDo.Models
{
    public class ListModel
    {
        public int id { get; set; }
        public string list_name { get; set; }
        public DateTime created_date { get; set; }
        public DateTime last_updated { get; set; }
    }
}
