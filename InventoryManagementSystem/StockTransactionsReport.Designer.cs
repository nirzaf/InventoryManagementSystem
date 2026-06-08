namespace InventoryManagementSystem
{
    partial class StockTransactionsReport
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.StockTransationsDataSet = new InventoryManagementSystem.StockTransationsDataSet();
            this.StockTransactionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.StockTransactionsTableAdapter = new InventoryManagementSystem.StockTransationsDataSetTableAdapters.StockTransactionsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.StockTransationsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockTransactionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "StockTransationsDataSet_StockTransactions";
            reportDataSource1.Value = this.StockTransactionsBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "InventoryManagementSystem.StockTransationsReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(672, 506);
            this.reportViewer1.TabIndex = 0;
            // 
            // StockTransationsDataSet
            // 
            this.StockTransationsDataSet.DataSetName = "StockTransationsDataSet";
            this.StockTransationsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // StockTransactionsBindingSource
            // 
            this.StockTransactionsBindingSource.DataMember = "StockTransactions";
            this.StockTransactionsBindingSource.DataSource = this.StockTransationsDataSet;
            // 
            // StockTransactionsTableAdapter
            // 
            this.StockTransactionsTableAdapter.ClearBeforeFill = true;
            // 
            // StockTransactionsReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 506);
            this.Controls.Add(this.reportViewer1);
            this.Name = "StockTransactionsReport";
            this.Text = "StockTransactionsReport";
            this.Load += new System.EventHandler(this.StockTransactionsReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.StockTransationsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockTransactionsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource StockTransactionsBindingSource;
        private StockTransationsDataSet StockTransationsDataSet;
        private InventoryManagementSystem.StockTransationsDataSetTableAdapters.StockTransactionsTableAdapter StockTransactionsTableAdapter;
    }
}