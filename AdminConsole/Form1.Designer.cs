
using System.Collections.Generic;
using System.Windows.Forms;

namespace AdminConsole
{
    public partial class Form1 : Form
    {
        private Queue<string> queue = new Queue<string>(32);
        private List<string> list = new List<string>();
        private bool isPaused = false;
        private AdminConsole console;
        private ListBox listBox1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        public Form1 (AdminConsole console)
        {
            this.console = console;
            this.errorLabel.AutoSize = false;

        }
        public void errorMessage(string errorMessage)
        {
            if (errorLabel.InvokeRequired)
            {
                errorLabel.Invoke((MethodInvoker)delegate
                {
                    errorLabel.Text = errorMessage;
                });
            }
            else
            {
                errorLabel.Text = errorMessage;
            }
        }
        public void addToList(string str)
        {
            if (!isPaused)
            {
                if (queue.Count >= 32)
                {
                    queue.Dequeue();
                }
                queue.Enqueue(str);


                if (listBox1.InvokeRequired)
                {
                    listBox1.Invoke((MethodInvoker)delegate
                   {

                       listBox1.DataSource = new List<string>(queue);
                   });
                }
                else
                {
                    this.listBox1.DataSource = new List<string>(queue);
                }
            }
        }

        public void setAdminConsole(AdminConsole console)
        {
            this.console = console;

        }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.textField = new System.Windows.Forms.TextBox();
            this.pushBtn = new System.Windows.Forms.Button();
            this.errorLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
           
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(2, -3);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(243, 420);
            this.listBox1.TabIndex = 0;
            // 
            // refreshBtn
            // 
            this.refreshBtn.Location = new System.Drawing.Point(186, 459);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(59, 32);
            this.refreshBtn.TabIndex = 1;
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // textField
            // 
            this.textField.Location = new System.Drawing.Point(2, 414);
            this.textField.Name = "textField";
            this.textField.Size = new System.Drawing.Size(243, 20);
            this.textField.TabIndex = 2;

            // 
            // pushBtn
            // 
            this.pushBtn.Location = new System.Drawing.Point(75, 459);
            this.pushBtn.Name = "pushBtn";
            this.pushBtn.Size = new System.Drawing.Size(105, 32);
            this.pushBtn.TabIndex = 3;
            this.pushBtn.Text = "Push";
            this.pushBtn.UseVisualStyleBackColor = true;
            this.pushBtn.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(127, 437);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(0, 13);
            this.errorLabel.TabIndex = 4;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(2, 459);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 32);
            this.button1.TabIndex = 5;
            this.button1.Text = "Pause";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(247, 503);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.pushBtn);
            this.Controls.Add(this.textField);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.listBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Noponet";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.textField.PreviewKeyDown += new PreviewKeyDownEventHandler(keyPressed) ;
        }

        #endregion

        public ListBox getFeedList()
        {
            return listBox1;
        }

        private Button refreshBtn;
        private TextBox textField;
        private Button pushBtn;
        private Label errorLabel;
        private Button button1;
        
    }
}

