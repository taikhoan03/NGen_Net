﻿namespace NexGen.UserConctrol
{
    partial class doc_rin_input
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtItemNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboxItemType = new NexGen.Controls.Combobox_ItemType();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Qty";
            // 
            // txtQty
            // 
            this.txtQty.Location = new System.Drawing.Point(162, 84);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(100, 22);
            this.txtQty.TabIndex = 9;
            this.txtQty.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Item Number";
            // 
            // txtItemNumber
            // 
            this.txtItemNumber.Location = new System.Drawing.Point(162, 18);
            this.txtItemNumber.Name = "txtItemNumber";
            this.txtItemNumber.Size = new System.Drawing.Size(316, 22);
            this.txtItemNumber.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Price";
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(162, 53);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(100, 22);
            this.txtPrice.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Type:";
            // 
            // cboxItemType
            // 
            this.cboxItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxItemType.FormattingEnabled = true;
            this.cboxItemType.Location = new System.Drawing.Point(162, 120);
            this.cboxItemType.Name = "cboxItemType";
            this.cboxItemType.Size = new System.Drawing.Size(121, 24);
            this.cboxItemType.TabIndex = 10;
            this.cboxItemType.SelectedIndexChanged += new System.EventHandler(this.cboxItemType_SelectedIndexChanged);
            // 
            // doc_rin_input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboxItemType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtItemNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.label1);
            this.Name = "doc_rin_input";
            this.Size = new System.Drawing.Size(560, 263);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtItemNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label label4;
        private Controls.Combobox_ItemType cboxItemType;
    }
}