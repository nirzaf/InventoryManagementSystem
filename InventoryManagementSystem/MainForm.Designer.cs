namespace InventoryManagementSystem
{
    partial class MainForm
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
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(488, 356);
            this.Name = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem locationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem suppliersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem receiveStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sellStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transferStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ordersListToolStripMenuItem;
        private System.Windows.Forms.ToolStrip MaintoolStrip;
        private System.Windows.Forms.ToolStripButton toolStripNewItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripLocations;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripSuppliers;
        private System.Windows.Forms.ToolStripMenuItem newLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToLocationListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSupplierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenSuppliersListToolStripMenuItem;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape SideBar;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.TreeView tvItems;
        private System.Windows.Forms.GroupBox gbFindItem;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.ComboBox cbFindItem;
        private System.Windows.Forms.ToolStripMenuItem invetoryReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purchaseOrderReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripReports;
        private System.Windows.Forms.ToolStripMenuItem toolStripTbInventory;
        private System.Windows.Forms.ToolStripMenuItem toolStripPoDetails;
        private System.Windows.Forms.ToolStripMenuItem exitCloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stockTransactionReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stockTransactionReportToolStripMenuItem1;


    }
}

