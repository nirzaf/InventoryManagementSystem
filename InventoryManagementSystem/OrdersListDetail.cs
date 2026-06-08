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
    public partial class OrdersListDetail : Form
    {
        public OrdersListDetail()
        {
            InitializeComponent();
        }

        private void OrdersListDetail_Load(object sender, EventArgs e)
        {
            string PoNum = PrintPO.ordernumber;
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter adp;
            DataSet ds = new DataSet();

            adp = new SqlDataAdapter("Select * From PODetails where PONumber = '" + PoNum + "'", con);
            ds.Clear();
            adp.Fill(ds, "PODetails");
            dgvPODetails.DataSource = ds.Tables["PODetails"].DefaultView;
            dgvPODetails.Columns[0].HeaderText = "Order Number";
            dgvPODetails.Columns[1].HeaderText = "Location";
            dgvPODetails.Columns[2].HeaderText = "Item Code";
            dgvPODetails.Columns[3].HeaderText = "Description";
            dgvPODetails.Columns[4].HeaderText = "Quantity";
            dgvPODetails.Columns[5].HeaderText = "Rate";
            dgvPODetails.Columns[6].HeaderText = "Amount";
        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {
            int rowcount = dgvPODetails.RowCount;
            PrintPO.itemcode = "";
            PrintPO.quantity = "";
            PrintPO.description = "";
            PrintPO.unitvalue = "";
            PrintPO.totalvalue = "";

            for (int i = 0; i < rowcount; i++)
            {
                PrintPO.itemcode += dgvPODetails.Rows[i].Cells["ItemCode"].Value + Environment.NewLine;
                PrintPO.quantity += dgvPODetails.Rows[i].Cells["Quantity"].Value + Environment.NewLine;
                PrintPO.description += dgvPODetails.Rows[i].Cells["ItemDescription"].Value + Environment.NewLine;
                PrintPO.unitvalue += dgvPODetails.Rows[i].Cells["Rate"].Value + Environment.NewLine;
                PrintPO.totalvalue += dgvPODetails.Rows[i].Cells["Amount"].Value + Environment.NewLine;
            }
            PrintPO ppo = new PrintPO();
            ppo.ShowDialog();
        }

        private void tsbPrint_Paint(object sender, PaintEventArgs e)
        {
            dgvPODetails.Height = this.Height - 89;
        }
    }
}
