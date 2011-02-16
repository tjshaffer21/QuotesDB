using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace QuotesDB
{
    public partial class MainWin : Form
    {
        QuotesDB.Sqlite db;

        public MainWin()
        {
            InitializeComponent();

            tagList.SelectedIndexChanged += 
                new EventHandler(tagList_SelectedIndexChanged);
            quotesList.DoubleClick += 
                new EventHandler(quotesList_DoubleClicked);
            
            connect();
            populate_tagList();     // Automatically calls populate_quotesList();
        }

        private void connect()
        {
            bool exists = false;
            string str = "C:\\Users\\Thomas\\Desktop\\test.db";      // CHANGE

            if (File.Exists(str))
            {
                exists = true;
            }

            db = new QuotesDB.Sqlite();
            db.openDatabase(str);

            if (!exists)
            {
                db.createDatabase();
            }
        }

        /// <summary>
        /// Populates tagList with all available tags.
        /// </summary>
        private void populate_tagList()
        {
            DataTable population = db.Get(
                "SELECT * FROM tag_list ORDER BY tag ASC;");

            tagList.Items.Add("All");
            tagList.SetSelected(0, true);

            foreach (DataRow r in population.Rows)
            {
                string str = r[0] + " (" + r[1] + ")";
                tagList.Items.Add(str);
            }
        }

        /// <summary>
        /// Populates quotesList with all available quotes.
        /// </summary>
        private void populate_quotesList(string tag)
        {
            DataTable population = db.createQuotesTable();
            
            if (tag.CompareTo("All") == 0)
            {
                population = db.Get("SELECT * FROM quotes");
            }
            else
            {
                DataTable ids = db.Get("SELECT q_id FROM tags WHERE tag=\"" + 
                    tag + "\"");

                 for(int i = 0; i < ids.Rows.Count; i++) {
                    DataTable q = db.Get("SELECT * FROM quotes WHERE id=" + 
                        ids.Rows[i][0]);
                    
                    population.Rows.Add(q.Rows[0][0], q.Rows[0][1], q.Rows[0][2], 
                        q.Rows[0][3]);
                }
            }

            quotesList.Items.Clear();
            
            for(int i = 0; i < population.Rows.Count; i++)
            {
                string str;

                if (population.Rows[i][3].Equals(""))
                {
                    str = population.Rows[i][2] + " ~ " + population.Rows[i][1]
                        + " " + population.Rows[i][3];
                } else
                {
                    str = population.Rows[i][2] + " ~ " + population.Rows[i][1] 
                        + ", " + population.Rows[i][3];
                }

                quotesList.Items.Add(str);
            }

            numQuotesStatus.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshQuotes()
        {   // Rewrite to refresh properly.
            tagList.Items.Clear();
            populate_tagList();
        }

        /**********************************************************************
         *                             Event Handlers                         *
         *********************************************************************/
        private void tagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tag_selected = tagList.SelectedItem.ToString();
            string tag;

            if (tag_selected.CompareTo("All") == 0)
            {
                tag = tag_selected;
            }
            else
            {
                char[] delimiters = {' '};
                string[] split = tag_selected.Split(delimiters);

                tag = split[0];
            }

            populate_quotesList(tag);
        }

        private void quotesList_DoubleClicked(object sender, EventArgs e)
        {
            object quote = quotesList.SelectedItem;
            if (quote != null)
            {
                new EditQuote(db, quote.ToString()).ShowDialog();
            }

            this.RefreshQuotes();
        }

        private void addQuoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form aq = new AddQuote(db);
            aq.ShowDialog(this);

            this.RefreshQuotes();
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            this.Close();
        }
    }
}
