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
            string str = "C:\\Users\\Thomas\\Desktop\\quotes2.db";

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

            long count = 0;
            foreach (DataRow r in population.Rows)
            {
                string str = r[0] + " (" + r[1] + ")";
                tagList.Items.Add(str);

                count += (long)r[1];
            }

            numQuotesStatus.Text = count.ToString() + " quotes";
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
            foreach (DataRow r in population.Rows)
            {
                string str;

                if (r[3].Equals(""))
                {
                    str = r[2] + " ~ " + r[1] + " " + r[3];
                } else
                {
                    str = r[2] + " ~ " + r[1] + ", " + r[3];
                }

                quotesList.Items.Add(str);
            }
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
            string quoteString = quotesList.SelectedItem.ToString();
            
            Form eq = new EditQuote(quoteString);
            eq.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().Show();
        }

        private void addQuoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddQuote().Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            this.Close();
        }
    }
}
