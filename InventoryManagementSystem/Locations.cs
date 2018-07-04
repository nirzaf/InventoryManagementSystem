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
    public partial class Locations : Form
    {
        public Locations()
        {
            InitializeComponent();
        }

        public static string LocName, LocAddress, act = null;
        public static int rowPointer;

        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
        SqlDataAdapter adp;
        DataSet ds = new DataSet();

        private void GetData()
        {

            ds.Clear();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            adp = new SqlDataAdapter("Select * From ItemLocation", con);
            adp.Fill(ds, "ItemLocation");
            DataTable LocationTable1 = ds.Tables["ItemLocation"];
            dgvLocations.DataSource = LocationTable1.DefaultView;
            con.Close();
        }

        private void Locations_Load(object sender, EventArgs e)
        {
            act = null;
            GetData();
            dgvLocations.Select();
        }

        private void Locations_Paint(object sender, PaintEventArgs e)
        {
            dgvLocations.Height = this.Height - 64;
        }

        private void toolStripEdit_Click(object sender, EventArgs e)
        {
            if (dgvLocations.SelectedCells[0].Value.ToString() == "" || dgvLocations.SelectedCells[1].Value.ToString() == "")
            {
                MessageBox.Show("Please select the location, you want to edit", "Select Row", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LocName = dgvLocations.SelectedCells[0].Value.ToString();
            LocAddress = dgvLocations.SelectedCells[1].Value.ToString();
            rowPointer = dgvLocations.SelectedCells[0].RowIndex;
            ChangeLocation cl = new ChangeLocation();
            cl.ShowDialog();
            // After editing, refetch the data from database, to view updated record
            GetData();
            if (act == "Edit")
            {
                this.Close();
            }
        }

        private void toolStripAdd_Click(object sender, EventArgs e)
        {
            AddLocation al = new AddLocation();
            al.ShowDialog();

            // After adding the new location, refetch the data from database, to view updated record
            GetData();
        }

        private void toolStripDelete_Click(object sender, EventArgs e)
        {
            rowPointer = dgvLocations.SelectedCells[0].RowIndex;
            LocName = dgvLocations.SelectedCells[0].Value.ToString();
            if (rowPointer == -1)
            {
                MessageBox.Show("Please select the location to delete", "No item selected", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Are you sure to delete this location", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    SqlDataAdapter adp1 = new SqlDataAdapter("Select * From StockInHand where LocationName = '" + LocName + "'", con);
                    DataSet ds2 = new DataSet();
                    ds2.Clear();
                    adp1.Fill(ds2, "StockInHand");
                    DataTable t1 = new DataTable();
                    t1 = ds2.Tables["StockInHand"];
                    if (t1.Rows.Count >= 1)
                    {
                        MessageBox.Show("The Location you want to delete has some stock in it." + Environment.NewLine + "Clear the stock from location and then try again", "Deleting Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                    string DelCommand = "Delete from ItemLocation where LocationName = '" + LocName + "'";
                    SqlCommand myDelCommand = new SqlCommand(DelCommand, con);
                    if (con.State == ConnectionState.Closed)
                    {
                        myDelCommand.Connection.Open();
                    }
                    myDelCommand.ExecuteNonQuery();
                    myDelCommand.Connection.Close();

                    MessageBox.Show("The location deleted successfully", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetData();
                }
            }
        }
    }
}
