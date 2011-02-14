using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace QuotesDB
{
    public class Sqlite : DataInterface
    {
        SQLiteConnection db;
        public Sqlite() { }

        public bool openDatabase(string name)
        {
            string conn = "Data Source=" + name;

            db = new SQLiteConnection(conn);
            db.Open();

            return true;
        }

        public DataTable Get(string sql)
        {
            DataTable table = new DataTable();

            try
            {
                SQLiteCommand cmds = new SQLiteCommand(sql, db);
                SQLiteDataReader reader = cmds.ExecuteReader();

                table.Load(reader);
                reader.Close();
            }
            catch
            {
            }

            return table;
        }

        /// <summary>
        /// Inserts a row into the table.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>The ID of the row; -1 if failure.</returns>
        public long Insert(string sql)
        {
            long id;

            try
            {
                SQLiteCommand cmds = new SQLiteCommand(sql, db);
                cmds.ExecuteNonQuery();

                SQLiteCommand idCmd = new SQLiteCommand(
                    "SELECT last_insert_rowid();", db);
                id = (long)idCmd.ExecuteScalar();
            }
            catch (SQLiteException ae)
            {
                return -1;
            }

            return id;
        }

        /// <summary>
        /// Checks if a row results from the query.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>True if row exists, else false</returns>
        public bool Exists(string sql)
        {
            SQLiteCommand cmds = new SQLiteCommand(sql, db);
            object id = cmds.ExecuteScalar();

            if (id == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a row exists for the query.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>Returns the id if found, -1 if not</returns>
        public T GetID<T>(string sql)
        {
            SQLiteCommand cmds = new SQLiteCommand(sql, db);
            T id = (T) cmds.ExecuteScalar();

            if (id == null)
            {
                return default(T);
            }

            return id;
        }

        public bool Update(string sql)
        {
            SQLiteCommand cmds = new SQLiteCommand(sql, db);
            cmds.ExecuteNonQuery();

            return true;
        }

        public bool createDatabase()
        {
            int num_cmds = 4;
            string[] sqls = new string[num_cmds];
            sqls[0] = "PRAGMA foreign_keys = ON;";
            sqls[1] = "CREATE TABLE quotes ( id INTEGER PRIMARY KEY AUTOINCREMENT," +
                      "author TEXT NOT NULL DEFAULT ' '," +
                      "quotes TEXT NOT NULL DEFAULT ' '," +
                      "loc TEXT NOT NULL DEFAULT ' ' );";
            sqls[2] = "CREATE TABLE tags ( id INTEGER PRIMARY KEY AUTOINCREMENT," +
                      "q_id INTEGER NOT NULL REFERENCES quotes(id)," +
                      "tag TEXT NOT NULL DEFAULT ' ');";
            sqls[3] = "CREATE TABLE tag_list (tag TEXT PRIMARY KEY, " +
                      "val INTEGER " +
                      "CONSTRAINT tag REFERENCES tags(tag));";

            SQLiteCommand[] cmds = new SQLiteCommand[num_cmds];
            for (int i = 0; i < num_cmds; i++)
            {
                try
                {
                    cmds[i] = new SQLiteCommand(sqls[i], db);

                    int updated = cmds[i].ExecuteNonQuery();
                }
                catch (SQLiteException ae)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Creates a table that contains the columns and types matching the quotes table in the database
        /// </summary>
        /// <returns>DataBase with the quotes table layout.</returns>
        /// <remarks>Necessary to have layout when viewing with tags.</remarks>
        public DataTable createQuotesTable()
        {
            DataTable quotes = new DataTable();
            quotes.Columns.Add("id", typeof(int));
            quotes.Columns.Add("author", typeof(string));
            quotes.Columns.Add("quote", typeof(string));
            quotes.Columns.Add("loc", typeof(string));

            return quotes;
        }

        public bool closeDatabase()
        {
            db.Close();
            return true;
        }
    }
}
