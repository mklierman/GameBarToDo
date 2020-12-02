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
        private readonly string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "GameBarToDo.db");
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
                _ = ApplicationData.Current.LocalFolder.CreateFileAsync("GameBarToDo.db", CreationCollisionOption.OpenIfExists);
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    string createTables = @"PRAGMA foreign_keys = ON;

                    CREATE TABLE IF NOT EXISTS Lists (
                    id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    list_name TEXT NOT NULL,
                    created_date DEFAULT CURRENT_TIMESTAMP,
                    last_updated DEFAULT CURRENT_TIMESTAMP
                    );

                    CREATE TABLE IF NOT EXISTS Tasks (
                    id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    list_id INTEGER,
                    task_name TEXT NOT NULL,
                    is_complete BIT NOT NULL,
                    created_date DEFAULT CURRENT_TIMESTAMP,
                    last_updated DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY(list_id) REFERENCES Lists(id)
                    );

                    CREATE TABLE IF NOT EXISTS Notes (
                    id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    task_id INTEGER,
                    note_text TEXT NOT NULL,
                    created_date DEFAULT CURRENT_TIMESTAMP,
                    last_updated DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY(task_ID) REFERENCES Tasks(id)
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
                    string insertScript = "INSERT INTO Tasks (list_id, task_name, is_complete) VALUES (@listID, @taskName, 0)";

                    SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
                    insertStuff.Parameters.AddWithValue("@taskName", taskName);
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
        /// Adds a row to the Notes table
        /// </summary>
        /// <param name="noteText">The Note text to be added</param>
        /// <param name="taskID">The taskID the Note should be added to</param>
        /// <returns>A NoteModel object of the newly created note_text</returns>
        public NoteModel AddNewNoteToTable(string noteText, int taskID)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string insertScript = "INSERT INTO Notes (task_id, note_text) VALUES (@taskID, @noteText)";

                    SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
                    insertStuff.Parameters.AddWithValue("@taskID", taskID);
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
                            created_date = Convert.ToDateTime(reader["created_date"]),
                            last_updated = Convert.ToDateTime(reader["last_updated"])
                        };
                        result.Add(listModel);
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
                        listModel.last_updated = Convert.ToDateTime(reader["last_updated"]);
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
                    string selectScript = "Select * from Tasks where task_name = @taskName ORDER BY id desc LIMIT 1;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);

                    command.Parameters.AddWithValue("@taskName", taskName);
                    SqliteDataReader reader = command.ExecuteReader();
                    TaskModel listItemModel = new TaskModel();
                    while (reader.Read())
                    {
                        listItemModel.id = Convert.ToInt32(reader["id"]);
                        listItemModel.task_name = Convert.ToString(reader["task_name"]);
                        listItemModel.created_date = Convert.ToDateTime(reader["created_date"]);
                        listItemModel.list_id = Convert.ToInt32(reader["list_id"]);
                        listItemModel.is_complete = Convert.ToBoolean(reader["is_complete"]);
                        listItemModel.last_updated = Convert.ToDateTime(reader["last_updated"]);
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
                    string selectScript = "Select * from Tasks where list_id = @list_id;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    command.Parameters.AddWithValue("@list_id", listName.id);
                    SqliteDataReader reader = command.ExecuteReader();
                    ObservableCollection<TaskModel> result = new ObservableCollection<TaskModel>();
                    while (reader.Read())
                    {
                        TaskModel listModel = new TaskModel
                        {
                            id = Convert.ToInt32(reader["id"]),
                            task_name = Convert.ToString(reader["task_name"]),
                            created_date = Convert.ToDateTime(reader["created_date"]),
                            is_complete = Convert.ToBoolean(reader["is_complete"]),
                            list_id = Convert.ToInt32(reader["list_id"]),
                            last_updated = Convert.ToDateTime(reader["last_updated"])
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
                    string selectScript = "Select * from Notes where task_id = @task_id;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    if (taskID < 0)
                    {
                        command.Parameters.AddWithValue("@task_id", taskName.id);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@task_id", taskID);
                    }
                    SqliteDataReader reader = command.ExecuteReader();
                    NoteModel result = new NoteModel();
                    while (reader.Read())
                    {
                        NoteModel noteModel = new NoteModel
                        {
                            id = Convert.ToInt32(reader["id"]),
                            task_id = Convert.ToInt32(reader["task_id"]),
                            note_text = Convert.ToString(reader["note_text"]),
                            created_date = Convert.ToDateTime(reader["created_date"]),
                            last_updated = Convert.ToDateTime(reader["last_updated"])
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
        public bool RenameList(string newListName, int ListID)
        {
            if (tablesCreated)
            {
                if (CheckIfListExistsByName(newListName))
                {
                    return false;
                }
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string updateScript = "UPDATE Lists set list_name = @new_name, last_updated = CURRENT_TIMESTAMP where id = @id; ";
                    SqliteCommand command = new SqliteCommand(updateScript, db);
                    command.Parameters.AddWithValue("@new_name", newListName);
                    command.Parameters.AddWithValue("@id", ListID);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
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

        /// <summary>
        /// Renames a Task or marks it complete
        /// </summary>
        /// <param name="taskModel">The Taskmodel object to be renamed or marked complete</param>
        /// <returns>True or False</returns>
        public bool UpdateTask(TaskModel taskModel)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string updateScript = "Update Tasks set task_name = @new_name, is_complete = @is_complete, last_updated = CURRENT_TIMESTAMP where id = @id;";
                    SqliteCommand command = new SqliteCommand(updateScript, db);
                    command.Parameters.AddWithValue("@new_name", taskModel.task_name);
                    command.Parameters.AddWithValue("@is_complete", taskModel.is_complete);
                    command.Parameters.AddWithValue("@id", taskModel.id);
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
        public bool RenameTask(string newTaskName, int TaskID)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string updateScript = "UPDATE Tasks set task_name = @new_name where id = @TaskID; ";
                    SqliteCommand command = new SqliteCommand(updateScript, db);
                    command.Parameters.AddWithValue("@new_name", newTaskName);
                    command.Parameters.AddWithValue("@TaskID", TaskID);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
        }

        /// <summary>
        /// Updates the content of a note_text
        /// </summary>
        /// <param name="note_text">The string content of the note_text</param>
        /// <param name="TaskID">The ID of the Task containing the note_text</param>
        /// <returns>Success bool</returns>
        public bool UpdateNote(string note_text, int TaskID)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string updateScript = "UPDATE Notes SET note_text = @note_text, last_updated = CURRENT_TIMESTAMP where task_id = @TaskID; ";
                    SqliteCommand command = new SqliteCommand(updateScript, db);
                    command.Parameters.AddWithValue("@note_text", note_text);
                    command.Parameters.AddWithValue("@TaskID", TaskID);
                    return Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            return false;
        }

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
                    string deleteScript = "Delete from Tasks where id = @id;";
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
                    string deleteScript = "Delete from Notes where id = @id;";
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
                    string deleteScript = "Delete from Tasks where list_id = @id;";
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
                        deleteScript = "DELETE FROM Notes WHERE task_id IN (SELECT id FROM Tasks WHERE list_id = @id);";
                    }
                    else if (taskModel != null)
                    {
                        deleteScript = "DELETE FROM Notes WHERE task_id = @id;";
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
