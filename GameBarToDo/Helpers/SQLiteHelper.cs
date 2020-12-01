using GameBarToDo.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Windows.Storage;

namespace GameBarToDo.Helpers
{
    public class SQLiteHelper
    {
        private readonly string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Lists.db");
        private bool tablesCreated = false;

        public SQLiteHelper()
        {
            InitializeDatabase();
        }

        /// <summary>
        /// Create needed tables if they don't already exist
        /// </summary>
        /// <returns>True or False</returns>
        public bool InitializeDatabase()
        {
            try
            {
                ApplicationData.Current.LocalFolder.CreateFileAsync("Lists.db", CreationCollisionOption.OpenIfExists);
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    string createTables = @"PRAGMA foreign_keys = ON;

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

                    return (long)selectCommand.ExecuteScalar() == 1;
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

                    return Convert.ToInt32(selectCommand.ExecuteScalar()) == 1;
                }
            }
            return false;
        }

        /// <summary>
        /// User types in a new list name. We add this to the table here.
        /// </summary>
        /// <param name="listName">List to be added</param>
        /// <returns>Success or failure message as string</returns>
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

        /// <summary>
        /// User types in a new task while inside a list.
        /// </summary>
        /// <param name="taskName">Task to be added</param>
        /// <param name="listID">ListID for Task to be added to</param>
        /// <returns>Success or failure message as string</returns>
        public string AddNewTask(string taskName, int listID)
        {
            if (CheckIfListExistsByID(listID) && taskName != null)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string insertScript = "INSERT INTO list_items (list_ID, item_name, is_complete) VALUES (@listID, @itemName, 0)";

                    SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
                    insertStuff.Parameters.AddWithValue("@itemName", taskName);
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

        /// <summary>
        /// Adds a row to the Item_notes table
        /// </summary>
        /// <param name="noteText">The Note text to be added</param>
        /// <param name="taskID">The taskID the Note should be added to</param>
        /// <returns>A NoteModel object of the newly created note</returns>
        public NoteModel AddNewNoteToItemTable(string noteText, int taskID)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string insertScript = "INSERT INTO Item_notes (item_ID, note) VALUES (@itemID, @noteText)";

                    SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
                    insertStuff.Parameters.AddWithValue("@itemID", taskID);
                    insertStuff.Parameters.AddWithValue("@noteText", noteText);
                    insertStuff.ExecuteNonQuery();
                }
                return GetNote(null, taskID);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a collection of the Main Lists
        /// </summary>
        /// <returns>Collection of ListModel objects</returns>
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

        /// <summary>
        /// Gets a Task by Name
        /// </summary>
        /// <param name="taskName">The name of the Task to get</param>
        /// <returns>A TaskModel object</returns>
        public TaskModel GetSpecificTask(string taskName)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select * from List_Items where item_name = @itemName ORDER BY id desc LIMIT 1;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);

                    command.Parameters.AddWithValue("@itemName", taskName);
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

        /// <summary>
        /// Gets all Tasks for a given List
        /// </summary>
        /// <param name="listName">The name of the list to get Tasks for</param>
        /// <returns>Collection of TaskModel objects</returns>
        public ObservableCollection<TaskModel> GetTasks(ListModel listName)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select * from List_items where list_ID = @list_ID;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
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
                    }
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a NoteModel object by Task name or ID
        /// </summary>
        /// <param name="taskName">Name of Task to get Note for</param>
        /// <param name="taskID">ID of the Task to get Note for</param>
        /// <returns>A NoteModel object</returns>
        public NoteModel GetNote(TaskModel taskName, int taskID = -1)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select * from Item_notes where item_ID = @item_ID;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    if (taskID < 0)
                    {
                        command.Parameters.AddWithValue("@item_ID", taskName.id);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@item_ID", taskID);
                    }
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
                        result = noteModel;
                    }
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Renames a List
        /// </summary>
        /// <param name="listModel">The ListModel object to be renamed</param>
        /// <returns>True or False</returns>
        public bool RenameList(ListModel listModel)
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

        /// <summary>
        /// Renames a Task
        /// </summary>
        /// <param name="taskModel">The Taskmodel object to be renamed</param>
        /// <returns>True or False</returns>
        public bool RenameTask(TaskModel taskModel)
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

        /// <summary>
        /// Updates the content of a note
        /// </summary>
        /// <param name="note">The string content of the note</param>
        /// <param name="TaskID">The ID of the Task containing the note</param>
        /// <returns>Success bool</returns>
        public bool UpdateNote(string note, int TaskID)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string updateScript = "UPDATE Item_notes SET note = @note where item_ID = @TaskID; ";
                    SqliteCommand command = new SqliteCommand(updateScript, db);
                    command.Parameters.AddWithValue("@note", note);
                    command.Parameters.AddWithValue("@TaskID", TaskID);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes a List from the db
        /// </summary>
        /// <param name="listModel">List to be deleted</param>
        /// <returns>Success bool</returns>
        public bool DeleteList(ListModel listModel)
        {
            if (tablesCreated)
            {
                DeleteAllTasks(listModel);
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

        /// <summary>
        /// Deletes a Task from the db
        /// </summary>
        /// <param name="taskModel">Task to be deleted</param>
        /// <returns>Success bool</returns>
        public bool DeleteTask(TaskModel taskModel)
        {
            if (tablesCreated)
            {
                DeleteAllTaskNotes(null, taskModel);
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

        /// <summary>
        /// Deletes a Note from the db
        /// </summary>
        /// <param name="noteModel">Note to be deleted</param>
        /// <returns>Success bool</returns>
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

        //Deletes all Tasks within a list. Needed when trying to delete a list
        private void DeleteAllTasks(ListModel listModel)
        {
            if (tablesCreated)
            {
                DeleteAllTaskNotes(listModel);
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

        //Deletes all notes for a Task or for all Tasks in a list
        private void DeleteAllTaskNotes(ListModel listModel = null, TaskModel taskModel = null)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string deleteScript = "";

                    if (listModel != null)
                    {
                        deleteScript = "DELETE FROM Item_notes WHERE item_id IN (SELECT id FROM List_items WHERE list_id = @id);";
                    }
                    else if (taskModel != null)
                    {
                        deleteScript = "DELETE FROM Item_notes WHERE item_id = @id;";
                    }

                    SqliteCommand command = new SqliteCommand(deleteScript, db);

                    if (listModel != null)
                    {
                        command.Parameters.AddWithValue("@id", listModel.id);
                    }
                    else if (taskModel != null)
                    {
                        command.Parameters.AddWithValue("@id", taskModel.id);
                    }

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
