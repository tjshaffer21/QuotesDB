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
        private string origQuote;
        private string origAuthor;
        private string origLoc;
        private string[] origTags;

        public EditQuote(string quoteString)
        {
            InitializeComponent();

            string[] arg = parseString(quoteString);

            quoteText.Text = arg[0];
            authorText.Text = arg[1].Trim();
            locationText.Text = arg[2].Substring(1);
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

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
