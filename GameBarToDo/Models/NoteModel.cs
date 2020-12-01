using System;

namespace GameBarToDo.Models
{
    public class NoteModel
    {
        public int id;
        public int task_id;
        public string note_text;
        public DateTime created_date;
        public DateTime last_updated;
    }
}
