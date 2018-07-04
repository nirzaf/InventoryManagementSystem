using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace InventoryManagementSystem
{
    public partial class PrintPO : Form
    {
        public PrintPO()
        {
            InitializeComponent();
        }

        public static string itemcode, quantity, description, unitvalue, totalvalue, ordernumber, orderdate, supplier, totalamount, suppAddress;

        private void PrintPO_Load(object sender, EventArgs e)
        {
            lblItemCode.Text = itemcode;
            lblQuant.Text = quantity;
            lblDescp.Text = description;
            lblUnitValue.Text = unitvalue;
            lblAmount.Text = totalvalue;
            lblPoNum.Text = ordernumber;
            lblPoDate.Text = orderdate;
            lblGrandTotal.Text = totalamount;
            lblSuppName.Text = supplier;
            lblSuppAddress.Text = suppAddress;
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(toolStrip1);
            CaptureScreen();

            PrintDialog pd = new PrintDialog();
            DialogResult result = pd.ShowDialog();

            if (result == DialogResult.OK)
            {
                printDocument1.Print();
            }
            this.Controls.Add(toolStrip1);
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;
        private void CaptureScreen()
        {
            Graphics mygraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
            mygraphics.ReleaseHdc(dc1);
            memoryGraphics.ReleaseHdc(dc2);
        }
        
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }
    }
}
