using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace QuotesDB
{
    class Sqlite : DataInterface
    {
        SQLiteConnection db;
        public Sqlite() {}

        public bool openDatabase(string name)
        {
            string conn = "Data Source=" + name;
            Console.WriteLine(conn);
            db = new SQLiteConnection(conn);
            db.Open();
            return true;
        }

        public DataTable Get(string sql)
        {
            DataTable table = new DataTable();

            try
            {
                SQLiteCommand cmds = new SQLiteCommand(db);
                cmds.CommandText = sql;

                SQLiteDataReader reader = cmds.ExecuteReader();

                table.Load(reader);
                reader.Close();
            }
            catch
            {
            }

            return table;
        }

        public bool createDatabase()
        {
            int num_cmds = 3;
            string[] sqls = new string[num_cmds];
            sqls[0] = "CREATE TABLE quotes ( id INTEGER PRIMARY KEY AUTOINCREMENT," +
                      "author TEXT NOT NULL DEFAULT ' '," +
                      "quotes TEXT NOT NULL DEFAULT ' '," +
                      "loc TEXT NOT NULL DEFAULT ' ' );";
            sqls[1] = "CREATE TABLE tags ( id INTEGER PRIMARY KEY AUTOINCREMENT," +
                      "q_id INTEGER NOT NULL REFERENCES quotes(id)," +
                      "tag TEXT NOT NULL DEFAULT ' ');";
            sqls[2] = "CREATE TABLE tag_list (tag TEXT PRIMARY KEY, " +
                      "val INTEGER" +
                      "CONSTRAINT tag REFERENCES tags(tag));";

            SQLiteCommand[] cmds = new SQLiteCommand[num_cmds];
            for(int i = 0; i < num_cmds; i++) {
                try
                {
                    cmds[i] = new SQLiteCommand(sqls[i],db);

                    int updated = cmds[i].ExecuteNonQuery();
                }
                catch(SQLiteException ae)
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
