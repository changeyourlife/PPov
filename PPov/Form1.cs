using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPov
{
    public partial class Form1 : Form
    {
        Form2 form2 = new Form2();
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value += progressBar1.Step;
            if (progressBar1.Value == 100 || progressBar1.Value > 100)
            {
                form2.Show();
                this.Hide();
                timer1.Stop();
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            pictureBox1.Load(Application.StartupPath + @"\pic.jpg");
            //form2.notifyIcon1.Icon.(Application.StartupPath + @"\icon.jpg");
        }
    }
}
