using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace InventoryManagementSystem
{
    public partial class TransferStock : Form
    {
        public TransferStock()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
        public static string act = null;

        private void TransferStock_Load(object sender, EventArgs e)
        {
            fillItemDetails();
            fillLocation();
            act = null;
        }

        private void ValidateQuantity(string txt)
        {
            Regex rx = new Regex("[^0-9]");
            if (rx.IsMatch(txt))
            {
                throw new Exception("Only Numbers without decimal value are allowed in Quantity text field");
            }
        }

        private void ValidateFirstIntValue(string a)
        {
            Regex rx = new Regex("[^1-9]");
            if (rx.IsMatch(a))
            {
                throw new Exception("The first value cannot be zero in Quantity text field");
            }
        }
        
        public void fillItemDetails()
        {
            SqlDataAdapter adp = new SqlDataAdapter("Select * from StockInHand", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "StockInHand");

            dgvStockInHand.DataSource = ds.Tables[0].DefaultView;
            dgvStockInHand.Columns[0].HeaderText = "Item Code";
            dgvStockInHand.Columns[1].HeaderText = "Description";
            dgvStockInHand.Columns[2].HeaderText = "Location";
            dgvStockInHand.Columns[3].HeaderText = "Rate";
            dgvStockInHand.Columns[4].HeaderText = "Quantity";
        }

        public void fillLocation()
        {
            SqlDataAdapter adp = new SqlDataAdapter("Select * from ItemLocation", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "ItemLocation");
            cbLocation.Items.Clear();
            foreach (DataRow dr in ds.Tables["ItemLocation"].Rows)
            {
                cbLocation.Items.Add(dr["LocationName"]);
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if (dgvStockInHand.CurrentRow.Cells[2].Value.ToString() == cbLocation.Text)
            {
                MessageBox.Show("Please select another location to transfer the item");
                return;
            }

            try
            {
                ValidateQuantity(txtQuantity.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtQuantity.Text = "1";
                txtQuantity.Focus();
                return;
            }

            try
            {
                char[] allVal = txtQuantity.Text.Trim().ToCharArray();
                ValidateFirstIntValue(allVal[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtQuantity.Focus();
                return;
            }

            int QuantityAvailable, QuantityTransfer;
            QuantityAvailable = Convert.ToInt32(dgvStockInHand.CurrentRow.Cells["Quantity"].Value);
            QuantityTransfer = Convert.ToInt32(txtQuantity.Text);

            if (QuantityTransfer > QuantityAvailable)
            {
                MessageBox.Show("You can transfer more than the avialable quantity");
                return;
            }
            if (cbLocation.Text == "")
            {
                MessageBox.Show("Please select a location to tranfer the stock", "Invalid Location", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SqlDataAdapter adp = new SqlDataAdapter("Select * from StockInHand where ItemCode = '" + dgvStockInHand.CurrentRow.Cells[0].Value.ToString() + "' and LocationName = '" + cbLocation.Text + "'", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "StockInHand");

            if (ds.Tables["StockInHand"].Rows.Count > 0)
            {
                if (txtQuantity.Text != dgvStockInHand.CurrentRow.Cells["Quantity"].Value.ToString())
                {
                    //add quantity in new location
                    SqlCommand UpdateComm = new SqlCommand("update StockInHand set Quantity = Quantity + '" + txtQuantity.Text + "' Where ItemCode = '" + dgvStockInHand.CurrentRow.Cells[0].Value.ToString() + "' and LocationName = '" + cbLocation.Text + "'", con);

                    //subtract quantity from old location
                    SqlCommand UpdateComm1 = new SqlCommand("update StockInHand set Quantity = Quantity - '" + txtQuantity.Text + "' Where ItemCode = '" + dgvStockInHand.CurrentRow.Cells[0].Value.ToString() + "' and LocationName = '" + dgvStockInHand.CurrentRow.Cells[2].Value.ToString() + "'", con);

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    UpdateComm.ExecuteNonQuery();
                    UpdateComm1.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    //add quantity in new location
                    SqlCommand UpdateComm = new SqlCommand("update StockInHand set Quantity = Quantity + '" + txtQuantity.Text + "' Where ItemCode = '" + dgvStockInHand.CurrentRow.Cells[0].Value.ToString() + "' and LocationName = '" + cbLocation.Text + "'", con);

                    //Delete the item from old location
                    SqlCommand DeleteComm = new SqlCommand("Delete from StockInHand Where ItemCode = '" + dgvStockInHand.CurrentRow.Cells[0].Value.ToString() + "' and LocationName = '" + dgvStockInHand.CurrentRow.Cells[2].Value.ToString() + "'", con);

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    UpdateComm.ExecuteNonQuery();
                    DeleteComm.ExecuteNonQuery();
                    con.Close();
                }
            }
            else
            {
                SqlDataAdapter adp1 = new SqlDataAdapter("Select * from StockInHand", con);
                DataSet ds1 = new DataSet();
                ds1.Clear();
                adp1.Fill(ds1, "StockInHand");
                DataTable mytable1 = ds1.Tables["StockInHand"];

                DataRow newrow1 = mytable1.NewRow();
                newrow1[0] = dgvStockInHand.CurrentRow.Cells["ItemCode"].Value;
                newrow1[1] = dgvStockInHand.CurrentRow.Cells["ItemDescription"].Value;
                newrow1[2] = cbLocation.Text;
                newrow1[3] = dgvStockInHand.CurrentRow.Cells["ItemRate"].Value;
                newrow1[4] = txtQuantity.Text;

                //adding new row to the table
                mytable1.Rows.Add(newrow1);

                //generating insert command
                SqlCommandBuilder updatedatacommand1 = new SqlCommandBuilder(adp1);
                adp1.InsertCommand = updatedatacommand1.GetInsertCommand();

                //addding row to the dataset
                adp1.Update(ds1, "StockInHand");

                //updating database with the new row
                ds1.AcceptChanges();
                con.Close();

                if (txtQuantity.Text != dgvStockInHand.CurrentRow.Cells["Quantity"].Value.ToString())
                {
                    //subtract quantity from old location
                    SqlCommand UpdateComm = new SqlCommand("update StockInHand set Quantity = Quantity - '" + txtQuantity.Text + "' Where ItemCode = '" + dgvStockInHand.CurrentRow.Cells[0].Value.ToString() + "' and LocationName = '" + dgvStockInHand.CurrentRow.Cells[2].Value.ToString() + "'", con);

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    UpdateComm.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    //Delete the item from old location
                    SqlCommand DeleteComm = new SqlCommand("Delete from StockInHand Where ItemCode = '" + dgvStockInHand.CurrentRow.Cells[0].Value.ToString() + "' and LocationName = '" + dgvStockInHand.CurrentRow.Cells[2].Value.ToString() + "'", con);

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    DeleteComm.ExecuteNonQuery();
                    con.Close();
                }
            }
            SqlCommand myTransaction = new SqlCommand("Insert into StockTransactions values( '" + DateTime.Now.ToShortDateString() + "', '" + dgvStockInHand.CurrentRow.Cells["ItemCode"].Value.ToString() + "', '" + dgvStockInHand.CurrentRow.Cells["ItemDescription"].Value.ToString() + "', '" + dgvStockInHand.CurrentRow.Cells[2].Value.ToString() + "', '" + txtQuantity.Text + "', ' Transferred to " + cbLocation.Text + "')", con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            myTransaction.ExecuteNonQuery();
            con.Close();
            fillItemDetails();
            txtQuantity.Text = "1";
            cbLocation.SelectedIndex = -1;
            act = "Transfer";
            this.Close();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            act = null;
            this.Close();
        }
    }
}
