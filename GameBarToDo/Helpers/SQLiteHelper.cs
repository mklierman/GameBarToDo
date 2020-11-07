using GameBarToDo.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;


namespace GameBarToDo.Helpers
{
    public class SQLiteHelper
    {
        private string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Lists.db");
        private bool tablesCreated = false;

        public SQLiteHelper()
        {
            InitializeDatabase();
        }
        public bool InitializeDatabase()
        {
            try
            {
                ApplicationData.Current.LocalFolder.CreateFileAsync("Lists.db", CreationCollisionOption.OpenIfExists);
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    String createTables = @"PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Lists (
id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
list_name TEXT NOT NULL,
created_date DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS List_items (
id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
list_ID INTEGER,
item_name TEXT NOT NULL,
is_complete BIT NOT NULL,
created_date DEFAULT CURRENT_TIMESTAMP,
FOREIGN KEY(list_ID) REFERENCES Lists(id)
);

CREATE TABLE IF NOT EXISTS Item_notes (
id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
item_ID INTEGER,
note TEXT NOT NULL,
created_date DEFAULT CURRENT_TIMESTAMP,
FOREIGN KEY(item_ID) REFERENCES List_items(id)
);";

                    SqliteCommand createTable = new SqliteCommand(createTables, db);

                    createTable.ExecuteReader();
                    tablesCreated = true;
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool RunSQLNonQuery(string sqlString, List<SqliteParameter> parameters)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand sqliteCommand = new SqliteCommand(sqlString, db);
                    foreach (SqliteParameter parameter in parameters)
                    {
                        sqliteCommand.Parameters.Add(parameter);
                    }
                    return Convert.ToBoolean(sqliteCommand.ExecuteNonQuery());
                }
            }
            return false;
        }

        //Clear DB
        public void EraseAllData()
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string deleteScript = @"
DROP TABLE Item_notes;
DROP TABLE List_items;
DROP TABLE lists;";

