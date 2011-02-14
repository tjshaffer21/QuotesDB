namespace QuotesDB
{
    partial class AddQuote
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.authorText = new System.Windows.Forms.TextBox();
            this.locText = new System.Windows.Forms.TextBox();
            this.tagsText = new System.Windows.Forms.TextBox();
            this.quoteText = new System.Windows.Forms.RichTextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Person:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Location:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Quote:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tags:";
            // 
            // authorText
            // 
            this.authorText.Location = new System.Drawing.Point(63, 13);
            this.authorText.Name = "authorText";
            this.authorText.Size = new System.Drawing.Size(326, 20);
            this.authorText.TabIndex = 4;
            // 
            // locText
            // 
            this.locText.Location = new System.Drawing.Point(63, 40);
            this.locText.Name = "locText";
            this.locText.Size = new System.Drawing.Size(326, 20);
            this.locText.TabIndex = 5;
            // 
            // tagsText
            // 
            this.tagsText.Location = new System.Drawing.Point(63, 127);
            this.tagsText.Name = "tagsText";
            this.tagsText.Size = new System.Drawing.Size(326, 20);
            this.tagsText.TabIndex = 7;
            // 
            // quoteText
            // 
            this.quoteText.Location = new System.Drawing.Point(63, 70);
            this.quoteText.Name = "quoteText";
            this.quoteText.Size = new System.Drawing.Size(326, 51);
            this.quoteText.TabIndex = 6;
            this.quoteText.Text = "";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(314, 174);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(233, 174);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 10;
            this.okButton.Tag = "8";
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // AddQuote
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 209);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.quoteText);
            this.Controls.Add(this.tagsText);
            this.Controls.Add(this.locText);
            this.Controls.Add(this.authorText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddQuote";
            this.Text = "Add Quote";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox authorText;
        private System.Windows.Forms.TextBox locText;
        private System.Windows.Forms.TextBox tagsText;
        private System.Windows.Forms.RichTextBox quoteText;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
    }
}