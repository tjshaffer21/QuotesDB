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
    public partial class AddQuote : Form
    {
        QuotesDB.Sqlite db;

        public AddQuote(QuotesDB.Sqlite db)
        {
            this.db = db;
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            char[] delimiter = { ',' };

            string author = authorText.Text.Trim();
            string loc    = locText.Text.Trim();
            string quote  = quoteText.Text.Trim();
            string[] tags = tagsText.Text.Split(delimiter);

            if (quote.Length > 0)
            {
                if (author.Length == 0)
                {
                    author = "Unknown";
                }

                string sql = "INSERT INTO quotes (author, quotes, loc) VALUES ('" +
                             author + "', '" + quote + "', '" + loc + "');";
                long id = db.Insert<long>(sql);

                for (int i = 0; i < tags.Length; i++)
                {
                    string tag = tags[i].Trim();
                    sql = "SELECT id FROM tags WHERE tag='" + tag + "';";
                    db.Exists(sql);

                    sql = "INSERT INTO tags (q_id, tag) VALUES (" + id + ", '" +
                        tag + "');";
                    db.Insert<long>(sql);

                    sql = "SELECT tag FROM tag_list WHERE tag='" + tag + "';";

                    if (db.Exists(sql))
                    {
                        sql = "UPDATE tag_list SET val=val+1 WHERE tag='" + tag + "';";
                        db.Update(sql);
                    }
                    else
                    {
                        sql = "INSERT INTO tag_list (tag, val) VALUES ('" + tag + "', 1);";
                        db.Insert<long>(sql);
                    }
                }
            }

            this.Close();
        }
    }
}