                    SqliteCommand selectCommand = new SqliteCommand(deleteScript, db);
                    selectCommand.ExecuteNonQuery();
                }

                InitializeDatabase();
            }
        }

        //Make sure we don't have repeat lists
        private bool CheckIfListExistsByName(string listName)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select count(*) from Lists where list_name = @listName";


                    SqliteCommand selectCommand = new SqliteCommand(selectScript, db);
                    selectCommand.Parameters.AddWithValue("@listName", listName);

                    return (Int64)selectCommand.ExecuteScalar() == 1;
                }
            }
            return false;
        }

        //Make sure we don't have repeat list IDs
        private bool CheckIfListExistsByID(int listID)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select count(*) from Lists where id = @listID";


                    SqliteCommand selectCommand = new SqliteCommand(selectScript, db);
                    selectCommand.Parameters.AddWithValue("@listID", listID);


                    return Convert.ToInt32 (selectCommand.ExecuteScalar()) == 1;
                }
            }
            return false;
        }

        //User types in a new list name. We add this to the table here.
        public string AddNewListToTable(string listName)
        {
            if (CheckIfListExistsByName(listName))
            {
                return "A list by that name already exists.";
            }
            else if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string insertScript = "INSERT INTO Lists (list_name) VALUES (@listName)";

                    SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
                    insertStuff.Parameters.AddWithValue("@listName", listName);

                    insertStuff.ExecuteNonQuery();

                    return "List created";
                }
            }

            return "Something went wrong";
        }

        //User types in a new task while inside a list.
        public string AddNewTask(string itemName, int listID)
        {
            if (CheckIfListExistsByID(listID))
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string insertScript = "INSERT INTO list_items (list_ID, item_name, is_complete) VALUES (@listID, @itemName, 0)";

                    SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
                    insertStuff.Parameters.AddWithValue("@itemName", itemName);
                    insertStuff.Parameters.AddWithValue("@listID", listID);

                    insertStuff.ExecuteNonQuery();

                    return "Item Added";
                }
            }
            else
            {
                return "That list doesn't exist.";
            }
        }

        //public string AddNewItemToListItemTableByListName(string itemName, string listName)
        //{
        //    if (CheckIfListExistsByName(listName))
        //    {
        //        using (SqliteConnection db =
        //           new SqliteConnection($"Filename={dbpath}"))
        //        {
        //            db.Open();
        //            string insertScript = "INSERT INTO list_items (list_ID, item_name, is_complete) VALUES ((Select id from Lists where list_name = @listName), @itemName, 0)";

        //            SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
        //            insertStuff.Parameters.AddWithValue("@itemName", itemName);
        //            insertStuff.Parameters.AddWithValue("@listName", listName);

        //            insertStuff.ExecuteNonQuery();

        //            return "Item Added";
        //        }
        //    }
        //    else
        //    {
        //        return "That list doesn't exist.";
        //    }
        //}

        private bool CheckIfItemExistsByID(int itemID)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select count(*) from Lists where id = @itemID";


                    SqliteCommand selectCommand = new SqliteCommand(selectScript, db);
                    selectCommand.Parameters.Add(new SqlParameter("@itemID", SqlDbType.Int).Value = itemID);

                    return (int)selectCommand.ExecuteScalar() == 1;
                }
            }
            return false;
        }

        public NoteModel AddNewNoteToItemTable(string noteText, int itemID)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ListModel> GetUserLists()
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select * from Lists;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    SqliteDataReader reader = command.ExecuteReader();
                    ObservableCollection<ListModel> result = new ObservableCollection<ListModel>();
                    while (reader.Read())
                    {
                        ListModel listModel = new ListModel
                        {
                            id = Convert.ToInt32(reader["id"]),
                            list_name = Convert.ToString(reader["list_name"]),
                            created_date = Convert.ToDateTime(reader["created_date"])
                        };
                        result.Add(listModel);

                        //result.Add(Convert.ToString(reader["list_name"]));
                    }
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a ListModel object for the given list name
        /// </summary>
        /// <param name="listName">The name of the list to be returned</param>
        /// <returns>A ListModel object</returns>
        public ListModel GetSpecificList(string listName)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select * from Lists where list_name = @listName LIMIT 1;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    command.Parameters.AddWithValue("@listName", listName);
                    SqliteDataReader reader = command.ExecuteReader();
                    ListModel listModel = new ListModel();
                    while (reader.Read())
                    {
                        listModel.id = Convert.ToInt32(reader["id"]);
                        listModel.list_name = Convert.ToString(reader["list_name"]);
                        listModel.created_date = Convert.ToDateTime(reader["created_date"]);
                    }
                    return listModel;
                }
            }
            return null;
        }

        public ObservableCollection<TaskModel> GetListByID(int id)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select * from List_items where list_ID = @list_ID";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    command.Parameters.AddWithValue("@list_ID", listName.id);
                    SqliteDataReader reader = command.ExecuteReader();
                    ObservableCollection<TaskModel> listModel = new ObservableCollection<TaskModel>();
                    while (reader.Read())
                    {
                        TaskModel listModel = new TaskModel
                        {
                            listModel.id = Convert.ToInt32(reader["id"]);
                            listModel.list_name = Convert.ToString(reader["list_name"]);
                            listModel.created_date = Convert.ToDateTime(reader["created_date"]);
                        }
                        
                    }
                    
                }
            }
            return null;
        }
