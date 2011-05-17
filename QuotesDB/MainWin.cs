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
            FillTagList();     // Automatically calls FillQuotesList();
        }

        private void connect()
        {
            bool exists = false;
            string str = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments) + "\\quotes.db";
            
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
        private void FillTagList()
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
        private void FillQuotesList(string tag)
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

        private void RefreshQuotes()
        {
            tagList.Items.Clear();
            FillTagList();
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

            FillQuotesList(tag);
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
            new AddQuote(db).ShowDialog(this);
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

        private void tagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ManageTags(db).ShowDialog(this);
            this.RefreshQuotes();
        }

        private void textCommaDelimitedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog expTxt   = new SaveFileDialog();
            expTxt.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
            expTxt.Filter           = "All (*.*)|*.*|Text (*.txt)|*.txt";
            expTxt.FilterIndex      = 1;

            if (expTxt.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                expTxt.FileName.Length > 0)
            {
                WriteToFile(expTxt.FileName);
            }
        }

        /// <summary>
        /// Write database to a file.
        /// </summary>
        /// <param name="filename">Name of the file</param>
        private void WriteToFile(string filename)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamWriter sr = new StreamWriter(filename))
            {
                string sql = "SELECT * FROM quotes;";
                DataTable tbl = db.Get(sql);

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    sb.Append(tbl.Rows[i][1] + "|" + tbl.Rows[i][2] + 
                        "|" + tbl.Rows[i][3]);
                    sql = "SELECT tag FROM tags WHERE q_id=" + tbl.Rows[i][0]
                        + ";";
                    DataTable tagtbl = db.Get(sql);

                    for (int j = 0; j < tagtbl.Rows.Count; j++)
                    {
                        sb.Append("|" + tagtbl.Rows[j][0]);
                    }
                    sb.AppendLine();
                }

                sr.Write(sb.ToString());
            }
        }

        private void textCommaDelimitedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog impTxt   = new OpenFileDialog();
            impTxt.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
            impTxt.Filter           = "All (*.*)|*.*|Text (*.txt)|*.txt";
            impTxt.FilterIndex      = 1;

            if (impTxt.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                impTxt.FileName.Length > 0)
            {
                ReadFromFile(impTxt.FileName);
                RefreshQuotes();
            }
        }

        private void ReadFromFile(string filename)
        {
            char[] delimiter = { '|' };
            StreamReader sr = File.OpenText(filename);
            string s = sr.ReadLine();

            while (s != null)
            {
                s = ReplaceQuoteChar(s);
                string[] data = s.Split(delimiter);

                long id       = db.Insert<long>("quotes", "author, quotes, loc",
                    "'" + data[0] + "','" + data[1] + "','" + data[2] + "'");

                for (int i = 3; i < data.Length; i++)
                {
                    db.Insert<long>("tags", "q_id, tag", id + ", '" + data[i] + "'");

                    string sql = "SELECT tag FROM tag_list WHERE tag='" + data[i] + "';";

                    if (db.Exists(sql))
                    {
                        sql = "UPDATE tag_list SET val=val+1 WHERE tag='" +
                            data[i] + "';";
                        db.Update(sql);
                    }
                    else
                    {
                        db.Insert<long>("tag_list", "tag, val", "'" + data[i] +
                            "', 1");
                    }
                }

                s = sr.ReadLine();
            }
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
                s   = s.Insert(pos, "'");
                pos = s.IndexOf('\'', pos + 2);
            }

            return s;
        }
    }
}
