using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace InventoryManagementSystem
{
    public partial class SplashForm : Form
    {

        int LoadPercent;
        public SplashForm()
        {
            InitializeComponent();

        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            splashTimer1.Enabled = true;
            LoadPercent = 0;
        }

        private void splashTimer1_Tick(object sender, EventArgs e)
        {
            if (LoadPercent < 100)
            {
                LoadPercent += 1;
                lblLoadPerc.Text = LoadPercent.ToString() + "% Loaded";
            }
            else
            {
                splashTimer1.Enabled = false;
                this.Close();
            }

        }
    }
}