//"Select count(*) from Lists where id = @itemID";
//CREATE TABLE IF NOT EXISTS List_items(
//id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
//list_ID INTEGER,
//item_name TEXT NOT NULL,
//is_complete BIT NOT NULL,
//created_date DEFAULT CURRENT_TIMESTAMP,
//FOREIGN KEY(list_ID) REFERENCES Lists(id)
//);

        public TaskModel GetSpecificTask(string listItem)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select * from List_Items where item_name = @itemName ORDER BY id desc LIMIT 1;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);

                    command.Parameters.AddWithValue("@itemName", listItem);
                    SqliteDataReader reader = command.ExecuteReader();
                    TaskModel listItemModel = new TaskModel();
                    while (reader.Read())
                    {
                        listItemModel.id = Convert.ToInt32(reader["id"]);
                        listItemModel.item_name = Convert.ToString(reader["item_name"]);
                        listItemModel.created_date = Convert.ToDateTime(reader["created_date"]);
                        listItemModel.list_id = Convert.ToInt32(reader["list_ID"]);
                        listItemModel.is_complete = Convert.ToBoolean(reader["is_complete"]);
                    }
                    return listItemModel;
                }
            }

            return null;
        }


        public ObservableCollection<TaskModel> GetTasks(ListModel listName)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select * from List_items where list_ID = @list_ID;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    //command.Parameters.Add(new SqlParameter("@name", SqlDbType.Text).Value = listName);
                    command.Parameters.AddWithValue("@list_ID", listName.id);
                    SqliteDataReader reader = command.ExecuteReader();
                    ObservableCollection<TaskModel> result = new ObservableCollection<TaskModel>();
                    while (reader.Read())
                    {
                        TaskModel listModel = new TaskModel
                        {
                            id = Convert.ToInt32(reader["id"]),
                            item_name = Convert.ToString(reader["item_name"]),
                            created_date = Convert.ToDateTime(reader["created_date"]),
                            is_complete = Convert.ToBoolean(reader["is_complete"]),
                            list_id = Convert.ToInt32(reader["list_ID"])
                        };
                        result.Add(listModel);

                        //result.Add(Convert.ToString(reader["item_name"]));
                    }
                    return result;
                }
            }
            return null;
        }

        public NoteModel GetNote(TaskModel taskName)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select * from Item_notes where item_ID = @item_ID;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    command.Parameters.AddWithValue("@item_ID", taskName.id);
                    SqliteDataReader reader = command.ExecuteReader();
                    NoteModel result = new NoteModel();
                    while (reader.Read())
                    {
                        NoteModel noteModel = new NoteModel
                        {
                            id = Convert.ToInt32(reader["id"]),
                            item_ID = Convert.ToInt32(reader["item_ID"]),
                            note = Convert.ToString(reader["note"]),
                            created_date = Convert.ToDateTime(reader["created_date"])
                        };
                    }
                    return result;
                }
            }
            return null;
        }

        public bool UpdateList(ListModel listModel)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string updateScript = "Update Lists set list_name = @new_name where id = @id;";
                    SqliteCommand command = new SqliteCommand(updateScript, db);
                    command.Parameters.AddWithValue("@new_name", listModel.list_name);
                    command.Parameters.AddWithValue("@id", listModel.id);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
        }

        public bool UpdateTask(TaskModel taskModel)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string updateScript = "Update List_items set item_name = @new_name, is_complete = @is_complete where id = @id;";
                    SqliteCommand command = new SqliteCommand(updateScript, db);
                    command.Parameters.AddWithValue("@new_name", taskModel.item_name);
                    command.Parameters.AddWithValue("@is_complete", taskModel.is_complete);
                    command.Parameters.AddWithValue("@id", taskModel.id);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
        }

        public bool UpdateItemNote(NoteModel noteModel)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string updateScript = "Update Item_notes set note = @note where id = @id;";
                    SqliteCommand command = new SqliteCommand(updateScript, db);
                    command.Parameters.AddWithValue("@note", noteModel.note);
                    command.Parameters.AddWithValue("@id", noteModel.id);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
        }

        public bool DeleteList(ListModel listModel)
        {
            if (tablesCreated)
            {
                DeleteAllListItems(listModel);
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string deleteScript = "Delete from Lists where id = @id;";
                    SqliteCommand command = new SqliteCommand(deleteScript, db);
                    command.Parameters.AddWithValue("@id", listModel.id);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
        }

        public bool DeleteTask(TaskModel taskModel)
        {
            if (tablesCreated)
            {
                DeleteAllItemNotes(null, taskModel);
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string deleteScript = "Delete from List_items where id = @id;";
                    SqliteCommand command = new SqliteCommand(deleteScript, db);
                    command.Parameters.AddWithValue("@id", taskModel.id);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
        }

        public bool DeleteItemNote(NoteModel noteModel)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string deleteScript = "Delete from Item_notes where id = @id;";
                    SqliteCommand command = new SqliteCommand(deleteScript, db);
                    command.Parameters.AddWithValue("@id", noteModel.id);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
        }

        public void DeleteAllListItems(ListModel listModel)
        {
            if (tablesCreated)
            {
                DeleteAllItemNotes(listModel);
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string deleteScript = "Delete from List_items where list_id = @id;";
                    SqliteCommand command = new SqliteCommand(deleteScript, db);
                    command.Parameters.AddWithValue("@id", listModel.id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAllItemNotes(ListModel listModel = null, TaskModel taskModel = null)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string deleteScript = "";

                    if (listModel != null)
                        deleteScript = "DELETE FROM Item_notes WHERE item_id IN (SELECT id FROM List_items WHERE list_id = @id);";
                    else if (taskModel != null)
                        deleteScript = "DELETE FROM Item_notes WHERE item_id = @id;";

                    SqliteCommand command = new SqliteCommand(deleteScript, db);

                    if (listModel != null)
                        command.Parameters.AddWithValue("@id", listModel.id);
                    else if (taskModel != null)
                        command.Parameters.AddWithValue("@id", taskModel.id);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
