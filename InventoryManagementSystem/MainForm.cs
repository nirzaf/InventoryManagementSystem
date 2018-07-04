using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryManagementSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
        SqlDataAdapter adp;
        DataSet ds = new DataSet();
        public static string ItemCode;

        public void GetData()
        {
            adp = new SqlDataAdapter("Select * From StockInHand", con);
            ds.Clear();
            adp.Fill(ds, "StockInHand");
        }

        public void fillItems()
        {
            SqlDataAdapter adp = new SqlDataAdapter("Select ItemCode from Items", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "Items");
            cbFindItem.Items.Clear();
            foreach (DataRow dr in ds.Tables["Items"].Rows)
            {
                cbFindItem.Items.Add(dr["ItemCode"]);
            }
        }

        public void fillDGV()
        {
            dgvItems.DataSource = ds.Tables[0].DefaultView;
            dgvItems.Columns[0].HeaderText = "Item Code";
            dgvItems.Columns[1].HeaderText = "Description";
            dgvItems.Columns[2].HeaderText = "Location";
            dgvItems.Columns[3].HeaderText = "Rate";
            dgvItems.Columns[4].HeaderText = "Quantity";
        }
    

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            SideBar.Height = this.Height - 163;
            dgvItems.Width = this.Width - 240;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GetData();
            fillDGV();
            tvItems.ExpandAll();
            fillItems();
        }

        private void newItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Items it = new Items();
            it.ShowDialog();
            GetData();
            fillItems();
            it.BringToFront();
        }

        private void toolStripNewItem_Click(object sender, EventArgs e)
        {
            Items it = new Items();
            it.ShowDialog();
            GetData();
            fillItems();
            it.BringToFront();

        }

        private void toolStripLocations_Click(object sender, EventArgs e)
        {
            Locations lc = new Locations();
            do
            {
                lc.ShowDialog();
                GetData();
                fillDGV();
            }
            while (Locations.act == "Edit");

            fillItems();
        }

        private void newLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddLocation al = new AddLocation();
            al.ShowDialog();
            GetData();
            fillItems();

        }

        private void goToLocationListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Locations lc = new Locations();
            lc.ShowDialog();
            GetData();
            fillItems();
        }

        private void toolStripSuppliers_Click(object sender, EventArgs e)
        {
            SupplierDetails sd = new SupplierDetails();
            sd.ShowDialog();
            GetData();
            fillItems();
        }

        private void tvItems_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvItems.SelectedNode.Text == "Add New Item")
            {
                Items it = new Items();
                it.ShowDialog();
                GetData();
                fillItems();
                tvItems.CollapseAll();
                tvItems.ExpandAll();
                return;
            }
            
            if (tvItems.SelectedNode.Text == "Order Stock")
            {
                NewPurchaseOrder nps = new NewPurchaseOrder();
                nps.ShowDialog();
                GetData();
                fillItems();
                tvItems.CollapseAll();
                tvItems.ExpandAll();
                return;
            }
            if (tvItems.SelectedNode.Text == "Receive Stock")
            {
                ReceiveStock rs = new ReceiveStock();
                do
                {
                    rs.ShowDialog();
                    GetData();
                    fillItems();
                }
                while (ReceiveStock.act == "Receive");
                tvItems.CollapseAll();
                tvItems.ExpandAll();
                return;
            }
            if (tvItems.SelectedNode.Text == "Sell Stock")
            {
                SellStock ss = new SellStock();
                do
                {
                    ss.ShowDialog();
                    GetData();
                    fillItems();
                }
                while (SellStock.act == "Sell");
                tvItems.CollapseAll();
                tvItems.ExpandAll();
                return;
            }
            if (tvItems.SelectedNode.Text == "Transfer Stock")
            {
                TransferStock ts = new TransferStock();
                do
                {
                    GetData();
                    fillDGV();
                    ts.ShowDialog();
                }
                while (TransferStock.act == "Transfer");
                
                fillItems();
                tvItems.CollapseAll();
                tvItems.ExpandAll();
                return;
            }
            if (tvItems.SelectedNode.Text == "Orders List")
            {
                OrdersList ol = new OrdersList();
                ol.ShowDialog();
                GetData();
                fillItems();
                tvItems.CollapseAll();
                tvItems.ExpandAll();
                return;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ItemReport ir = new ItemReport();
            ir.ShowDialog();
            GetData();
            fillItems();
            return;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (cbFindItem.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an item to find");
                return;
            }
            ItemCode = cbFindItem.Text;
            FindItem fi = new FindItem();
            fi.ShowDialog();
            cbFindItem.Text = "";
            GetData();
            fillItems();
        }

        private void toolStripPoDetails_Click(object sender, EventArgs e)
        {
            PurchaseOrderReport pod = new PurchaseOrderReport();
            pod.ShowDialog();
            GetData();
            fillItems();
        }

        private void exitCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void orderStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewPurchaseOrder nps = new NewPurchaseOrder();
            nps.ShowDialog();
            GetData();
            fillItems();
        }

        private void receiveStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReceiveStock rs = new ReceiveStock();
            do
            {
                rs.ShowDialog();
                GetData();
                fillItems();
            }
            while (ReceiveStock.act == "Receive");
        }

        private void sellStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SellStock ss = new SellStock();
            do
            {
                ss.ShowDialog();
                GetData();
                fillItems();
            }
            while (SellStock.act == "Sell");            
        }

        private void transferStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransferStock ts = new TransferStock();
            do
            {
                GetData();
                fillDGV();
                ts.ShowDialog();
            }
            while (TransferStock.act == "Transfer");
            fillItems();
        }

        private void ordersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrdersList ol = new OrdersList();
            ol.ShowDialog();
            GetData();
            fillItems();
        }

        private void newSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewSupplier ans = new AddNewSupplier();
            ans.ShowDialog();
            GetData();
            fillItems();
        }

        private void invetoryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemReport ir = new ItemReport();
            ir.ShowDialog();
            GetData();
            fillItems();
        }

        private void toolStripTbInventory_Click(object sender, EventArgs e)
        {
            ItemReport ir = new ItemReport();
            ir.ShowDialog();
        }

        private void purchaseOrderReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PurchaseOrderReport pod = new PurchaseOrderReport();
            pod.ShowDialog();
        }

        private void toolStripReports_ButtonClick(object sender, EventArgs e)
        {
            if (toolStripReports.HasDropDownItems)
            {
                toolStripReports.ShowDropDown();
            }
        }

        private void stockTransactionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockTransactionsReport str = new StockTransactionsReport();
            str.ShowDialog();
        }

        private void OpenSuppliersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SupplierDetails sd = new SupplierDetails();
            sd.ShowDialog();
            GetData();
            fillItems();
        }

        private void stockTransactionReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StockTransactionsReport str = new StockTransactionsReport();
            str.ShowDialog();
        }
    }
}
