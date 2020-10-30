using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBarToDo.Models
{
    public class TaskModel
    {
        public int id { get; set; }
        public int list_id { get; set; }
        public string item_name { get; set; }
        public bool is_complete { get; set; }
        public DateTime created_date { get; set; }
    }
}
