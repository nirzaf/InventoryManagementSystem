using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace InventoryManagementSystem
{
    public partial class AddNewSupplier : Form
    {
        public AddNewSupplier()
        {
            InitializeComponent();
        }

        private void Validate(TextBox textBoxControl)
        {
            Regex rx = new Regex("[^A-Z|^a-z|^0-9|^@|^.|^_]");
            if (rx.IsMatch(textBoxControl.Text.Trim()))
            {
                throw new Exception("Invalid E-Mail address");
            }
        }

        private void ValidateText(TextBox textBoxControl)
        {
            Regex rx = new Regex("[^A-Z|^a-z|^ |^\t]");
            if (rx.IsMatch(textBoxControl.Text.Trim()))
            {
                throw new Exception("The Contact Person Name can contain only Characters");
            }
        }

        private void ValidateInt(TextBox textBoxControl)
        {
            Regex rx = new Regex("[^0-9]");
            if (rx.IsMatch(textBoxControl.Text.Trim()))
            {
                throw new Exception("Only Numbers without decimal value are allowed in Contact Number box");
            }

        }

        private void ValidateSpecialCharacter(string str)
        {
            string txtString = str.Replace("-", "");
            Regex rx = new Regex("[^A-Z|^a-z|^0-9|^ |^\t|^,|^/|^.|^\n]");
            if (rx.IsMatch(txtString))
            {
                throw new Exception("The address cannot contain special Characters, other than Hyphen(-), Comma (,), and Slash (/)");
            }
        }


        private void CountChar(string str)
        {
            int chrCount1 = 0;
            int chrCount2 = 0;

            foreach (char c in str)
            {
                string chk = c.ToString();
                if (chk == "@")
                {
                    chrCount1++;
                }

                if (chk == ".")
                {
                    chrCount2++;
                }

            }

            if (chrCount1 != 1)
            {
                throw new Exception("Invalid E-Mail address.");
            }

            if (chrCount2 < 1)
            {
                throw new Exception("Invalid E-Mail address.");
            }
        }
               
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSuppName.Text = "";
            txtContPeson.Text = "";
            rtbAddress.Text = "";
            txtContactNum.Text = "";
            txtEmail.Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtSuppName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter the supplier name", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSuppName.Focus();
                return;
            }
            if (txtContPeson.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter the Contact person name", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContPeson.Focus();
                return;
            }
            if (rtbAddress.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter the Address", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rtbAddress.Focus();
                return;
            }
            if (txtContactNum.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter the Contact Number", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContactNum.Focus();
                return;
            }
            if (txtContactNum.Text.StartsWith("0") | txtContactNum.Text.Length < 10)
            {
                MessageBox.Show("Invalid Contact Number", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContactNum.Focus();
                return;
            }
            if (txtEmail.Text == "")
            {
                MessageBox.Show("Please Enter the E-mail Id of the user", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return;
            }

            try
            {
                ValidateText(txtContPeson);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContPeson.Focus();
                return;
            }

            try
            {
                ValidateSpecialCharacter(rtbAddress.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rtbAddress.Focus();
                return;
            }

            try
            {
                ValidateInt(txtContactNum);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContactNum.Focus();
                return;
            }
            try
            {
                Validate(txtEmail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return;
            }
            try
            {
                CountChar(txtEmail.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return;
            }

            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter adp;
            DataSet ds = new DataSet();
            adp = new SqlDataAdapter("Select * From Suppliers", con);
            adp.Fill(ds, "Suppliers");

            int rowCount = ds.Tables["Suppliers"].Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                string lName = ds.Tables["Suppliers"].Rows[i][0].ToString();

                if (lName == txtSuppName.Text.Trim())
                {
                    MessageBox.Show("The supplier with this name, already exists in our record" +
                    Environment.NewLine + "Please enter different supplier name", "Data Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSuppName.Focus();
                    return;
                }
            }

            //Initializing New Row
            DataTable MyTable = ds.Tables["Suppliers"];
            DataRow MyNewRow = MyTable.NewRow();

            // Adding Values to the Row
            MyNewRow[0] = txtSuppName.Text.Trim();
            MyNewRow[1] = txtContPeson.Text.Trim();
            MyNewRow[2] = rtbAddress.Text.Trim();
            MyNewRow[3] = txtContactNum.Text.Trim();
            MyNewRow[4] = txtEmail.Text.Trim();

            // Adding row to the table
            MyTable.Rows.Add(MyNewRow);

            //Generating Insert Command
            SqlCommandBuilder UpdateDataCommand = new SqlCommandBuilder(adp);
            adp.InsertCommand = UpdateDataCommand.GetInsertCommand();

            //Addding row to the dataset
            adp.Update(ds, "Suppliers");

            //Updating Database with the new row
            ds.AcceptChanges();
            con.Close();

            DialogResult res = MessageBox.Show("The new supplier details are added successfully" +
                Environment.NewLine + "Do you want to add more suppliers?"
                , "Add Supplier Success", MessageBoxButtons.YesNo);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                txtSuppName.Text = "";
                txtContPeson.Text = "";
                rtbAddress.Text = "";
                txtContactNum.Text = "";
                txtEmail.Text = "";
                txtSuppName.Focus();
            }
            else
            {
                this.Close();
            }
        }
    }
}
