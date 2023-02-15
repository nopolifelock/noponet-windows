
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management;
using System.Net;
using System.Resources;
using System.Windows.Forms;
namespace NoponetInstaller
{
    partial class Form1
    {
        private Image[] frames;
        private int currentFrame = 0;
        private Timer timer = new Timer();
        private Installer installer;
        public Form1(Installer installer)
        {
            this.InitializeComponent();
            Image gif = Resource1.sanic256;
            if (gif != null)
            {
                frames = GetFrames(gif);
                installer.setFrame(this);

                // Set up the timer to update the image in regular intervals
                timer.Interval = 500; // in milliseconds
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            this.installer = installer;

            this.userComboBox.Items.AddRange(Form1.LoadUsers());
        }
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.javaCheckBox = new System.Windows.Forms.CheckBox();
            this.libraryCheckBox = new System.Windows.Forms.CheckBox();
            this.pythonCheckBox = new System.Windows.Forms.CheckBox();
            this.chromeCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.userComboBox = new ClickableComboBox();
            this.installBtn = new System.Windows.Forms.Button();
            this.javaToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.libraryToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pythonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chromeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.Black;
            this.pictureBox.Location = new System.Drawing.Point(12, 28);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(256, 256);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // javaCheckBox
            // 
            this.javaCheckBox.AutoSize = true;
            this.javaCheckBox.Checked = true;
            this.javaCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.javaCheckBox.Location = new System.Drawing.Point(12, 290);
            this.javaCheckBox.Name = "javaCheckBox";
            this.javaCheckBox.Size = new System.Drawing.Size(132, 17);
            this.javaCheckBox.TabIndex = 1;
            this.javaCheckBox.Text = "Make java admin only.";
            this.javaToolTip.SetToolTip(this.javaCheckBox, "Java can be used to bypass the proxy.\nGet off minecraft and touch grass.");
            this.javaCheckBox.UseVisualStyleBackColor = true;
            // 
            // libraryCheckBox
            // 
            this.libraryCheckBox.AutoSize = true;
            this.libraryCheckBox.Checked = true;
            this.libraryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.libraryCheckBox.Location = new System.Drawing.Point(12, 313);
            this.libraryCheckBox.Name = "libraryCheckBox";
            this.libraryCheckBox.Size = new System.Drawing.Size(133, 17);
            this.libraryCheckBox.TabIndex = 2;
            this.libraryCheckBox.Text = "Library computer mode";
            this.libraryToolTip.SetToolTip(this.libraryCheckBox, "Feature that blocks all programs not in program files \nor system32 from accessing" +
        " the internet.");
            this.libraryCheckBox.UseVisualStyleBackColor = true;
            // 
            // pythonCheckBox
            // 
            this.pythonCheckBox.AutoSize = true;
            this.pythonCheckBox.Checked = true;
            this.pythonCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pythonCheckBox.Location = new System.Drawing.Point(156, 290);
            this.pythonCheckBox.Name = "pythonCheckBox";
            this.pythonCheckBox.Size = new System.Drawing.Size(112, 17);
            this.pythonCheckBox.TabIndex = 3;
            this.pythonCheckBox.Text = "Python admin only";
            this.javaToolTip.SetToolTip(this.pythonCheckBox, "Like Java, Python can also bypass the proxy.\nIf you're a programmer, its time to" +
        " take a break.");
            this.pythonCheckBox.UseVisualStyleBackColor = true;
            // 
            // chromeCheckBox
            // 
            this.chromeCheckBox.AutoSize = true;
            this.chromeCheckBox.Checked = true;
            this.chromeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chromeCheckBox.Location = new System.Drawing.Point(156, 313);
            this.chromeCheckBox.Name = "chromeCheckBox";
            this.chromeCheckBox.Size = new System.Drawing.Size(106, 17);
            this.chromeCheckBox.TabIndex = 5;
            this.chromeCheckBox.Text = "Jailbreak chrome";
            this.javaToolTip.SetToolTip(this.chromeCheckBox, "Chrome can easily bypass the proxy with extensions.\nThis feature only allows adbl" +
        "ocker plus and blocks all \nother extensions and browsers.");
            this.chromeCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 342);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Check all of these for a money back guarantee!";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // userComboBox
            // 
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.Location = new System.Drawing.Point(23, 372);
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(230, 21);
            this.userComboBox.TabIndex = 8;
            // 
            // installBtn
            // 
            this.installBtn.Location = new System.Drawing.Point(12, 397);
            this.installBtn.Name = "installBtn";
            this.installBtn.Size = new System.Drawing.Size(250, 41);
            this.installBtn.TabIndex = 9;
            this.installBtn.Text = "Install";
            this.installBtn.UseVisualStyleBackColor = true;
            this.installBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 399);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(250, 39);
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Hide();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(280, 450);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.installBtn);
            this.Controls.Add(this.userComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chromeCheckBox);
            this.Controls.Add(this.pythonCheckBox);
            this.Controls.Add(this.libraryCheckBox);
            this.Controls.Add(this.javaCheckBox);
            this.Controls.Add(this.pictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "NopoNet Installer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Image[] GetFrames(Image gif)
        {
            // Get the frame count and frame dimensions from the Image object
            int frameCount = gif.GetFrameCount(FrameDimension.Time);
            int frameWidth = gif.Width;
            int frameHeight = gif.Height;

            // Create an array to hold the frames
            Image[] frames = new Image[frameCount];

            // Get each frame from the Image object and store it in the array
            for (int i = 0; i < frameCount; i++)
            {
                
                gif.SelectActiveFrame(FrameDimension.Time, i);
                frames[i] = new Bitmap(frameWidth, frameHeight);
                using (Graphics g = Graphics.FromImage(frames[i]))
                {
                    g.DrawImage(gif, new Rectangle(0, 0, frameWidth, frameHeight));
                }
            }

            return frames;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Display the next frame
            pictureBox.Image = frames[currentFrame];
            currentFrame = (currentFrame + 1) % frames.Length;
        }
        public void setLabel(string message)
        {
            this.BackColor = Color.Gold;
            this.label2.Text = message;
            this.Refresh();
        }
        public void setError(string error)
        {
            this.label2.BackColor = Color.Red;
            this.label2.Text = error;
            this.Refresh();
        }
        private Image LoadGIF()
        {
            string url = "https://github.com/nopolifelock/noponet/blob/main/trollpage/sanic.gif?raw=true";

            using (WebClient client = new WebClient())
            {
                byte[] data = client.DownloadData(url);
                using (MemoryStream stream = new MemoryStream(data))
                {
                    Image gif = Image.FromStream(stream);
                    return gif;
                    // Do something with the loaded image (e.g. display it in a PictureBox)
                }
            }
        }
        private static string[] LoadUsers()
        {
            string[] users = null;
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");
                ManagementObjectCollection collection = searcher.Get();
                users = new string[collection.Count];
                int i = 0;
                foreach (ManagementObject obj in collection)
                {
                    users[i++] = obj["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return users;
        }
        public ProgressBar getProgressBar()
        {
            return progressBar1;
        }
        #endregion
        static Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.CheckBox javaCheckBox;
        private System.Windows.Forms.CheckBox libraryCheckBox;
        private System.Windows.Forms.CheckBox pythonCheckBox;
        private System.Windows.Forms.CheckBox chromeCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox userComboBox;
        private System.Windows.Forms.Button installBtn;
        private ToolTip javaToolTip;
        private ToolTip libraryToolTip;
        private ToolTip pythonToolTip;
        private ToolTip chromeToolTip;
        private ProgressBar progressBar1;
    }
    public class ClickableComboBox : ComboBox
    {
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.DroppedDown = true;
            base.OnMouseDown(e);
        }
    }

}

