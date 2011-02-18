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
    public partial class ManageTags : Form
    {
        private Sqlite db;

        public ManageTags(Sqlite db)
        {
            InitializeComponent();

            this.db = db;
            FillList();
        }

        private void FillList()
        {
            DataTable tags = db.Get(
                "SELECT tag FROM tag_list ORDER BY tag ASC");
            for (int i = 0; i < tags.Rows.Count; i++)
            {
                tagList.Items.Add(tags.Rows[i][0]);
            }
        }

        /**********************************************************************
         *                              Event Handlers                        *
         *********************************************************************/
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tagList.CheckedItems.Count; i++)
            {
                string tag = tagList.CheckedItems[i].ToString();
                db.Delete("tag_list", "tag='" + tag + "'");
                db.Delete("tags", "tag='" + tag + "'");
            }

            tagList.Items.Clear();
            FillList();
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            char[] delimiter = { ',' };
            string[] tags = insertBox.Text.Split(delimiter);

            for (int i = 0; i < tags.Length; i++)
            {
                string sql = "INSERT INTO tag_list (tag, val) VALUES " +
                    "('" + tags[i] + "', 0 )";
                db.Insert<long>(sql);
            }

            tagList.Items.Clear();
            FillList();
        }
    }
}
