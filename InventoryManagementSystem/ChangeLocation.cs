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
    public partial class ChangeLocation : Form
    {
        public ChangeLocation()
        {
            InitializeComponent();
        }

        private void ChangeLocation_Load(object sender, EventArgs e)
        {
            Locations.act = null;
            txtLocName.Text = Locations.LocName;
            rtbAddress.Text = Locations.LocAddress;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Locations.act = null;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtLocName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the location name", "Data Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLocName.Focus();
                return;
            }
            if (rtbAddress.Text.Trim() == "")
            {
                MessageBox.Show("Please enter address of the location", "Data Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtbAddress.Focus();
                return;
            }
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter adp;
            DataSet ds = new DataSet();
            adp = new SqlDataAdapter("Select * From ItemLocation", con);
            int rowPointer = Locations.rowPointer;
            adp.Fill(ds, "ItemLocation");
            int rowCount = ds.Tables["ItemLocation"].Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                string lName = ds.Tables["ItemLocation"].Rows[i][0].ToString();

                if (i == rowPointer)
                {
                    continue;
                }

                if (lName == txtLocName.Text.Trim())
                {
                    MessageBox.Show("This location Name matches with one of the existing locations in the record" +
                    Environment.NewLine + "Please enter different location name", "Data Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLocName.Focus();
                    return;
                }
            }
            
            //Updating the Location table 
            SqlCommandBuilder cmdBuilder = null;
            cmdBuilder = new SqlCommandBuilder(adp);
            
            con.Open();
            ds.Tables["ItemLocation"].Rows[rowPointer][0] = txtLocName.Text.Trim();
            ds.Tables["ItemLocation"].Rows[rowPointer][1] = rtbAddress.Text.Trim();
            adp.Update(ds, "ItemLocation");
            ds.AcceptChanges();

            MessageBox.Show("Location details edited successfully.", "Location Edit Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.Close();
            Locations.act = "Edit";
            this.Close();
        }
    }
}
