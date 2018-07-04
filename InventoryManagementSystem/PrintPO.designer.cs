namespace InventoryManagementSystem
{
    partial class PrintPO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPO));
            this.lblDU = new System.Windows.Forms.Label();
            this.lblCompName = new System.Windows.Forms.Label();
            this.lblPo = new System.Windows.Forms.Label();
            this.lblPoNum = new System.Windows.Forms.Label();
            this.lblPoDate = new System.Windows.Forms.Label();
            this.lblStaticQuant = new System.Windows.Forms.Label();
            this.lblStatDescp = new System.Windows.Forms.Label();
            this.lblStatUnitValue = new System.Windows.Forms.Label();
            this.lblQuant = new System.Windows.Forms.Label();
            this.lblDescp = new System.Windows.Forms.Label();
            this.lblUnitValue = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblItemCode = new System.Windows.Forms.Label();
            this.lblStatAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.lblSuppName = new System.Windows.Forms.Label();
            this.lblSuppAddress = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDU
            // 
            this.lblDU.AutoSize = true;
            this.lblDU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDU.Location = new System.Drawing.Point(78, 74);
            this.lblDU.Name = "lblDU";
            this.lblDU.Size = new System.Drawing.Size(151, 13);
            this.lblDU.TabIndex = 0;
            this.lblDU.Text = "DU Invetory Management";
            // 
            // lblCompName
            // 
            this.lblCompName.AutoSize = true;
            this.lblCompName.Location = new System.Drawing.Point(78, 103);
            this.lblCompName.Name = "lblCompName";
            this.lblCompName.Size = new System.Drawing.Size(90, 13);
            this.lblCompName.TabIndex = 0;
            this.lblCompName.Text = "Kogent Learnings";
            // 
            // lblPo
            // 
            this.lblPo.AutoSize = true;
            this.lblPo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPo.Location = new System.Drawing.Point(491, 74);
            this.lblPo.Name = "lblPo";
            this.lblPo.Size = new System.Drawing.Size(179, 26);
            this.lblPo.TabIndex = 0;
            this.lblPo.Text = "Purchase Order";
            // 
            // lblPoNum
            // 
            this.lblPoNum.AutoSize = true;
            this.lblPoNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoNum.Location = new System.Drawing.Point(588, 103);
            this.lblPoNum.Name = "lblPoNum";
            this.lblPoNum.Size = new System.Drawing.Size(73, 13);
            this.lblPoNum.TabIndex = 0;
            this.lblPoNum.Text = "Order Number";
            // 
            // lblPoDate
            // 
            this.lblPoDate.AutoSize = true;
            this.lblPoDate.Location = new System.Drawing.Point(146, 131);
            this.lblPoDate.Name = "lblPoDate";
            this.lblPoDate.Size = new System.Drawing.Size(59, 13);
            this.lblPoDate.TabIndex = 0;
            this.lblPoDate.Text = "Order Date";
            // 
            // lblStaticQuant
            // 
            this.lblStaticQuant.AutoSize = true;
            this.lblStaticQuant.Font = new System.Drawing.Font("Andalus", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblStaticQuant.Location = new System.Drawing.Point(83, 3);
            this.lblStaticQuant.Name = "lblStaticQuant";
            this.lblStaticQuant.Size = new System.Drawing.Size(64, 19);
            this.lblStaticQuant.TabIndex = 0;
            this.lblStaticQuant.Text = "Quantity";
            // 
            // lblStatDescp
            // 
            this.lblStatDescp.AutoSize = true;
            this.lblStatDescp.Font = new System.Drawing.Font("Andalus", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblStatDescp.Location = new System.Drawing.Point(156, 3);
            this.lblStatDescp.Name = "lblStatDescp";
            this.lblStatDescp.Size = new System.Drawing.Size(83, 19);
            this.lblStatDescp.TabIndex = 0;
            this.lblStatDescp.Text = "Description";
            // 
            // lblStatUnitValue
            // 
            this.lblStatUnitValue.AutoSize = true;
            this.lblStatUnitValue.Font = new System.Drawing.Font("Andalus", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblStatUnitValue.Location = new System.Drawing.Point(248, 3);
            this.lblStatUnitValue.Name = "lblStatUnitValue";
            this.lblStatUnitValue.Size = new System.Drawing.Size(77, 19);
            this.lblStatUnitValue.TabIndex = 0;
            this.lblStatUnitValue.Text = "Unit Value";
            // 
            // lblQuant
            // 
            this.lblQuant.AutoSize = true;
            this.lblQuant.Location = new System.Drawing.Point(83, 39);
            this.lblQuant.Name = "lblQuant";
            this.lblQuant.Size = new System.Drawing.Size(46, 13);
            this.lblQuant.TabIndex = 0;
            this.lblQuant.Text = "Quantity";
            // 
            // lblDescp
            // 
            this.lblDescp.AutoSize = true;
            this.lblDescp.Location = new System.Drawing.Point(156, 39);
            this.lblDescp.Name = "lblDescp";
            this.lblDescp.Size = new System.Drawing.Size(60, 13);
            this.lblDescp.TabIndex = 0;
            this.lblDescp.Text = "Description";
            // 
            // lblUnitValue
            // 
            this.lblUnitValue.AutoSize = true;
            this.lblUnitValue.Location = new System.Drawing.Point(248, 39);
            this.lblUnitValue.Name = "lblUnitValue";
            this.lblUnitValue.Size = new System.Drawing.Size(56, 13);
            this.lblUnitValue.TabIndex = 0;
            this.lblUnitValue.Text = "Unit Value";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblGrandTotal);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 631);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 35);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(308, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Grand Total";
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.AutoSize = true;
            this.lblGrandTotal.Location = new System.Drawing.Point(407, 13);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(43, 13);
            this.lblGrandTotal.TabIndex = 0;
            this.lblGrandTotal.Text = "Amount";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetPartial;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 267F));
            this.tableLayoutPanel1.Controls.Add(this.lblAmount, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblUnitValue, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDescp, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblItemCode, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblQuant, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblStatAmount, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatUnitValue, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatDescp, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStaticQuant, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(59, 210);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.832808F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.16719F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(568, 392);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(334, 39);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(43, 13);
            this.lblAmount.TabIndex = 0;
            this.lblAmount.Text = "Amount";
            // 
            // lblItemCode
            // 
            this.lblItemCode.AutoSize = true;
            this.lblItemCode.Location = new System.Drawing.Point(6, 39);
            this.lblItemCode.Name = "lblItemCode";
            this.lblItemCode.Size = new System.Drawing.Size(52, 13);
            this.lblItemCode.TabIndex = 0;
            this.lblItemCode.Text = "ItemCode";
            // 
            // lblStatAmount
            // 
            this.lblStatAmount.AutoSize = true;
            this.lblStatAmount.Font = new System.Drawing.Font("Andalus", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblStatAmount.Location = new System.Drawing.Point(334, 3);
            this.lblStatAmount.Name = "lblStatAmount";
            this.lblStatAmount.Size = new System.Drawing.Size(57, 19);
            this.lblStatAmount.TabIndex = 0;
            this.lblStatAmount.Text = "Amount";
            this.lblStatAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Andalus", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "ItemCode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Order Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(493, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Order Number:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPrint,
            this.toolStripSeparator1,
            this.tsbClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(706, 50);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbPrint
            // 
            this.tsbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrint.Image")));
            this.tsbPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Size = new System.Drawing.Size(87, 47);
            this.tsbPrint.Text = "Print this purchase order";
            this.tsbPrint.Click += new System.EventHandler(this.tsbPrint_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 50);
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(65, 47);
            this.tsbClose.ToolTipText = "Close this form";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // lblSuppName
            // 
            this.lblSuppName.AutoSize = true;
            this.lblSuppName.Location = new System.Drawing.Point(493, 131);
            this.lblSuppName.Name = "lblSuppName";
            this.lblSuppName.Size = new System.Drawing.Size(74, 13);
            this.lblSuppName.TabIndex = 0;
            this.lblSuppName.Text = "Supplier name";
            // 
            // lblSuppAddress
            // 
            this.lblSuppAddress.AutoSize = true;
            this.lblSuppAddress.Location = new System.Drawing.Point(493, 157);
            this.lblSuppAddress.Name = "lblSuppAddress";
            this.lblSuppAddress.Size = new System.Drawing.Size(86, 13);
            this.lblSuppAddress.TabIndex = 0;
            this.lblSuppAddress.Text = "Supplier Address";
            // 
            // PrintPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(706, 666);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSuppAddress);
            this.Controls.Add(this.lblSuppName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPoNum);
            this.Controls.Add(this.lblPoDate);
            this.Controls.Add(this.lblCompName);
            this.Controls.Add(this.lblPo);
            this.Controls.Add(this.lblDU);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(722, 702);
            this.MinimumSize = new System.Drawing.Size(722, 702);
            this.Name = "PrintPO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrintPO";
            this.Load += new System.EventHandler(this.PrintPO_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDU;
        private System.Windows.Forms.Label lblCompName;
        private System.Windows.Forms.Label lblPo;
        private System.Windows.Forms.Label lblPoNum;
        private System.Windows.Forms.Label lblPoDate;
        private System.Windows.Forms.Label lblStaticQuant;
        private System.Windows.Forms.Label lblStatDescp;
        private System.Windows.Forms.Label lblStatUnitValue;
        private System.Windows.Forms.Label lblQuant;
        private System.Windows.Forms.Label lblDescp;
        private System.Windows.Forms.Label lblUnitValue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblStatAmount;
        private System.Windows.Forms.Label lblItemCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.Label lblSuppName;
        private System.Windows.Forms.Label lblSuppAddress;
    }
}