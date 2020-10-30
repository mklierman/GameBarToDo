using System;

namespace GameBarToDo.Helpers
{
    public class NoteModel
    {
        public int id { get; set; }
        public int item_ID { get; set; }
        public string note { get; set; }
        public DateTime created_date { get; set; }
    }
}
