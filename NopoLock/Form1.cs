using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NopoLock
{
    public partial class Form1 : Form
    {
        private Client client;
        public Form1()
        {
            InitializeComponent();
            this.client = new Client(this);
            new Thread(client.Run).Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                // User selected a file, do something with it
                string selectedFile = openFileDialog.FileName;
                // for example, display the file name in a label
                this.fileSeclectBox.Text = selectedFile;
                String[] request = { "PUSH", selectedFile };
                this.client.SendRequest(request);

                //selectedFile. = selectedFile;
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select a folder.";
                folderBrowserDialog.ShowNewFolderButton = false;
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // User selected a folder, display the folder path in a label
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    this.folderSelectBox.Text = selectedFolder;
                }
            }
        }
        private void fileListBox_MouseDoubleClick(object sender, EventArgs e)
        {
            // Check if an item in the list is selected
            if (fileListBox.SelectedItem != null)
            {
                // Retrieve the text of the selected item
                string selectedFile = fileListBox.SelectedItem.ToString();

                Form prompt = new Form();
                prompt.Width = 250;
                prompt.Height = 150;
                prompt.Text = "Lock File";
                Label textLabel = new Label() { Left = 20, Top = 18, Text = "Lock minutes" };
                TextBox inputBox = new TextBox() { Left = 20, Top = 42, Width = 200 };
                Button submitButton = new Button() { Text = "Submit", Left = 20, Width = 100, Top = 70 };
                Button cancelButton = new Button() { Text = "Cancel", Left = 130, Width = 100, Top = 70 };

                // Set up the button event handlers
                submitButton.Click += (senderr, ee) => { prompt.DialogResult = DialogResult.OK; };
                cancelButton.Click += (senderr, ee) => { prompt.DialogResult = DialogResult.Cancel; };

                // Add the controls to the form
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(inputBox);
                prompt.Controls.Add(submitButton);
                prompt.Controls.Add(cancelButton);

                // Show the form as a dialog box and get the user's input
                DialogResult result = prompt.ShowDialog();
                if (result == DialogResult.OK)
                {

                    int seconds;
                    if (int.TryParse(inputBox.Text, out seconds))
                    {
                        string[] request = { "SET", selectedFile, inputBox.Text };
                        client.SendRequest(request);
                    }
                    else
                    {
                        // User entered an invalid value, display an error message or take appropriate action
                    }
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItems.Count > 0)
            {
                fileListBox.Refresh();
                string selectedFile = fileListBox.SelectedItems[0].ToString();
                if (folderSelectBox.Text.Length > 0)
                {
                    string[] request = { "CLONE", selectedFile, folderSelectBox.Text };

                    client.SendRequest(request);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItems.Count > 0)
            {
                fileListBox.Refresh();
                string selectedFile = fileListBox.SelectedItems[0].ToString();
                
                string[] request = { "CHECK", selectedFile };
                client.SendRequest(request);
            }
        }


    }
}
