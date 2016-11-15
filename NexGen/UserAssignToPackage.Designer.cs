namespace NexGen
{
    partial class UserAssignToPackage
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
            this.cboxUsers = new System.Windows.Forms.ComboBox();
            this.cboxPackage = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnDel = new System.Windows.Forms.Button();
            this.lblPackageid = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboxUsers
            // 
            this.cboxUsers.FormattingEnabled = true;
            this.cboxUsers.Location = new System.Drawing.Point(834, 59);
            this.cboxUsers.Name = "cboxUsers";
            this.cboxUsers.Size = new System.Drawing.Size(257, 24);
            this.cboxUsers.TabIndex = 0;
            // 
            // cboxPackage
            // 
            this.cboxPackage.FormattingEnabled = true;
            this.cboxPackage.Location = new System.Drawing.Point(834, 135);
            this.cboxPackage.Name = "cboxPackage";
            this.cboxPackage.Size = new System.Drawing.Size(257, 24);
            this.cboxPackage.TabIndex = 1;
            this.cboxPackage.SelectedIndexChanged += new System.EventHandler(this.cboxPackage_SelectedIndexChanged_1);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(911, 243);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(33, 59);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(773, 268);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(252, 345);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "Delete";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // lblPackageid
            // 
            this.lblPackageid.AutoSize = true;
            this.lblPackageid.Location = new System.Drawing.Point(967, 171);
            this.lblPackageid.Name = "lblPackageid";
            this.lblPackageid.Size = new System.Drawing.Size(46, 17);
            this.lblPackageid.TabIndex = 5;
            this.lblPackageid.Text = "label1";
            // 
            // UserAssignToPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 741);
            this.Controls.Add(this.lblPackageid);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cboxPackage);
            this.Controls.Add(this.cboxUsers);
            this.Name = "UserAssignToPackage";
            this.Text = "UserAssignToPackage";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboxUsers;
        private System.Windows.Forms.ComboBox cboxPackage;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Label lblPackageid;
    }
}