using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuotesDB
{
    public partial class EditQuote : Form
    {
        private Sqlite   db;
        private bool     changed;
        private bool     tChanged;
        private string[] origTags;
        private long     id;

        public EditQuote(Sqlite db, string quoteString)
        {
            InitializeComponent();
 
            this.db      = db;
            changed      = false;
            tChanged     = false;

            string[] arg      = parseString(quoteString);
            quoteText.Text    = arg[0].Trim();
            authorText.Text   = arg[1].Trim();
            locationText.Text = arg[2].Trim();

            string sql = "SELECT id FROM quotes WHERE quotes='" + quoteText.Text + "';";
            id         = db.GetID<long>(sql);

            // Get tags from database and update tag textbox.
            string[] tagList = GetTags();
            origTags         = new string[tagList.Length];
            for (int i = 0; i < tagList.Length; i++)
            {
                origTags[i]   = tagList[i];     // Keep a copy

                if (i + 1 == tagList.Length)
                {
                    tagText.Text += tagList[i];
                }
                else
                {
                    tagText.Text += tagList[i] + ", ";
                }
            }

            authorText.TextChanged += 
                new EventHandler(authorText_TextChanged);
            quoteText.TextChanged += 
                new EventHandler(quoteText_TextChanged);
            locationText.TextChanged += 
                new EventHandler(locationText_TextChanged);
            tagText.TextChanged += 
                new EventHandler(tagText_TextChanged);
        }

        /// <summary>
        /// Obtains list of tags from database.
        /// </summary>
        /// <returns>String array</returns>
        private string[] GetTags()
        {
            string sql     = "SELECT tag FROM tags WHERE q_id=" + id + ";";
            DataTable tags = db.Get(sql);

            string[] tagList = new string[tags.Rows.Count];

            for (int i = 0; i < tags.Rows.Count; i++)
            {
                tagList[i] = tags.Rows[i][0].ToString();
            }

            return tagList;
        }

        /// <summary>
        /// Parses a string and breaks it into its components.
        /// </summary>
        /// <param name="quoteString"></param>
        /// <returns>String array: quote, author, location</returns>
        private string[] parseString(string quoteString)
        {
            char[] delimiters = { '~' };
            string[] vals = new string[3];

            string[] tmp = quoteString.Split(delimiters);
            vals[0] = tmp[0];

            delimiters[0] = ',';
            tmp = tmp[1].Split(delimiters);

            vals[1] = tmp[0];

            if (tmp.Length == 2)
            {
                vals[2] = tmp[1];
            }
            else
            {
                vals[2] = " ";
            }


            return vals;
        }

        /**********************************************************************
         *                          Event Handlers                            *
         *********************************************************************/
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            string sql = "SELECT tag FROM tags WHERE q_id=" + id + ";";
            DataTable tbl = db.Get(sql);

            
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                sql = "UPDATE tag_list SET val=val-1 WHERE tag='" +
                    tbl.Rows[i][0] + "';";
                db.Update(sql);
            }

            db.Delete("tags", "q_id=" + id);
            db.Delete("quotes", "id=" + id);

            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                string author = authorText.Text.Trim();

                if (author.Equals(""))
                {
                    author = "Unknown";
                }

                db.Update("UPDATE quotes SET author='" + author +
                    "', quotes='" + quoteText.Text + "', loc='" +
                    locationText.Text + "' WHERE id=" + id + ";");
            }

            if (tChanged)
            {
                char[] delimiter = { ',' };
                string[] tags = tagText.Text.Split(delimiter);

                string[] deleted = SearchTags(origTags, tags);
                string[] added = SearchTags(tags, origTags);

                for (int i = 0; i < deleted.Length; i++)
                {
                    string sql = "UPDATE tag_list SET val=val-1 WHERE tag='"
                        + deleted[i] + "';";
                    db.Update(sql);

                    db.Delete("tags", "tag='" + deleted[i] + "'");
                }

                for (int i = 0; i < added.Length; i++)
                {
                    string sql = "INSERT INTO tags (q_id, tag) VALUES ( " + id + ", '"
                        + added[i] + "');";

                    db.Insert<long>(sql);
                    
                    sql = "SELECT tag FROM tag_list WHERE tag='" + added[i] + "';";

                    if (db.Exists(sql))
                    {
                        sql = "UPDATE tag_list SET val=val+1 WHERE tag='" + added[i] + "';";
                        db.Update(sql);
                    }
                    else
                    {
                        sql = "INSERT INTO tag_list (tag, val) VALUES ('" + added[i] + "', 1);";
                        db.Insert<long>(sql);
                    }
                }
            }

            this.Close();
        }

        /// <summary>
        /// Searches two list of tags and returns the union.
        /// </summary>
        /// <param name="searchingTags">The array used for search criteria</param>
        /// <param name="searchTags">The array to search through.</param>
        /// <returns>An array of the union.</returns>
        private string[] SearchTags(string[] searchingTags, string[] searchTags)
        {
            bool found;
            List<string> tags = new List<string>();

            for (int i = 0; i < searchingTags.Length; i++)
            {
                found = false;
                for (int j = 0; j < searchTags.Length; j++)
                {
                    if (searchTags[j].Trim().Equals(searchingTags[i].Trim()))
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    tags.Add(searchingTags[i]);
                }
            }

            return tags.ToArray<string>();
        }

        private void authorText_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void locationText_TextChanged(object sender, EventArgs e)
        {  
 	        changed = true;
        }

        private void quoteText_TextChanged(object sender, EventArgs e)
        {   
 	        changed = true;
        }

        private void tagText_TextChanged(object sender, EventArgs e)
        {
            tChanged = true;
        }
    }
}
