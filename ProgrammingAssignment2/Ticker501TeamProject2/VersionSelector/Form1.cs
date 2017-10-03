using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VersionSelector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            uxRbGui.Checked = true;
        }

        private void uxBtnStart_Click(object sender, EventArgs e)
        {
            if(uxRbGui.Checked)
            {
                //Run Gui Application
            }
            else
            {
                //Run Console Application
            }



            
            //This might close all other forms as well
            this.Dispose();
            //Check on this later
        }
    }
}
