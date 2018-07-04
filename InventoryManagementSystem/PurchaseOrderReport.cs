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
    public partial class PurchaseOrderReport : Form
    {
        public PurchaseOrderReport()
        {
            InitializeComponent();
        }

        private void PurchaseOrderReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'PurchaseOrderDetails.PurchaseOrder' table. 
            // You can move, or remove it, as needed.
            POdataSet.EnforceConstraints = false;
            this.Purchase_Order_Details_ProcedureTableAdapter.Fill(this.POdataSet.Purchase_Order_Details_Procedure);
            this.rvPurOrder.RefreshReport();
        }
    }
}
