using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class ItemReport : Form
    {
        public ItemReport()
        {
            InitializeComponent();
        }

        private void ItemReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'StockInHandDataSet.StockInHand' table. You can move, or remove it, as needed.
            this.StockInHandTableAdapter.Fill(this.StockInHandDataSet.StockInHand);
            this.rvStockInHand.RefreshReport();
        }
    }
}
