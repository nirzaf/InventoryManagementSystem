namespace InventoryManagementSystem
{
    partial class SellStock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblItem = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblQuantAvailable = new System.Windows.Forms.Label();
            this.cbItems = new System.Windows.Forms.ComboBox();
            this.cbItemLocation = new System.Windows.Forms.ComboBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtQuantityAvailable = new System.Windows.Forms.TextBox();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblItem
            // 
            this.lblItem.AutoSize = true;
            this.lblItem.Location = new System.Drawing.Point(13, 36);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(27, 13);
            this.lblItem.TabIndex = 0;
            this.lblItem.Text = "Item";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(13, 82);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(48, 13);
            this.lblLocation.TabIndex = 1;
            this.lblLocation.Text = "Location";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(13, 134);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(46, 13);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Quantity";
            // 
            // lblQuantAvailable
            // 
            this.lblQuantAvailable.AutoSize = true;
            this.lblQuantAvailable.Location = new System.Drawing.Point(13, 184);
            this.lblQuantAvailable.Name = "lblQuantAvailable";
            this.lblQuantAvailable.Size = new System.Drawing.Size(92, 13);
            this.lblQuantAvailable.TabIndex = 3;
            this.lblQuantAvailable.Text = "Quantity Available";
            // 
            // cbItems
            // 
            this.cbItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItems.FormattingEnabled = true;
            this.cbItems.Location = new System.Drawing.Point(176, 36);
            this.cbItems.Name = "cbItems";
            this.cbItems.Size = new System.Drawing.Size(179, 21);
            this.cbItems.TabIndex = 4;
            this.cbItems.SelectedIndexChanged += new System.EventHandler(this.cbItems_SelectedIndexChanged);
            // 
            // cbItemLocation
            // 
            this.cbItemLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItemLocation.FormattingEnabled = true;
            this.cbItemLocation.Location = new System.Drawing.Point(176, 82);
            this.cbItemLocation.Name = "cbItemLocation";
            this.cbItemLocation.Size = new System.Drawing.Size(179, 21);
            this.cbItemLocation.TabIndex = 4;
            this.cbItemLocation.SelectedIndexChanged += new System.EventHandler(this.cbItemLocation_SelectedIndexChanged);
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(176, 134);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(179, 20);
            this.txtQuantity.TabIndex = 5;
            this.txtQuantity.Leave += new System.EventHandler(this.txtQuantity_Leave);
            // 
            // txtQuantityAvailable
            // 
            this.txtQuantityAvailable.Location = new System.Drawing.Point(176, 177);
            this.txtQuantityAvailable.Name = "txtQuantityAvailable";
            this.txtQuantityAvailable.ReadOnly = true;
            this.txtQuantityAvailable.Size = new System.Drawing.Size(179, 20);
            this.txtQuantityAvailable.TabIndex = 5;
            // 
            // btnSell
            // 
            this.btnSell.Location = new System.Drawing.Point(85, 233);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(75, 23);
            this.btnSell.TabIndex = 6;
            this.btnSell.Text = "Sell";
            this.btnSell.UseVisualStyleBackColor = true;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(213, 233);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SellStock
            // 
            this.AcceptButton = this.btnSell;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(366, 268);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSell);
            this.Controls.Add(this.txtQuantityAvailable);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.cbItemLocation);
            this.Controls.Add(this.cbItems);
            this.Controls.Add(this.lblQuantAvailable);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblItem);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(382, 304);
            this.MinimumSize = new System.Drawing.Size(382, 304);
            this.Name = "SellStock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sell the Stock";
            this.Load += new System.EventHandler(this.SellStock_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblQuantAvailable;
        private System.Windows.Forms.ComboBox cbItems;
        private System.Windows.Forms.ComboBox cbItemLocation;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtQuantityAvailable;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Button btnCancel;
    }
}