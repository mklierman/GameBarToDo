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
        public string list_name { get; set; }
        public DateTime created_date { get; set; }
    }
}
