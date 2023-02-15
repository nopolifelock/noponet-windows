using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminConsole
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
        }
        // refresh
        private void button1_Click(object sender, EventArgs e)
        {
            this.console.send("refresh");
        }

        //push
        private void button1_Click_1(object sender, EventArgs e)
        {
            List<string> selectedItems = new List<string>();
            foreach (var item in listBox1.SelectedItems)
            {
                selectedItems.Add(item.ToString());
            }
            string[] toAdd = selectedItems.ToArray();
            this.console.updateList(toAdd, AdminConsole.WHITELIST_DIR);
        }

        //pause
        private void button1_Click_2(object sender, EventArgs e)
        {
            this.isPaused = !this.isPaused;
            if (this.isPaused)
                this.button1.Text = "Unpause";
            else
                this.button1.Text = "Pause";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void keyPressed(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string[] toAdd = { textField.Text };
                this.console.updateList(toAdd, AdminConsole.KEYWORDS_DIR);
                textField.Text = "";
            }
        }
    }
}
