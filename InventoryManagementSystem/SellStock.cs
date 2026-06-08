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
    public partial class SellStock : Form
    {
        public SellStock()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
        SqlDataAdapter adp;
        DataSet ds = new DataSet();
        public static string act = null;
        string ItemDesc;

        private void GetItems()
        {
            adp = new SqlDataAdapter("Select distinct ItemCode From StockInHand", con);
            ds.Clear();
            adp.Fill(ds, "StockInHand");
        }

        private void ValidateQuant(string Quantity)
        {
            Regex rx = new Regex("[^0-9]");
            if (rx.IsMatch(Quantity))
            {
                throw new Exception("Only Numbers without decimal value are allowed");
            }
        }

        private void SellStock_Load(object sender, EventArgs e)
        {
            act = null;
            GetItems();
            DataTable AllItems = new DataTable();
            AllItems = ds.Tables["StockInHand"];
            int rowCount = AllItems.Rows.Count;
            cbItems.Items.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                cbItems.Items.Add(AllItems.Rows[i][0]);
            }
            cbItems.SelectedIndex = -1;
        }

        private void cbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbItems.SelectedIndex == -1)
            {
                return;
            }
            string itemCode = cbItems.Text;

            adp = new SqlDataAdapter("Select * From StockInHand where ItemCode = '" + itemCode + "'", con);
            ds.Clear();
            adp.Fill(ds, "StockInHand");

            DataTable AllItems = new DataTable();
            AllItems = ds.Tables["StockInHand"];
            int rowCount = AllItems.Rows.Count;
            cbItemLocation.Items.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                cbItemLocation.Items.Add(AllItems.Rows[i][2]);
            }
            txtQuantity.Text = "";
            txtQuantityAvailable.Text = "";
        }

        private void cbItemLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbItemLocation.SelectedIndex == -1)
            {
                return;
            }
            int rowPointer = cbItemLocation.SelectedIndex;
            txtQuantityAvailable.Text = ds.Tables["StockInHand"].Rows[rowPointer][4].ToString();
        }

        private void txtQuantity_Leave(object sender, EventArgs e)
        {
            try
            {
                ValidateQuant(txtQuantity.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Quantity Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQuantity.Text = "";
                txtQuantity.Focus();
                return;
            }

            int textLength = txtQuantity.Text.Trim().Length;
            char[] quant = txtQuantity.Text.Trim().ToCharArray();
            for (int i = 0; i < textLength; i++)
            {
                if (quant[i] == '0')
                {
                    txtQuantity.Text = txtQuantity.Text.Remove(0, 1);
                }
                else
                {
                    break;
                }
            }
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            if (cbItems.Text == "")
            {
                MessageBox.Show("Please select an item.", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbItems.Focus();
                return;
            }
            if (cbItemLocation.Text == "")
            {
                MessageBox.Show("Please select a location.", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbItemLocation.Focus();
                return;
            }
            if (txtQuantity.Text == "")
            {
                MessageBox.Show("Please enter valid quantity.", "Invalid Qunatity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQuantity.Focus();
                return;
            }
            if ((Convert.ToInt32(txtQuantity.Text)) > (Convert.ToInt32(txtQuantityAvailable.Text)))
            {
                MessageBox.Show("You cannot sell more than the Quantity Available: " + txtQuantityAvailable.Text, "Invalid Qunatity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQuantity.Focus();
                return;
            }
            if ((Convert.ToInt32(txtQuantity.Text)) == (Convert.ToInt32(txtQuantityAvailable.Text)))
            {
                SqlCommand DeleteCommand = new SqlCommand("Delete from StockInHand where ItemCode = '" + cbItems.Text + "' and LocationName = '"+ cbItemLocation.Text + "'", con);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                DeleteCommand.ExecuteNonQuery();
                con.Close();                
            }
            else if ((Convert.ToInt32(txtQuantity.Text)) < (Convert.ToInt32(txtQuantityAvailable.Text)))
            {
                SqlCommand UpdateCommand = new SqlCommand("Update StockInHand Set Quantity = Quantity - " + txtQuantity.Text + " where ItemCode = '" + cbItems.Text + "' and LocationName = '" + cbItemLocation.Text + "'", con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                UpdateCommand.ExecuteNonQuery();
                con.Close();
            }
            MessageBox.Show("The stock sold succesfully");
            SqlCommand myTransaction = new SqlCommand("Insert into StockTransactions values( '" + DateTime.Now.ToShortDateString() + "', '" + cbItems.Text.Trim() + "', '" + ds.Tables["StockInHand"].Rows[0]["ItemDescription"].ToString() + "', '" + cbItemLocation.Text.Trim() + "', '" + txtQuantity.Text + "', 'Sent to selling department')", con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            myTransaction.ExecuteNonQuery();
            con.Close();
            GetItems();
            DataTable AllItems = new DataTable();
            AllItems = ds.Tables["StockInHand"];
            int rowCount = AllItems.Rows.Count;
            cbItems.Items.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                cbItems.Items.Add(AllItems.Rows[i][0]);
            }
            cbItems.SelectedIndex = -1;
            cbItemLocation.Items.Clear();
            txtQuantity.Text = "";
            txtQuantityAvailable.Text = "";
            act = "Sell";
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            act = null;
            this.Close();
        }
    }
}
