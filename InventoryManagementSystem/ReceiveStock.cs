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
    public partial class ReceiveStock : Form
    {
        public ReceiveStock()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
        public static string act = null;
        private void ReceiveStock_Load(object sender, EventArgs e)
        {
            act = null;
            fillItems();
            fillLocation();
        }
        public void fillItems()
        {
            SqlDataAdapter adp = new SqlDataAdapter("Select ItemCode from Items", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "Items");
            cbItem.Items.Clear();
            foreach (DataRow dr in ds.Tables["Items"].Rows)
            {
                cbItem.Items.Add(dr["ItemCode"]);
            }
        }

        public void fillLocation()
        {
            SqlDataAdapter adp = new SqlDataAdapter("Select LocationName from ItemLocation", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "ItemLocation");
            cbLocation.Items.Clear();
            foreach (DataRow dr in ds.Tables["ItemLocation"].Rows)
            {
                cbLocation.Items.Add(dr["LocationName"]);
            }
        }

        public void fillOrderNumber()
        {
            SqlDataAdapter adp = new SqlDataAdapter("Select PONumber from PurchaseOrder Where OrderStatus = 'Not Received'", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "PurchaseOrder");

            cbOrder.Items.Clear();
            foreach (DataRow dr in ds.Tables["PurchaseOrder"].Rows)
            {
                cbOrder.Items.Add(dr["PONumber"]);
            }
            try
            {
                cbOrder.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("There is no pending Order to be received", "No Pending order", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                rbSpecificItem.Checked = true;
            }
        }

        private void rbSpecificItem_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSpecificItem.Checked == true)
            {
                cbItem.Enabled = true;
                cbLocation.Enabled = true;
                txtQuantity.Enabled = true;

                cbOrder.Enabled = false;
            }
        }

        private void rbPurchaseOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPurchaseOrder.Checked == true)
            {
                cbItem.Enabled = false;
                cbLocation.Enabled = false;
                txtQuantity.Enabled = false;

                cbOrder.Enabled = true;

                fillOrderNumber();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            act = null;
            this.Close();
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {

            if (rbSpecificItem.Checked == true)
            {
                if (cbItem.Text == "")
                {
                    MessageBox.Show("Please select an item", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbItem.Focus();
                    return;
                }
                if (cbLocation.Text == "")
                {
                    MessageBox.Show("Please select a location", "Invalid location", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbLocation.Focus();
                    return;
                }
                if (txtQuantity.Text == "" | txtQuantity.Text == "0")
                {
                    MessageBox.Show("Please enter some quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQuantity.Focus();
                    return;
                }
                else
                {
                    Regex rx = new Regex("[^0-9]");
                    if (rx.IsMatch(txtQuantity.Text))
                    {
                        MessageBox.Show("Please enter valid quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtQuantity.Focus();
                        return;
                    }
                }
                SqlDataAdapter adp_chkExistingItems = new SqlDataAdapter("Select * from StockInHand where ItemCode = '" + cbItem.Text + "' and LocationName = '" + cbLocation.Text +"'", con);
                DataSet ds_chkExistingItems = new DataSet();
                adp_chkExistingItems.Fill(ds_chkExistingItems, "StockInHand");

                SqlDataAdapter adp = new SqlDataAdapter("Select ItemDescription, ItemRate from Items where ItemCode = '" + cbItem.Text + "'", con);
                DataSet ds = new DataSet();
                adp.Fill(ds, "items");
                if (ds_chkExistingItems.Tables["StockInHand"].Rows.Count > 0)
                {
                    SqlCommand UpdateCommand = new SqlCommand("Update StockInHand Set Quantity = Quantity + '" + txtQuantity.Text + "' WHERE ItemCode = '" + cbItem.Text + "' and LocationName = '" + cbLocation.Text + "'", con);

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    UpdateCommand.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    SqlCommand InsertCommand = new SqlCommand("Insert into StockInHand values (@ItemCode, @Desc, @Location, @Rate, @Qty)", con);

                    SqlParameter itemcode = new SqlParameter("@ItemCode", SqlDbType.VarChar);
                    SqlParameter description = new SqlParameter("@Desc", SqlDbType.VarChar);
                    SqlParameter location = new SqlParameter("@Location", SqlDbType.VarChar);
                    SqlParameter rate = new SqlParameter("@Rate", SqlDbType.Decimal);
                    SqlParameter quatnity = new SqlParameter("@Qty", SqlDbType.BigInt);

                    itemcode.Value = cbItem.Text;
                    description.Value = ds.Tables["Items"].Rows[0]["ItemDescription"].ToString();
                    location.Value = cbLocation.Text;
                    rate.Value = ds.Tables["Items"].Rows[0]["ItemRate"].ToString();
                    quatnity.Value = txtQuantity.Text;

                    InsertCommand.Parameters.Add(itemcode);
                    InsertCommand.Parameters.Add(description);
                    InsertCommand.Parameters.Add(location);
                    InsertCommand.Parameters.Add(rate);
                    InsertCommand.Parameters.Add(quatnity);

                    con.Open();
                    InsertCommand.ExecuteNonQuery();
                    con.Close();
                }
                MessageBox.Show("You have successfully received the stock and the corresponding entries are made in the database.", "Order Received", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (rbPurchaseOrder.Checked == true)
            {
                SqlDataAdapter adp2 = new SqlDataAdapter("Select ItemDescription, Location, ItemCode,  SUM(Quantity)as TotalQuantity from PODetails Where PONumber = '" + cbOrder.Text +"' group by ItemCode, ItemDescription, Location", con);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2,"PODetails");
                DataTable PODetails = ds2.Tables["PODetails"];
                int rowCount = PODetails.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    SqlDataAdapter adp3 = new SqlDataAdapter("Select * from StockInHand Where ItemCode = '" + PODetails.Rows[i]["ItemCode"] + "' and LocationName = '" + PODetails.Rows[i]["Location"] + "'", con);
                    DataSet ds3 = new DataSet();
                    adp3.Fill(ds3, "StockInHand");
                    DataTable StockInHand = ds3.Tables["StockInHand"];
                    int rowCount1 = StockInHand.Rows.Count;
                    if (rowCount1 > 0)
                    {
                        SqlCommand UpdateStock = new SqlCommand("Update StockInHand set Quantity = Quantity + " + PODetails.Rows[i]["TotalQuantity"] + " Where ItemCode = '" + PODetails.Rows[i]["ItemCode"] + "' and LocationName = '" + PODetails.Rows[i]["Location"] + "'", con);

                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        UpdateStock.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        SqlDataAdapter adp4 = new SqlDataAdapter("Select ItemRate from Items Where ItemCode = '" + PODetails.Rows[i]["ItemCode"] + "'", con);
                        DataSet ds4 = new DataSet();
                        adp4.Fill(ds4, "Items");
                        DataTable Items = ds4.Tables["Items"];
                        decimal itemRate = Convert.ToDecimal(Items.Rows[0]["ItemRate"]);

                        ds4.Clear();
                        adp4 = new SqlDataAdapter("Select * from StockInHand", con);
                        adp4.Fill(ds4, "StockInHand");
                        DataTable mytable1 = ds4.Tables["StockInHand"];

                        DataRow newrow1 = mytable1.NewRow();
                        newrow1[0] = PODetails.Rows[i]["ItemCode"];
                        newrow1[1] = PODetails.Rows[i]["ItemDescription"];
                        newrow1[2] = PODetails.Rows[i]["Location"];
                        newrow1[3] = itemRate;
                        newrow1[4] = PODetails.Rows[i]["TotalQuantity"];
                        
                        //adding new row to the table
                        mytable1.Rows.Add(newrow1);

                        //generating insert command
                        SqlCommandBuilder updatedatacommand1 = new SqlCommandBuilder(adp4);
                        adp4.InsertCommand = updatedatacommand1.GetInsertCommand();

                        //addding row to the dataset
                        adp4.Update(ds4, "StockInHand");

                        //updating database with the new row
                        ds4.AcceptChanges();
                        con.Close();
                    }

                }

                SqlCommand UpdatePO = new SqlCommand("Update PurchaseOrder set OrderStatus = 'Received' where PONumber = '" + cbOrder.Text +"'",con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                UpdatePO.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("You have successfully received the stock and the corresponding entries are made in the database.", "Order Received", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fillOrderNumber();

            }
            cbItem.SelectedIndex= -1;
            cbLocation.SelectedIndex = -1;
            cbOrder.SelectedIndex = -1;
            txtQuantity.Text = "1";
            rbSpecificItem.Checked = true;
            act = "Receive";
            this.Close();
        }
    }
}
