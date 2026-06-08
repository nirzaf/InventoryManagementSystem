namespace InventoryManagementSystem
{
    partial class ItemReport
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
            // ItemReport
            // 
            this.ClientSize = new System.Drawing.Size(408, 291);
            this.Name = "ItemReport";
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rvStockInHand;
        private System.Windows.Forms.BindingSource StockInHandBindingSource;
        private StockInHandDataSet StockInHandDataSet;
        private InventoryManagementSystem.StockInHandDataSetTableAdapters.StockInHandTableAdapter StockInHandTableAdapter;
        
    }
}