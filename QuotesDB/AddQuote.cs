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
        
        /// <summary>
        /// Sanitize data by converting single quote into two single quotes.
        /// </summary>
        /// <param name="s">String to manipulate</param>
        /// <returns>Sanitized string</returns>
        private string ReplaceQuoteChar(string s)
        {
            int pos = s.IndexOf('\'');
            while (pos >= 0)
            {
                s = s.Insert(pos, "'");
                pos = s.IndexOf('\'', pos + 2);
            }

            return s;
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

                if (tags.Length == 1 && tags[0].Equals(""))
                {
                    tags[0] = "Unknown";
                }

                Console.WriteLine(tags[0]);
                author = ReplaceQuoteChar(author);
                loc    = ReplaceQuoteChar(loc);
                quote  = ReplaceQuoteChar(quote);
                
                long id = db.Insert<long>("quotes", "author, quotes, loc",
                    "'" + author + "','" + quote + "','" + loc + "'");

                Console.WriteLine(tags.Length);
                for (int i = 0; i < tags.Length; i++)
                {
                    string tag = tags[i].Trim();
                    Console.WriteLine(tag);
                    db.Insert<long>("tags", "q_id, tag", id + ", '" + tag + "'");

                    string sql = "SELECT tag FROM tag_list WHERE tag='" + tag + "';";

                    if (db.Exists(sql))
                    {
                        sql = "UPDATE tag_list SET val=val+1 WHERE tag='" + 
                            tag + "';";
                        db.Update(sql);
                    }
                    else
                    {
                        db.Insert<long>("tag_list", "tag, val", "'" + tag + 
                            "', 1");
                    }
                }
            }

            this.Close();
        }
    }
}