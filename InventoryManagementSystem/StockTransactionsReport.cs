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
    public partial class StockTransactionsReport : Form
    {
        public StockTransactionsReport()
        {
            InitializeComponent();
        }

        private void StockTransactionsReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'StockTransationsDataSet.StockTransactions' table. You can move, or remove it, as needed.
            this.StockTransactionsTableAdapter.Fill(this.StockTransationsDataSet.StockTransactions);

            this.reportViewer1.RefreshReport();
        }
    }
}
