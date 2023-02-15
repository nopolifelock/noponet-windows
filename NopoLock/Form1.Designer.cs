
using System.Windows.Forms;

namespace NopoLock
{
    partial class Form1
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

        public void listFiles(string[] files)
        {
            if (fileListBox.InvokeRequired)
            {
                fileListBox.Invoke(new MethodInvoker(() => listFiles(files)));
            }
            else
            {
                fileListBox.Items.Clear();
                fileListBox.Items.AddRange(files);
                fileListBox.TopIndex = fileListBox.Items.Count - 1;

                // Optionally, ensure that the last item is visible by scrolling to it
                fileListBox.SelectedIndex = fileListBox.Items.Count - 1;
                fileListBox.ClearSelected();
                fileListBox.Refresh();

            }
        }

        public void updateConsole(string update)
        {
            if (responseListBox.InvokeRequired)
            {
                responseListBox.Invoke(new MethodInvoker(() => updateConsole(update)));
            }
            else
            {
                responseListBox.Items.Add(update);
                responseListBox.Refresh();
            }
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.fileSeclectBox = new System.Windows.Forms.TextBox();
            this.fileSelectBtn = new System.Windows.Forms.Button();
            this.fileListBox = new System.Windows.Forms.ListBox();
            this.responseListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selectBtn = new System.Windows.Forms.Button();
            this.folderSelectBox = new System.Windows.Forms.TextBox();
            this.copyFileBtn = new System.Windows.Forms.Button();
            this.timeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileSeclectBox
            // 
            this.fileSeclectBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(46)))), ((int)(((byte)(34)))));
            this.fileSeclectBox.Enabled = false;
            this.fileSeclectBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(31)))));
            this.fileSeclectBox.Location = new System.Drawing.Point(13, 13);
            this.fileSeclectBox.Name = "fileSeclectBox";
            this.fileSeclectBox.Size = new System.Drawing.Size(348, 20);
            this.fileSeclectBox.TabIndex = 0;
            // 
            // fileSelectBtn
            // 
            this.fileSelectBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(76)))), ((int)(((byte)(65)))));
            this.fileSelectBtn.ForeColor = System.Drawing.Color.White;
            this.fileSelectBtn.Location = new System.Drawing.Point(368, 13);
            this.fileSelectBtn.Name = "fileSelectBtn";
            this.fileSelectBtn.Size = new System.Drawing.Size(88, 23);
            this.fileSelectBtn.TabIndex = 1;
            this.fileSelectBtn.Text = "Select File";
            this.fileSelectBtn.UseVisualStyleBackColor = false;
            this.fileSelectBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // fileListBox
            // 
            this.fileListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(46)))), ((int)(((byte)(34)))));
            this.fileListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(31)))));
            this.fileListBox.FormattingEnabled = true;
            this.fileListBox.Location = new System.Drawing.Point(12, 55);
            this.fileListBox.Name = "fileListBox";
            this.fileListBox.Size = new System.Drawing.Size(100, 95);
            this.fileListBox.TabIndex = 2;
            this.fileListBox.DoubleClick += new System.EventHandler(this.fileListBox_MouseDoubleClick);
            // 
            // responseListBox
            // 
            this.responseListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(46)))), ((int)(((byte)(34)))));
            this.responseListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(31)))));
            this.responseListBox.FormattingEnabled = true;
            this.responseListBox.Location = new System.Drawing.Point(172, 55);
            this.responseListBox.Name = "responseListBox";
            this.responseListBox.Size = new System.Drawing.Size(284, 95);
            this.responseListBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(46)))), ((int)(((byte)(34)))));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "File List";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(46)))), ((int)(((byte)(34)))));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(169, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Responses";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // selectBtn
            // 
            this.selectBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(76)))), ((int)(((byte)(65)))));
            this.selectBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.selectBtn.Location = new System.Drawing.Point(12, 165);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(75, 23);
            this.selectBtn.TabIndex = 6;
            this.selectBtn.Text = "Select folder";
            this.selectBtn.UseVisualStyleBackColor = false;
            this.selectBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // folderSelectBox
            // 
            this.folderSelectBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(46)))), ((int)(((byte)(34)))));
            this.folderSelectBox.Enabled = false;
            this.folderSelectBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(31)))));
            this.folderSelectBox.Location = new System.Drawing.Point(93, 168);
            this.folderSelectBox.Name = "folderSelectBox";
            this.folderSelectBox.Size = new System.Drawing.Size(363, 20);
            this.folderSelectBox.TabIndex = 7;
            // 
            // copyFileBtn
            // 
            this.copyFileBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(76)))), ((int)(((byte)(65)))));
            this.copyFileBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.copyFileBtn.Location = new System.Drawing.Point(56, 194);
            this.copyFileBtn.Name = "copyFileBtn";
            this.copyFileBtn.Size = new System.Drawing.Size(349, 23);
            this.copyFileBtn.TabIndex = 8;
            this.copyFileBtn.Text = "Copy file to selected folder";
            this.copyFileBtn.UseVisualStyleBackColor = false;
            this.copyFileBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // timeBtn
            // 
            this.timeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(76)))), ((int)(((byte)(65)))));
            this.timeBtn.BackgroundImage = global::NopoLock.Properties.Resources.clockIcon;
            this.timeBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.timeBtn.Location = new System.Drawing.Point(118, 55);
            this.timeBtn.Name = "timeBtn";
            this.timeBtn.Size = new System.Drawing.Size(48, 95);
            this.timeBtn.TabIndex = 9;
            this.timeBtn.UseVisualStyleBackColor = false;
            this.timeBtn.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NopoLock.Properties.Resources.fileLockBg;
            this.ClientSize = new System.Drawing.Size(468, 232);
            this.Controls.Add(this.timeBtn);
            this.Controls.Add(this.copyFileBtn);
            this.Controls.Add(this.folderSelectBox);
            this.Controls.Add(this.selectBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.responseListBox);
            this.Controls.Add(this.fileListBox);
            this.Controls.Add(this.fileSelectBtn);
            this.Controls.Add(this.fileSeclectBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "NopoLock";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fileSeclectBox;
        private System.Windows.Forms.Button fileSelectBtn;
        private System.Windows.Forms.ListBox fileListBox;
        private System.Windows.Forms.ListBox responseListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button selectBtn;
        private System.Windows.Forms.TextBox folderSelectBox;
        private System.Windows.Forms.Button copyFileBtn;
        private Button timeBtn;
    }
}

