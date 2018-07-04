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
    public partial class OrdersList : Form
    {
        public OrdersList()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
        SqlDataAdapter adp;
        DataSet ds = new DataSet();

        private void GetData()
        {
            adp = new SqlDataAdapter("Select * From PurchaseOrder", con);
            ds.Clear();
            adp.Fill(ds, "PurchaseOrder");
            dgvPOs.DataSource = ds.Tables["PurchaseOrder"].DefaultView;
            dgvPOs.Columns[0].HeaderText = "Order Number";
            dgvPOs.Columns[1].HeaderText = "Supplier";
            dgvPOs.Columns[2].HeaderText = "Order Date";
            dgvPOs.Columns[3].HeaderText = "Order Status";
            dgvPOs.Columns[4].HeaderText = "Order Amount";
        }

        private void OrdersList_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void dgvPOs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowPointer = dgvPOs.SelectedCells[0].RowIndex;
            DataGridViewRow dgvr = dgvPOs.Rows[rowPointer];
            PrintPO.ordernumber = dgvr.Cells[0].Value.ToString();
            PrintPO.orderdate= dgvr.Cells[2].Value.ToString();
            PrintPO.supplier = dgvr.Cells[1].Value.ToString();            
            PrintPO.totalamount = dgvr.Cells[4].Value.ToString();
            SqlDataAdapter adp1 = new SqlDataAdapter("Select SuppAddress From Suppliers Where SuppName = '" + dgvr.Cells[1].Value.ToString() + "'", con);
            DataSet ds1 = new DataSet();
            ds1.Clear();
            adp1.Fill(ds1,"Suppliers");
            PrintPO.suppAddress = ds1.Tables["Suppliers"].Rows[0]["SuppAddress"].ToString();
            OrdersListDetail old = new OrdersListDetail();
            old.ShowDialog();
        }
    }
}
