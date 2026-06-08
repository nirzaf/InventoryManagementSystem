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
    public partial class Items : Form
    {
        public Items()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
        SqlDataAdapter adp;
        DataSet ds = new DataSet();

        private string GenerateId()
        {
            string id = System.Guid.NewGuid().ToString();
            string newId = "ITM";
            int charCount = 0;
            foreach (char c in id)
            {
                if (char.IsDigit(c))
                {
                    newId += c.ToString();
                    charCount += 1;
                }
                if (charCount == 4)
                {
                    break;
                }
            }
            return newId;
        }

        private void Items_Load(object sender, EventArgs e)
        {
            string getId = GenerateId();
            fillItems();
            //Checking if the ID already exists, 
            // If exists, regenerate it
            adp = new SqlDataAdapter("Select * from Items", con);
            ds.Clear();
            adp.Fill(ds, "Items");
            DataTable MyTable = ds.Tables["Items"];
            for (int i = 0; i < MyTable.Rows.Count; i++)
            {
                string idExist = MyTable.Rows[i][0].ToString();
                if (getId == idExist)
                {
                    getId = GenerateId();
                    i = -1;
                }
            }

            txtItemCode.Text = getId;
            txtItemCode.ReadOnly = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtItemCode.Text = GenerateId();
            rtbDescription.Text = "";
            txtItemRate.Text = "";            
        }

        private void fillItems()
        {
            SqlDataAdapter adpItem = new SqlDataAdapter("Select * from Items", con);
            DataSet dsItem = new DataSet();
            dsItem.Clear();
            adpItem.Fill(dsItem, "Items");

            dgvItem.DataSource = dsItem.Tables["Items"].DefaultView;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (rtbDescription.Text == "")
            {
                MessageBox.Show("Please provide description of the item", "Item Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtbDescription.Focus();
                return;
            }
            if (txtItemRate.Text == "")
            {
                MessageBox.Show("Please enter rate of the item", "Item Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtItemRate.Focus();
                return;
            }
            if (txtItemRate.Text.StartsWith("0"))
            {
                MessageBox.Show("Please enter valid rate of the item", "Item Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtItemRate.Focus();
                return;
            }
            char[] itemRate = txtItemRate.Text.ToCharArray();
            int length = txtItemRate.Text.Length;
            int decPoint = 0;
            int decDigitCount = 0;
            for (int i = 0; i < length; i++)
            {
                if (!char.IsDigit(itemRate[i]))
                {
                    if (itemRate[i].ToString() == ".")
                    {
                        if (decPoint == 0)
                        {
                            decPoint = 1;
                        }
                        else
                        {
                            MessageBox.Show("More than one decimal points are not allowed in the Item Rate text box", "Item Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtItemRate.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Only numeric values along with (or without) two digits after decimal point are allowed for the Item Rate text box",
                            "Item Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtItemRate.Focus();
                        return;
                    }
                }
                else
                {
                    if (decPoint == 1)
                    {
                        decDigitCount++;
                        if (decDigitCount > 2)
                        {
                            MessageBox.Show("Only two digits are allowed after decimal point", "Item Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtItemRate.Focus();
                            return;
                        }
                    }
                }
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            adp = new SqlDataAdapter("Select * from Items", con);
            ds.Clear();
            adp.Fill(ds, "Items");
            DataTable ItemsTable = ds.Tables["Items"];
            DataRow MyNewRow = null;
            MyNewRow = ItemsTable.NewRow();
            MyNewRow[0] = txtItemCode.Text;
            MyNewRow[1] = rtbDescription.Text;
            MyNewRow[2] = txtItemRate.Text;
            ItemsTable.Rows.Add(MyNewRow);

            SqlCommandBuilder UpdateDataCommand = new SqlCommandBuilder(adp);
            adp.InsertCommand = UpdateDataCommand.GetInsertCommand();
            adp.Update(ds, "Items");
            ds.AcceptChanges();
            MessageBox.Show("The New items details are enterd successfully", "Data Insertion Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.Close();

            txtItemCode.Text = GenerateId();
            txtItemRate.Text = "";
            rtbDescription.Text = "";
            fillItems();
        }        
    }
}
