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
    public partial class AddLocation : Form
    {
        public AddLocation()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
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
            adp.Fill(ds, "ItemLocation");
            
            int rowCount = ds.Tables["ItemLocation"].Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                string lName = ds.Tables["ItemLocation"].Rows[i][0].ToString();

                if (lName == txtLocName.Text.Trim())
                {
                    MessageBox.Show("This location Name matches with one of the existing locations in the record" +
                    Environment.NewLine + "Please enter different location name", "Data Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLocName.Focus();
                    return;
                }
            }

            //Initializing New Row
            DataTable MyTable = ds.Tables["ItemLocation"];
            DataRow MyNewRow = MyTable.NewRow();

            //Adding Values to the Row
            MyNewRow[0] = txtLocName.Text.Trim();
            MyNewRow[1] = rtbAddress.Text.Trim();

            //Adding row to the table
            MyTable.Rows.Add(MyNewRow);

            //Generating Insert Command
            SqlCommandBuilder UpdateDataCommand = new SqlCommandBuilder(adp);
            adp.InsertCommand = UpdateDataCommand.GetInsertCommand();

            //Addding row to the dataset
            adp.Update(ds, "ItemLocation");

            //Updating Database with the new row
            ds.AcceptChanges();
            con.Close();

            DialogResult res = MessageBox.Show("The new Location added successfully to the record" +
                Environment.NewLine + "Do you want to add more warehouse locations"
                , "Add Location Success", MessageBoxButtons.YesNo);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                txtLocName.Text = "";
                rtbAddress.Text = "";
                txtLocName.Focus();
            }
            else
            {
                this.Close();
            }
        }
    }
}
