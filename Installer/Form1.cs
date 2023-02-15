using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoponetInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void javaCheckBoxChanged(object sender, EventArgs e)
        {

        }

        private void pythonCheckBoxChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            object selected = this.userComboBox.SelectedItem;
            if (selected != null)
            {
                string user = selected.ToString();
                this.installBtn.Hide();
                this.progressBar1.Show();
                this.installer.run(user, javaCheckBox.Checked, pythonCheckBox.Checked, libraryCheckBox.Checked, chromeCheckBox.Checked);
                this.installBtn.Show();
                this.progressBar1.Hide();
            }
            else
            {
                this.setLabel("You must select a user to install to.");
            }

            
        }
    }
}
